using BusinessObject.Models;
using BusinessObject.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Customer GetById(int id);
        IEnumerable<Customer> GetAllByFilter(CustomerView customerViewFilter); 
    }
}
