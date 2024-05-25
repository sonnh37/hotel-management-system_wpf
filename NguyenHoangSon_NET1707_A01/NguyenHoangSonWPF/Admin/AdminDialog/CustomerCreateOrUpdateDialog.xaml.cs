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
                CustomerView view = _customerManagementPage.ConvertModelToView(customer);
                LoadCustomerInDialog(view);

                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                labelStatus.Visibility = Visibility.Visible;
                cboStatus.Visibility = Visibility.Visible;

                btnCreateOrUpdadte.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (customer != null)
            {
                // Update
                Customer customerGetById = _customerRepository.GetById(Convert.ToInt32(customer.CustomerId));
                _customerRepository.Update(UpdateCustomerFromInput(customerGetById));
            }
            else
            {
                _customerRepository.Add(UpdateCustomerFromInput(new Customer()));
            }
            this.Close();
            _customerManagementPage.RefreshListView();
        }

        private Customer UpdateCustomerFromInput(Customer c)
        {
            c.CustomerFullName = txtBoxFullName.Text;
            c.Telephone = txtBoxTelephone.Text;
            c.EmailAddress = txtBoxEmailAddress.Text;
            c.CustomerBirthday = txtBoxBirthday.SelectedDate.Value;
            c.CustomerStatus = ShareService.GetStatus(cboStatus.Text);
            c.Password = txtBoxPassword.Password;
            return c;
        }

        private void LoadCustomerInDialog(CustomerView view)
        {
            txtBoxFullName.Text = view.CustomerFullName;
            txtBoxTelephone.Text = view.Telephone;
            txtBoxEmailAddress.Text = view.EmailAddress;
            txtBoxBirthday.SelectedDate = view.CustomerBirthday;
            cboStatus.Text = view.CustomerStatus;
            txtBoxPassword.Password = view.Password;
            txtBoxId.Text = view.CustomerId.ToString();
        }
    }
}
