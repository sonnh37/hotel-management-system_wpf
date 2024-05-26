using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenHoangSonWPF
{
    internal class Session
    {
        public static string? Username { get; set; } = null;
        public static List<RoomInformation> carts { get; set; } = null;
        public static string Role { get; set; } = null;
    }
}
