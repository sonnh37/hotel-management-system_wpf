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
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public void Add(BookingDetail booking)
        {
            BookingDetailManagement.Instance.Add(booking);
        }

        public void Delete(BookingDetail booking)
        {
            BookingDetailManagement.Instance.Delete(booking);
        }

        public IEnumerable<BookingDetail> GetAllByBookingAndRoomId(BookingDetail bookingDetail)
        {
            return BookingDetailManagement.Instance.GetAllByBookingAndRoomId(bookingDetail);
        }

        public void Update(BookingDetail booking)
        {
            BookingDetailManagement.Instance.Update(booking);
        }
        public BookingDetail GetByBookingAndRoomId(int bookingId, int roomId)
        {
            return BookingDetailManagement.Instance.GetByBookingAndRoomId(bookingId, roomId);
        }

        public IEnumerable<BookingDetail> GetAllByFilter(BookingDetailView filter)
        {
            if (filter.StartDate == null || filter.EndDate == null)
            {
                if (filter.BookingReservationId != null)
                {
                    return GetAllByBookingAndRoomId(new BookingDetail { BookingReservationId = (int)filter.BookingReservationId});
                }
            }

            IEnumerable<BookingDetail> bookingDetails = BookingDetailManagement.Instance.FindAll(model =>
                (filter.BookingReservationId == null || model.BookingReservationId == filter.BookingReservationId) &&
                (filter.StartDate == null || model.StartDate.Equals(filter.StartDate) &&
                (filter.EndDate == null || model.EndDate.Equals(filter.EndDate))));
            
            return bookingDetails;
        }

    }
}
