using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Shared
{
    public class ServiceProcess
    {
        public static byte GetStatus(string status)
        {
            return status == "Active" ? Convert.ToByte(1) : Convert.ToByte(2);
        }

        public static string SetStatus(byte status)
        {
            return status == Convert.ToByte(1) ? "Active" : "Deleted";
        }
    }
}
