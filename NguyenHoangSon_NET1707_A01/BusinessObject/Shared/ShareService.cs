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

        private bool IsValidInputFields(CustomerView view)
        {
            if (string.IsNullOrEmpty(view.CustomerId.ToString()) &&
                string.IsNullOrEmpty(view.CustomerFullName.ToString()) &&
                string.IsNullOrEmpty(view.CustomerBirthday.ToString()) &&
                string.IsNullOrEmpty(view.CustomerStatus.ToString()) &&
                string.IsNullOrEmpty(view.EmailAddress.ToString()) &&
                string.IsNullOrEmpty(view.Telephone.ToString()))
            {
                return false;
            }
            return true;
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
