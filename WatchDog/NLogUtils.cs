using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    //if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) Directory.SetCurrentDirectory(path);

                    _namedLogger = LogManager.GetLogger("WatchdogServer");
     
                }
                return _namedLogger;
            }
        }
    }
}
