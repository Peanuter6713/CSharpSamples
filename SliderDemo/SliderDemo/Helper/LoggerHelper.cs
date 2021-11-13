using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliderDemo.Helper
{
    public class LoggerHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("Config/log4net.config");

        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

    }
}
