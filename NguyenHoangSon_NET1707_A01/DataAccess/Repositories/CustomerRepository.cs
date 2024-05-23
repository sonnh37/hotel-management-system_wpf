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

        public IEnumerable<Customer> List()
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
    }
}
