using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Views
{
    public class BookingView
    {
        public int? BookingReservationId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? CustomerId { get; set; }
        public string? BookingStatus { get; set; }

        public CustomerView Customer { get; set; } = null!;
        public IList<BookingDetailView> BookingDetails { get; set; }
    }
}
