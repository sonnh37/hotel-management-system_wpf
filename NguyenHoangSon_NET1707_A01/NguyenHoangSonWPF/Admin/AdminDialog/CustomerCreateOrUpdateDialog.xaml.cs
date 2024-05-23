using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF.Admin.AdminDialog
{
    /// <summary>
    /// Interaction logic for CustomerCreateOrUpdateDialog.xaml
    /// </summary>
    public partial class CustomerCreateOrUpdateDialog : Window
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManagementPage _customerManagementPage;
        private Customer? customer;

        public CustomerCreateOrUpdateDialog(ICustomerRepository customerRepository, CustomerManagementPage customerManagementPage, Customer? customer)
        {
            InitializeComponent();
            _customerRepository = customerRepository;
            _customerManagementPage = customerManagementPage;
            this.customer = customer;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (customer != null)
            {
                txtBoxFullName.Text = customer.CustomerFullName;
                txtBoxTelephone.Text = customer.Telephone;
                txtBoxEmailAddress.Text = customer.EmailAddress;
                txtBoxBirthday.SelectedDate = customer.CustomerBirthday;
                cboStatus.Text = ServiceProcess.SetStatus(customer.CustomerStatus == 1 ? Convert.ToByte(1) : Convert.ToByte(2));
                txtBoxPassword.Password = customer.Password;
                txtBoxId.Text = customer.CustomerId.ToString();
                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                labelStatus.Visibility = Visibility.Visible;
                cboStatus.Visibility = Visibility.Visible;

                btn.Content = "Update";
                this.Height = 550;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (customer != null)
            {
                // Update
                Customer customerGetById = _customerRepository.GetById(Convert.ToInt32(customer.CustomerId));
                _customerRepository.Update(GetCustomer(customerGetById));
            }
            else
            {
                _customerRepository.Add(GetCustomer(new Customer()));
            }
            this.Close();
            _customerManagementPage.RefreshListView();
        }

        

        private Customer GetCustomer(Customer c)
        {
            c.CustomerFullName = txtBoxFullName.Text;
            c.Telephone = txtBoxTelephone.Text;
            c.EmailAddress = txtBoxEmailAddress.Text;
            c.CustomerBirthday = txtBoxBirthday.SelectedDate.Value;
            c.CustomerStatus = ServiceProcess.GetStatus(cboStatus.Text);
            c.Password = txtBoxPassword.Password;
            return c;
        }
    }
}
