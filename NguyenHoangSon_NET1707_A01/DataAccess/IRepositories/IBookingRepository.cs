using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IBookingRepository 
    {
        IEnumerable<BookingReservation> GetAll();
        void Add(BookingReservation booking);
        void Update(BookingReservation booking);
        void Delete(BookingReservation booking);
    }
}
