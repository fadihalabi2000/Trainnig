using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.MyLogger
{
    public static class ExtantionLogger
    {
        public static void LogErorr(this Ilogger logger, string message) 
        {
            logger.log(message, "Erorr");
        }
        public static void LogWarning(this Ilogger logger, string message)
        {
            logger.log(message, "Warning");
        }
        public static void LogInformation(this Ilogger logger, string message)
        {
            logger.log(message, "Information");
        }
    }
}
