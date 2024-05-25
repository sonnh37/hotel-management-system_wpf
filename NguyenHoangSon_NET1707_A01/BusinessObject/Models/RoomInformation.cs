using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class RoomInformation : BaseModel
    {
        public RoomInformation()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public string? RoomDetailDescription { get; set; }
        public int? RoomMaxCapacity { get; set; }
        public int? RoomTypeId { get; set; }
        public byte? RoomStatus { get; set; }
        public decimal? RoomPricePerDay { get; set; }

        public virtual RoomType RoomType { get; set; } = null!;
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
