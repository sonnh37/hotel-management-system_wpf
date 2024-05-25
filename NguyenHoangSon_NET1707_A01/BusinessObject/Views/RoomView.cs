using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Views
{
    public class RoomView
    {
        public int? RoomId { get; set; }
        public string? RoomNumber { get; set; } = null!;
        public string? RoomDetailDescription { get; set; }
        public int? RoomMaxCapacity { get; set; }
        public int? RoomTypeId { get; set; }
        public string? RoomStatus { get; set; }
        public decimal? RoomPricePerDay { get; set; }

        public RoomTypeView RoomType { get; set; } = null!;
        //public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
