using BusinessObject.Models;
using BusinessObject.Views;
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
        public BookingReservation GetById(int id)
        {
            return BookingManagement.Instance.GetById(id);
        }

        public IEnumerable<BookingReservation> GetAllByFilter(BookingView filter)
        {
            return filter != null ? BookingManagement.Instance.FindAll(BookingReservation =>
                (filter.BookingReservationId == null || BookingReservation.BookingReservationId.Equals(filter.BookingReservationId)) &&
                (filter.CustomerId == null || BookingReservation.CustomerId == filter.CustomerId) &&
                (filter.BookingDate == null || BookingReservation.BookingDate.Equals(filter.BookingDate)))
            : GetAll();
        }
    }
}
