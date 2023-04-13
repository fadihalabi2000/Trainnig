using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.MyLogger
{
    public static class ExtantionLogger
    {
        public static async Task LogErorr(this IMyLogger logger, string message, int id, string role) 
        {
           await logger.log(message, "Erorr", id, role);
        }
        public static async Task LogWarning(this IMyLogger logger, string message, int id, string role)
        {
           await logger.log(message, "Warning", id, role);
        }
        public static async Task LogInformation(this IMyLogger logger, string message,int id ,string role)
        {
          await  logger.log(message, "Information",id,role);
        }
    }
}
