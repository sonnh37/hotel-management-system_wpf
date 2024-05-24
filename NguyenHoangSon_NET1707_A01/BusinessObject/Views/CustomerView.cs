using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Views
{
    public class CustomerView
    {
        public int? CustomerId { get; set; }
        public string? CustomerFullName { get; set; }
        public string? Telephone { get; set; }
        public string? EmailAddress { get; set; } = null!;
        public DateTime? CustomerBirthday { get; set; }
        public string? CustomerStatus { get; set; }
        public string? Password { get; set; }
        //public IList<BookingReservation> BookingReservations { get; set; }
    }
}
