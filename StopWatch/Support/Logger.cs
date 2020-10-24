using log4net;
using log4net.Config;

namespace StopWatch.Support
{
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger("Protocol");


        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
