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
    public class CustomerManagement : BaseManagement<Customer>
    {
        private static CustomerManagement instance = null;
        private static readonly object instanceLock = new object();
        private FUMiniHotelManagementContext _context;
        
        public CustomerManagement(FUMiniHotelManagementContext context) : base(context)
        {
            _context = context;
        }

        public static CustomerManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerManagement(new FUMiniHotelManagementContext());
                    }
                    return instance;
                }
            }
        }

        public void Add(Customer customer)
        {

            Customer p = FindOne(item => item.CustomerId.Equals(customer.CustomerId));
            if (p == null)
            {
                base.Add(customer);
            }
            else
            {
                throw new Exception("The customer is already exist");
            }
        }

        public void Delete(Customer customer)
        {
            Customer p = FindOne(item => item.CustomerId.Equals(customer.CustomerId));
            if (p != null)
            {
                base.Delete(customer);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The customer does not exist");
            }

        }

        public Customer FindOne(Expression<Func<Customer, bool>> predicate)
        {
            Customer customer = null;
            try
            {
                var queryable = base.GetQueryable(predicate);
                queryable = queryable.Include(model => model.BookingReservations);
                customer = queryable.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return customer;
        }

        public IEnumerable<Customer> FindAll(Expression<Func<Customer, bool>> predicate)
        {
            List<Customer> customers = new List<Customer>();
            customers = base.GetAll(predicate);
            return customers;
        }

        public IEnumerable<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            customers = base.GetAll(model => model.CustomerStatus == Convert.ToByte(1));

            return customers;
        }
        public Customer GetById(int id)
        {
            var queryable = base.GetQueryable<Customer>();
            queryable = queryable.Include(query => query.BookingReservations);
            return queryable.Where(cus => cus.CustomerId == id).SingleOrDefault();
        }

        public void Update(Customer customer)
        {
            Customer p = FindOne(item => item.CustomerId == customer.CustomerId);
            if (p != null)
            {
                base.Update(p);
            }
            else
            {
                throw new Exception("The customer does not exist");
            }
        }
    }
}
