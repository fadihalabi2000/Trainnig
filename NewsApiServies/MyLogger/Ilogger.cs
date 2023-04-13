using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.MyLogger
{
   public interface IMyLogger
    {
        //void log(string message);
        Task log(string message, String messagetype, int id, string role);
    }
}
