using BusinessObject.Models;
using DataAccess.IRepositories;
using DataAccess.Managements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public void Add(BookingReservation booking)
        {
            BookingManagement.Instance.Add(booking);
        }

        public void Delete(BookingReservation booking)
        {
            BookingManagement.Instance.Delete(booking);
        }

        public IEnumerable<BookingReservation> GetAll()
        {
            return BookingManagement.Instance.GetAll();
        }

        public void Update(BookingReservation booking)
        {
            BookingManagement.Instance.Update(booking);
        }
    }
}
