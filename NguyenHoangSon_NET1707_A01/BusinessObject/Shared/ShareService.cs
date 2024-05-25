using BusinessObject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BusinessObject.Shared
{
    public class ShareService
    {
        public static byte GetStatus(string status)
        {
            return status == "Active" ? Convert.ToByte(1) : Convert.ToByte(2);
        }

        public static string SetStatus(byte status)
        {
            return status == Convert.ToByte(1) ? "Active" : "Deleted";
        }

        public bool IsDecimalFormat(string input)
        {
            Decimal dummy;
            return Decimal.TryParse(input, out dummy);
        }

        public static List<string> GetStatuses()
        {
            return new List<string>
            {
                "Active",
                "Deleted"
            };
        }
    }
}
