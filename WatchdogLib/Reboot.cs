using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace WatchdogLib
{
    public class Reboot
    {
        public static TimeSpan GetUptime()
        {
            using (var uptime = new PerformanceCounter("System", "System Up Time")) {
                uptime.NextValue();       //Call this an extra time before reading its value
                return TimeSpan.FromSeconds(uptime.NextValue());
            }
        }

        public static TimeSpan GetUptime2()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }

        [DllImport("kernel32")]
        static extern ulong GetTickCount64();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(ShutdownType uFlags, ShutdownReason dwReason);

        public static bool Shutdown(ShutdownType shutdownType, ForceExit forceExit, ShutdownReason reason = ShutdownReason.MinorOther, bool setShutdownPrivilege=true)
        {
            if (setShutdownPrivilege && !TokenAdjuster.EnablePrivilege("SeShutdownPrivilege", true))
            {
                return false;
            }

            var mode = (ShutdownType)((int)shutdownType + (int)forceExit);
            return ExitWindowsEx(mode, reason) != 0;
        }
    }

    [Flags]
    public enum ShutdownType : uint
    {
        // ONE of the following:
        LogOff         = 0x00,
        ShutDown       = 0x01,
        Reboot         = 0x02,
        PowerOff       = 0x08,
        RestartApps    = 0x40,
        HybridShutdown = 400000,
        // plus AT MOST ONE of the following two:
        //Force        = 0x04,
        //ForceIfHung  = 0x10
    }

    public enum ForceExit : uint
    {
        Normal         = 0x00,
        Force          = 0x04,
        ForceIfHung    = 0x10
    }


    [Flags]
    public enum ShutdownReason : uint
    {
        None                      = 0,
        MajorApplication          = 0x00040000,
        MajorHardware             = 0x00010000,
        MajorLegacyApi            = 0x00070000,
        MajorOperatingSystem      = 0x00020000,
        MajorOther                = 0x00000000,
        MajorPower                = 0x00060000,
        MajorSoftware             = 0x00030000,
        MajorSystem               = 0x00050000,

        MinorBlueScreen           = 0x0000000F,
        MinorCordUnplugged        = 0x0000000b,
        MinorDisk                 = 0x00000007,
        MinorEnvironment          = 0x0000000c,
        MinorHardwareDriver       = 0x0000000d,
        MinorHotfix               = 0x00000011,
        MinorHung                 = 0x00000005,
        MinorInstallation         = 0x00000002,
        MinorMaintenance          = 0x00000001,
        MinorMMC                  = 0x00000019,
        MinorNetworkConnectivity  = 0x00000014,
        MinorNetworkCard          = 0x00000009,
        MinorOther                = 0x00000000,
        MinorOtherDriver          = 0x0000000e,
        MinorPowerSupply          = 0x0000000a,
        MinorProcessor            = 0x00000008,
        MinorReconfig             = 0x00000004,
        MinorSecurity             = 0x00000013,
        MinorSecurityFix          = 0x00000012,
        MinorSecurityFixUninstall = 0x00000018,
        MinorServicePack          = 0x00000010,
        MinorServicePackUninstall = 0x00000016,
        MinorTermSrv              = 0x00000020,
        MinorUnstable             = 0x00000006,
        MinorUpgrade              = 0x00000003,
        MinorWMI                  = 0x00000015,

        FlagUserDefined           = 0x40000000,
        FlagPlanned               = 0x80000000
    }

    public sealed class TokenAdjuster
    {
        // PInvoke stuff required to set/enable security privileges
        private const int SE_PRIVILEGE_ENABLED      = 0x00000002;
        private const int TOKEN_ADJUST_PRIVILEGES   = 0X00000020;
        private const int TOKEN_QUERY               = 0X00000008;
        private const int TOKEN_ALL_ACCESS          = 0X001f01ff;
        private const int PROCESS_QUERY_INFORMATION = 0X00000400;

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern int OpenProcessToken(
            IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
            );

        [DllImport("kernel32", SetLastError = true),
         SuppressUnmanagedCodeSecurity]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int AdjustTokenPrivileges(
            IntPtr TokenHandle,
            int DisableAllPrivileges,
            IntPtr NewState,
            int BufferLength,
            IntPtr PreviousState,
            ref int ReturnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LookupPrivilegeValue(
            string lpSystemName,
            string lpName,
            ref LUID lpLuid);

        public static bool EnablePrivilege(string lpszPrivilege, bool bEnablePrivilege)
        {
            var retval            = false;
            var ltkpOld           = 0;
            var hToken            = IntPtr.Zero;
            var tkp               = new TOKEN_PRIVILEGES();
            tkp.Privileges        = new int[3];
            var tkpOld            = new TOKEN_PRIVILEGES();
            tkpOld.Privileges     = new int[3];
            var tLUID             = new LUID();
            tkp.PrivilegeCount    = 1;
            if (bEnablePrivilege)
                tkp.Privileges[2] = SE_PRIVILEGE_ENABLED;
            else
                tkp.Privileges[2] = 0;
            if (LookupPrivilegeValue(null, lpszPrivilege, ref tLUID))
            {
                var proc = Process.GetCurrentProcess();
                if (proc.Handle != IntPtr.Zero)
                {
                    if (OpenProcessToken(proc.Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,
                        ref hToken) != 0)
                    {
                        tkp.PrivilegeCount  = 1;
                        tkp.Privileges[2]   = SE_PRIVILEGE_ENABLED;
                        tkp.Privileges[1]   = tLUID.HighPart;
                        tkp.Privileges[0]   = tLUID.LowPart;
                        const int bufLength = 256;
                        var tu              = Marshal.AllocHGlobal(bufLength);
                        Marshal.StructureToPtr(tkp, tu, true);
                        if (AdjustTokenPrivileges(hToken, 0, tu, bufLength, IntPtr.Zero, ref ltkpOld) != 0)
                        {
                            // successful AdjustTokenPrivileges doesn't mean privilege could be changed
                            if (Marshal.GetLastWin32Error() == 0)
                            {
                                retval = true; // Token changed
                            }
                        }
                        var tokp = (TOKEN_PRIVILEGES) Marshal.PtrToStructure(tu, typeof(TOKEN_PRIVILEGES));
                        Marshal.FreeHGlobal(tu);
                    }
                }
            }
            if (hToken != IntPtr.Zero)
            {
                CloseHandle(hToken);
            }
            return retval;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LUID
        {
            internal int LowPart;
            internal int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID_AND_ATTRIBUTES
        {
            private readonly LUID Luid;
            private readonly int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TOKEN_PRIVILEGES
        {
            internal int PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] internal int[] Privileges;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _PRIVILEGE_SET
        {
        private int PrivilegeCount;
        private int Control;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] // ANYSIZE_ARRAY = 1
        private LUID_AND_ATTRIBUTES[] Privileges;
    }
}

}