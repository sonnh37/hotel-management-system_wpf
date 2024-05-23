using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NguyenHoangSonWPF.Admin.AdminDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF.Admin
{
    /// <summary>
    /// Interaction logic for CustomerManagementPage.xaml
    /// </summary>
    public partial class CustomerManagementPage : Page
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerManagementPage(ICustomerRepository _customerRepository)
        {
            InitializeComponent();
            this.customerRepository = _customerRepository;
            this.listView.SelectionChanged += ListView_SelectionChanged;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = GetListView();
        }

        public void RefreshListView()
        {
            listView.ItemsSource = GetListView();
        }

        private IEnumerable<CustomerView> GetListView()
        {
            List<CustomerView> views = new List<CustomerView>();
            foreach (var item in customerRepository.List())
            {
                if (item.CustomerStatus != Convert.ToByte(2))
                {
                    views.Add(ConvertModelToView(item));
                }
            }

            return views;
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = GetListView();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove customer seledted?", "Remove customer", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<CustomerView> views = listView.SelectedItems.Cast<CustomerView>().ToList();
                views.ForEach(view => view.CustomerStatus = "Deleted");
                views.ForEach(view => customerRepository.Update(ConvertViewToModel(view)));

                RefreshListView();
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                List<CustomerView> customerViews = listView.SelectedItems.Cast<CustomerView>().ToList();
                customerViews.ForEach(customerView =>
                {
                    CustomerCreateOrUpdateDialog dialog = new CustomerCreateOrUpdateDialog(customerRepository, this, ConvertViewToModel(customerView));

                    dialog.Show();
                });
            }
            else
            {
                MessageBox.Show("Please select customer");
            }
        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            CustomerCreateOrUpdateDialog dialog = new CustomerCreateOrUpdateDialog(customerRepository, this, null);

            dialog.Show();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        #region Mapping View, Model 
        private CustomerView ConvertModelToView(Customer model)
        {
            return new CustomerView()
            {
                CustomerId = model.CustomerId,
                CustomerFullName = model.CustomerFullName,
                Telephone = model.Telephone,
                EmailAddress = model.EmailAddress,
                CustomerBirthday = model.CustomerBirthday,
                CustomerStatus = ServiceProcess.SetStatus((byte)model.CustomerStatus),
                Password = model.Password,
            };
        }

        private Customer ConvertViewToModel(CustomerView view)
        {
            Customer c = customerRepository.GetById(Convert.ToInt32(view.CustomerId));
            c.CustomerFullName = view.CustomerFullName;
            c.Telephone = view.Telephone;
            c.EmailAddress = view.EmailAddress;
            c.CustomerBirthday = view.CustomerBirthday;
            c.CustomerStatus = ServiceProcess.GetStatus(view.CustomerStatus);
            c.Password = view.Password;

            return c;
        }

        #endregion
    }
}
