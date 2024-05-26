using BusinessObject.Context;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Managements
{
    public class BookingManagement : BaseManagement<BookingReservation>
    {
        private static BookingManagement instance = null;
        private static readonly object instanceLock = new object();
        private FUMiniHotelManagementContext _context;
        
        public BookingManagement(FUMiniHotelManagementContext context) : base(context)
        {
            _context = context;
        }

        public static BookingManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingManagement(new FUMiniHotelManagementContext());
                    }
                    return instance;
                }
            }
        }

        public void Add(BookingReservation booking)
        {

            BookingReservation p = FindOne(item => item.BookingReservationId.Equals(booking.BookingReservationId));
            if (p == null)
            {
                base.Add(booking);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The booking is already exist");
            }
        }

        public void Delete(BookingReservation booking)
        {
            BookingReservation p = FindOne(item => item.BookingReservationId.Equals(booking.BookingReservationId));
            if (p != null)
            {
                base.Delete(booking);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The booking does not exist");
            }

        }

        public BookingReservation FindOne(Expression<Func<BookingReservation, bool>> predicate)
        {
            BookingReservation booking = null;
            try
            {
                booking = _context.BookingReservations.SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return booking;
        }

        public IEnumerable<BookingReservation> FindAll(Expression<Func<BookingReservation, bool>> predicate)
        {
            List<BookingReservation> bookings = new List<BookingReservation>();
            bookings = base.GetAll(predicate);

            return bookings;
        }

        public IEnumerable<BookingReservation> GetAll()
        {
            var queryable = GetQueryable(model => model.BookingStatus == Convert.ToByte(1));
            queryable = queryable.Include(model => model.Customer);
            queryable = queryable.Include(model => model.BookingDetails);

            return queryable.ToList();
        }

        public void Update(BookingReservation booking)
        {
            BookingReservation p = FindOne(item => item.BookingReservationId.Equals(booking.BookingReservationId));
            if (p != null)
            {
                base.Update(p);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The booking does not exist");
            }
        }
        public BookingReservation GetById(int id)
        {
            var queryable = base.GetQueryable<BookingReservation>();

            return queryable.Where(cus => cus.BookingReservationId == id).SingleOrDefault();
        }
    }
}
