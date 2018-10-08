using Topshelf;

namespace WatchdogService
{
    public class Program
    {
        public static void Main(string[] args)
        {
                var host = HostFactory.New(x =>
                {
                    x.Service<WatchdogStarterService>(s =>
                    {
                        s.ConstructUsing(name => new WatchdogStarterService());
                        s.WhenStarted(tc => tc.Start());              
                        s.WhenStopped(tc => tc.Stop());               
                    });
 
                    x.RunAsLocalSystem();
                    x.SetDescription("Starts watchdog in system tray and restarts it when exited");
                    x.SetDisplayName("WatchdogStarter");
                    x.SetServiceName("WatchdogStarter");
                    x.StartAutomatically();

                    x.EnableServiceRecovery(r =>
                    {
                        r.RestartService(1);
                        r.SetResetPeriod(1);
                    });


                });
 
    host.Run();
        }


    }
}
