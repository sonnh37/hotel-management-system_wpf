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
    public class CustomerRepository : ICustomerRepository
    {
        public void Add(Customer customer)
        {
            CustomerManagement.Instance.Add(customer);
        }

        public void Delete(Customer customer)
        {
            CustomerManagement.Instance.Delete(customer);
        }

        public IEnumerable<Customer> GetAll()
        {
            return CustomerManagement.Instance.GetAll();
        }

        public void Update(Customer customer)
        {
            CustomerManagement.Instance.Update(customer);
        }

        public Customer GetById(int id)
        {
            return CustomerManagement.Instance.GetById(id);
        }

        public IEnumerable<Customer> GetAllByFilter(CustomerView filter)
        {
            if (filter != null)
            {
                return CustomerManagement.Instance.FindAll
                    (customer => (filter.CustomerId == null || customer.CustomerId.Equals(filter.CustomerId)) &&
                    (filter.CustomerFullName == null || customer.CustomerFullName.ToLower().Trim().Contains(filter.CustomerFullName.ToLower().Trim())) &&
                    (filter.Telephone == null || customer.Telephone.Trim().Contains(filter.Telephone.Trim())) &&
                    (filter.EmailAddress == null || customer.EmailAddress.ToLower().Trim().Contains(filter.EmailAddress.ToLower().Trim())) &&
                    (filter.CustomerBirthday == null || customer.CustomerBirthday.Equals(filter.CustomerBirthday)));
                    
            }
            return GetAll();
        }
    }
}
