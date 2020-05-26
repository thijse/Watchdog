using System;
using System.Diagnostics;

namespace WatchdogClient
{

    public class Heartbeat
    {
        private const string PipeName = "named_pipe_watchdog";
        private readonly NamedPipeClient<string> _client = new NamedPipeClient<string>(PipeName);
        private readonly string _processName;
        private Stopwatch _stopwatchHeartBeat;
        public uint Timeout { get; private set; }
        public Heartbeat(string processName)
        {
            _processName = processName;
            Initialize();
        }

        public Heartbeat()
        {
            _processName = Process.GetCurrentProcess().ProcessName;
            Initialize();
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

        private void OnServerMessage(NamedPipeConnection<string, string> connection, string message)
        {
            var args = message.Split(',');
            if (args.Length ==0 ) return;
            uint command; if (!uint.TryParse(args[0], out command)) return;

            switch (command)
            {
                case (int) Commands.SetTimeOut:
                    if (args.Length < 2) return;
                    uint timeout;
                    if (uint.TryParse(args[1], out timeout)) { Timeout = timeout; }
                    break;
            }
        }

        public void SendHeartbeat()
        {

            if (_stopwatchHeartBeat.ElapsedMilliseconds > 25)
            {
                _stopwatchHeartBeat.Restart();
                SendCommand(Commands.Heartbeat, _processName);                
            }
        }

        public void RequestKill()
        {
            SendCommand(Commands.RequestKill, _processName);
        }

        public void RequestKill(uint sec)
        {
            SendCommand(Commands.RequestKill, _processName,sec);
        }

        public void RequestKill(TimeSpan time)
        {
            SendCommand(Commands.RequestKill, _processName,time.TotalSeconds);
        }


        private void OnDisconnected(NamedPipeConnection<string, string> connection)
        {
        }

        private void OnConnected(NamedPipeConnection<string, string> connection)
        {
            SendCommand(Commands.Heartbeat, _processName);
        }


        private enum Commands
        {
            SetTimeOut,
            Heartbeat,
            RequestKill,
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
            _client.PushMessage(((int)command).ToString() + "," +argument.ToString());
        }

        private void SendCommand<T1,T2>(Commands command, T1 argument1, T2 argument2)
        {
            _client.PushMessage(((int)command).ToString() + "," + argument1.ToString() + "," + argument2.ToString());
        }
    }
}
