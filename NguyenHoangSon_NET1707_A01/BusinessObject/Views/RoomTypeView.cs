using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Views
{
    public class RoomTypeView
    {
        public int? RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; } = null!;
        public string? TypeDescription { get; set; }
        public string? TypeNote { get; set; }

        public IList<RoomView> RoomInformations { get; set; }
    }
}
