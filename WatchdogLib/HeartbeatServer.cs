using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace WatchdogLib
{
    public class HeartbeatClient
    {
        public HeartbeatClient(string name)
        {
            Name = name;          
            LogManager.GetLogger("WatchdogServer");
            Restart();
        }

        public void Reset()
        {
            RequestKill = false;
            LastHeartbeat = DateTime.Now;
        }

        public void Restart()
        {
            Tries = 0;
            RequestKill = false;
            LastHeartbeat = DateTime.Now;
        }

        public string Name            { get; set; }
        public string ProcessName     { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public bool RequestKill       { get; set; }
        public DateTime KillTime      { get; set; }
        public int Tries              { get; set; }
    }


    public class HeartbeatServer
    {
        private enum Commands
        {
            SetTimeOut,
            Heartbeat,
            RequestKill,
        }

        private const string PipeName = "named_pipe_watchdog";
        private readonly NamedPipeServer<string> _server  = new NamedPipeServer<string>(PipeName);
        private readonly ISet<HeartbeatClient>   _clients = new HashSet<HeartbeatClient>();
        private DateTime _serverStarted;

        public ISet<HeartbeatClient> Clients    { get { return _clients; } }

        private static HeartbeatServer _instance;

        public static HeartbeatServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HeartbeatServer();
                }
                return _instance;
            }
        }

        public Logger Logger { get; set; }
        

        private HeartbeatServer()
        {
            Logger                      = LogManager.GetLogger("WatchdogServer");
            _server.ClientConnected    += OnClientConnected;
            _server.ClientDisconnected += OnClientDisconnected;
            _server.ClientMessage      += OnClientMessage;
            _serverStarted              = DateTime.Now;
            _server.Start();                        
        }

        public void Restart()
        {
            _serverStarted              = DateTime.Now;
            Clients.Clear();
            //foreach (var client in Clients)
            //{
            //    client.Restart();
            //}
        }

        private void OnClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
            var args = message.Split(',');
            if (args.Length == 0) return;
            uint command; if (!uint.TryParse(args[0], out command)) return;

            switch (command)
            {

                case (int)Commands.Heartbeat:
                    {
                        var client = FindByName(connection.Name);
                        if (!string.IsNullOrEmpty(connection.Name) && client == null)                        
                        {
                            _clients.Add(new HeartbeatClient(connection.Name));
                        }
                        if (args.Length < 2) return;
                        client.ProcessName = args[1];
                        client.LastHeartbeat = DateTime.Now;
                        Debug.WriteLine("received heartbeat");
                    }
                    break;
                case (int)Commands.RequestKill:
                    {
                        var client = FindByName(connection.Name);
                        if (!string.IsNullOrEmpty(connection.Name) && client == null)                        
                        {
                            _clients.Add(new HeartbeatClient(connection.Name));
                        }


                        if (args.Length < 2) return;
                        client.ProcessName = args[1];
                        
                        if (args.Length == 3)
                        {
                            uint delay;
                            if (!uint.TryParse(args[2], out delay)) return;
                            client.KillTime = DateTime.Now + TimeSpan.FromSeconds(delay);
                            Logger.Warn("Received kill after {0} seconds request by Process {1}", delay, client.ProcessName);
                            //Debug.WriteLine("received delayed kill");
                        }
                        else
                        {
                            client.KillTime = DateTime.Now ;
                            Logger.Warn("Received kill request by Process {0}", client.ProcessName);
                            //Debug.WriteLine("received  kill");
                        }
                        client.RequestKill = true;
                    }
                    break;
                default:
                    Debug.WriteLine("Unrecognized command");
                    break;
            }
        }

        private void OnClientConnected(NamedPipeConnection<string, string> connection)
        {
            if (FindByName(connection.Name) == null)
            {                
                _clients.Add(new HeartbeatClient(connection.Name));
            }
        }

        private void OnClientDisconnected(NamedPipeConnection<string, string> connection)
        {
            var client = FindByName(connection.Name);
            if (client != null) _clients.Remove(client);
        }

        public HeartbeatClient FindByName(string name)
        {
            return _clients.FirstOrDefault((client) => client.Name == name);
        }

        public HeartbeatClient FindByProcessName(string processName)
        {
            return _clients.FirstOrDefault((client) => client.ProcessName == processName);
        }

        public bool HeartbeatTimedOut(string processName, uint timeout)
        {
            var client = FindByProcessName(processName);
            if (client == null)
            {
                // No process with this name connected, so no timeout
                return false;
            }
            else
            {
                //if ((DateTime.Now - _serverStarted).TotalSeconds < 2 * timeout)
                //{
                //    // Server is not running long enough to have reliably received a heartbeat
                //    return false;
                //}
                //else
                {
                    // Check if last heartbeat was before timeout
                    return ((DateTime.Now - client.LastHeartbeat).TotalSeconds > timeout);
                }
            }
        }

        public bool Connected(string processName)
        {
            var client = FindByProcessName(processName);
            return (client != null);
        }

        public bool KillRequested(string processName)
        {
            var client = FindByProcessName(processName);
            if (client == null) return false;
            var performKill =  client.RequestKill && (DateTime.Now > client.KillTime );
            if (performKill ) client.RequestKill = false; // Kill request only returns true once
            return performKill;
        }

        //private void SendCommand(Commands command, string[] arguments)
        //{
        //    var commandString = new string[] { command.ToString(), "" };
        //    _server.PushMessage(command.ToString() + "," + string.Join(",", commandString));
        //}

        //private void SendCommand(Commands command)
        //{
        //    _server.PushMessage(command.ToString());
        //}
        private void SendCommand<T>(Commands command, T argument)
        {
            _server.PushMessage(((int)command).ToString() + "," + argument.ToString());
        }

        //private void SendCommand<T1, T2>(Commands command, T1 argument1, T2 argument2)
        //{
        //    _server.PushMessage(command.ToString() + "," + argument1.ToString() + "," + argument2.ToString());
        //}

    }
}
