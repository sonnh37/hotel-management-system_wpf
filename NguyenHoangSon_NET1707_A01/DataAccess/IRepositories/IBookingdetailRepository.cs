using BusinessObject.Models;
using BusinessObject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IBookingDetailRepository
    {
        void Add(BookingDetail booking);
        void Delete(BookingDetail booking);
        IEnumerable<BookingDetail> GetAllByBookingAndRoomId(BookingDetail bookingDetail);
        IEnumerable<BookingDetail> GetAllByFilter(BookingDetailView filter);
        BookingDetail GetByBookingAndRoomId(int bookingId, int roomId);
        void Update(BookingDetail booking);
    }
}
