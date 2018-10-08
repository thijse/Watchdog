using System;
using System.Diagnostics;

namespace WatchdogClient
{
    public class Heartbeat
    {
        private const string PipeName = "named_pipe_watchdog";
        private static Heartbeat _instance;
        private readonly NamedPipeClient<string> _client = new NamedPipeClient<string>(PipeName);
        private Stopwatch _stopwatchHeartBeat;

        private Heartbeat()
        {
            ProcessName = Process.GetCurrentProcess().ProcessName;
            Initialize();
        }

        public string ProcessName { get; set; }
        public uint Timeout { get; private set; }


        public static Heartbeat Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Heartbeat();
                }
                return _instance;
            }
        }

        private void Initialize()
        {
            Timeout = 5;
            _client.ServerMessage += OnServerMessage;
            _client.Disconnected += OnDisconnected;
            _client.Connected += OnConnected;
            _client.AutoReconnect = true;
            _client.Start();
            _stopwatchHeartBeat = new Stopwatch();
            _stopwatchHeartBeat.Start();
        }

        public void Start()
        {
            _stopwatchHeartBeat.Start();
        }

        public void Stop()
        {
            _stopwatchHeartBeat.Stop();
        }

        private void OnServerMessage(NamedPipeConnection<string, string> connection, string message)
        {
            var args = message.Split(',');
            if (args.Length == 0) return;
            uint command;
            if (!uint.TryParse(args[0], out command)) return;

            switch (command)
            {
                case (int) Commands.SetTimeOut:
                    if (args.Length < 2) return;
                    uint timeout;
                    if (uint.TryParse(args[1], out timeout))
                    {
                        Timeout = timeout;
                    }
                    break;
            }
        }

        public void SendHeartbeat(string processName = "")
        {
            if (_stopwatchHeartBeat.ElapsedMilliseconds > 25)
            {
                _stopwatchHeartBeat.Restart();
                SendCommand(Commands.Heartbeat, processName != "" ? processName : ProcessName);
            }
        }



        public void RequestKill()
        {
            SendCommand(Commands.RequestKill, ProcessName);
        }

        public void RequestKill(uint sec)
        {
            SendCommand(Commands.RequestKill, ProcessName, sec);
        }

        public void RequestKill(TimeSpan time)
        {
            SendCommand(Commands.RequestKill, ProcessName, time.TotalSeconds);
        }

        private void OnDisconnected(NamedPipeConnection<string, string> connection)
        {
        }

        private void OnConnected(NamedPipeConnection<string, string> connection)
        {
            SendCommand(Commands.Heartbeat, ProcessName);
        }

        //private void SendCommand(Commands command, string[] arguments)
        //{
        //    var commandString = new string[] { command.ToString(), ""};
        //    _client.PushMessage(command.ToString()+","+ string.Join(",", commandString));
        //}

        //private void SendCommand(Commands command)
        //{
        //    _client.PushMessage(command.ToString());
        //}
        private void SendCommand<T>(Commands command, T argument)
        {
            _client.PushMessage((int) command + "," + argument);
        }

        private void SendCommand<T1, T2>(Commands command, T1 argument1, T2 argument2)
        {
            _client.PushMessage((int) command + "," + argument1 + "," + argument2);
        }

        private enum Commands
        {
            SetTimeOut,
            Heartbeat,
            RequestKill
        }
    }
}