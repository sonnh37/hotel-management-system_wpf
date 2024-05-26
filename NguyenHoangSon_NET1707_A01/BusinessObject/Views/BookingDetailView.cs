using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Views
{
    public class BookingDetailView
    {
        public int? BookingReservationId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ActualPrice { get; set; }

        public BookingView BookingReservation { get; set; } = null!;
        public RoomView Room { get; set; } = null!;
    }
}
