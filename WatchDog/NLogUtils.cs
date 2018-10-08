using System.IO;
using System.Windows.Forms;
using NLog;

namespace WatchDog
{
    class NLogUtils
    {
        private static Logger _namedLogger;


        public static Logger NamedLogger
        {
            get
            {
                if (_namedLogger == null)
                {
                    var path = Path.GetDirectoryName(Application.ExecutablePath);
                    _namedLogger = LogManager.GetLogger("WatchdogServer");    
                }
                return _namedLogger;
            }
        }
    }
}
