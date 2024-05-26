using BusinessObject.Context;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Managements
{
    public class BookingDetailManagement : BaseManagement<BookingDetail>
    {
        private static BookingDetailManagement instance = null;
        private static readonly object instanceLock = new object();
        private FUMiniHotelManagementContext _context;

        public BookingDetailManagement(FUMiniHotelManagementContext context) : base(context)
        {
            _context = context;
        }

        public static BookingDetailManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDetailManagement(new FUMiniHotelManagementContext());
                    }
                    return instance;
                }
            }
        }

        public void Add(BookingDetail bookingDetail)
        {

            BookingDetail p = FindOne(item => item.BookingReservationId.Equals(bookingDetail.BookingReservationId) && item.RoomId.Equals(bookingDetail.RoomId));
            if (p == null)
            {
                base.Add(bookingDetail);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The bookingDetail is already exist");
            }
        }

        public void Delete(BookingDetail bookingDetail)
        {
            BookingDetail p = FindOne(item => item.BookingReservationId.Equals(bookingDetail.BookingReservationId) && item.RoomId.Equals(bookingDetail.RoomId));
            if (p != null)
            {
                base.Delete(bookingDetail);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The bookingDetail does not exist");
            }

        }

        public BookingDetail FindOne(Expression<Func<BookingDetail, bool>> predicate)
        {
            BookingDetail bookingDetail = null;
            try
            {
                bookingDetail = _context.BookingDetails.SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return bookingDetail;
        }

        public IEnumerable<BookingDetail> FindAll(Expression<Func<BookingDetail, bool>> predicate)
        {
            List<BookingDetail> bookingDetails = new List<BookingDetail>();
            bookingDetails = base.GetAll(predicate);

            return bookingDetails;
        }

        public IEnumerable<BookingDetail> GetAllByBookingAndRoomId(BookingDetail bookingDetail)
        {
            var queryable = GetQueryable(model => model.BookingReservationId == bookingDetail.BookingReservationId);
            queryable = queryable.Include(model => model.Room);
            queryable = queryable.Include(model => model.BookingReservation);

            return queryable.ToList();
        }

        public void Update(BookingDetail bookingDetail)
        {
            BookingDetail p = FindOne(item => item.BookingReservationId.Equals(bookingDetail.BookingReservationId) && item.RoomId.Equals(bookingDetail.RoomId));
            if (p != null)
            {
                base.Update(p);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The bookingDetail does not exist");
            }
        }
        public BookingDetail GetByBookingAndRoomId(int bookingId, int roomId)
        {
            var queryable = base.GetQueryable<BookingDetail>();

            return queryable.Where(cus => cus.BookingReservationId == bookingId && cus.RoomId == roomId).SingleOrDefault();
        }
    }
}
