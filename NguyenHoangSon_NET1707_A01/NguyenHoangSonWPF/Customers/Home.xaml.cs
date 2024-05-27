using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using NguyenHoangSonWPF.Admin;
using NguyenHoangSonWPF.Admin.AdminDialog;
using NguyenHoangSonWPF.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly MainWindow mainWindow;
        private Customer customer;

        public Home(MainWindow _mainWindow,
            ICustomerRepository _customerRepository,
            IBookingRepository _bookingRepository,
            IRoomRepository _roomRepository,
            IBookingDetailRepository _bookingDetailRepository,
            Customer _customer
            )
        {
            InitializeComponent();
            this.mainWindow = _mainWindow;
            this.customerRepository = _customerRepository;
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
            this.bookingDetailRepository = _bookingDetailRepository;
            this.customer = _customer;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Page();
        }

        private void Load_Page()
        {
            ListProduct.ItemsSource = roomRepository.GetAll();
            Session.carts = new List<RoomInformation>();
            UpdateCartQuantity();
        }

        private void Home_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Show();
        }

        private void Button_Order(object sender, RoutedEventArgs e)
        {
            if(Session.carts == null)
            {
                Session.carts = new List<RoomInformation>();
            }

            if( sender is Button button)
            {
                RoomInformation room = roomRepository.GetById((int)button.Tag);
                foreach(RoomInformation x in Session.carts)
                {
                    if(x.RoomId == room.RoomId)
                    {
                        MessageBox.Show($"Room {room.RoomNumber} was added. Pls choose another!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                Session.carts.Add(room);
                MessageBox.Show($"Room {room.RoomNumber} added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            UpdateCartQuantity();
        }

        public void UpdateCartQuantity()
        {
            CartCount.Text = Session.carts.Count().ToString();
        }

        private void Button_OpenOrder(object sender, RoutedEventArgs e)
        {
            CartPage cartPage = new CartPage(this,customer, bookingRepository, roomRepository, bookingDetailRepository);
            cartPage.Show();
        }

        private void Button_OpenHistory(object sender, RoutedEventArgs e)
        {
            HistoryBookingManagement bookingManagementPage = new HistoryBookingManagement(bookingRepository, bookingDetailRepository
                , new CustomerManagementPage(customerRepository), customerRepository.GetById(customer.CustomerId));
            bookingManagementPage.Show();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            RoomView customerViewFilter = GetRoomViewFilter();
            ListProduct.ItemsSource = roomRepository.GetAllByFilter(customerViewFilter);
        }
        
        private RoomView GetRoomViewFilter()
        {
            return new RoomView()
            {
                RoomNumber = !String.IsNullOrEmpty(searchByRoomNumber.Text) ? searchByRoomNumber.Text : null,
                RoomMaxCapacity = !String.IsNullOrEmpty(searchByRoomMaxCapacity.Text) ? Convert.ToInt32(searchByRoomMaxCapacity.Text) : null,
                RoomDetailDescription = !String.IsNullOrEmpty(searchByRoomDetailDescription.Text) ? searchByRoomDetailDescription.Text : null,
                RoomTypeId = !String.IsNullOrEmpty(searchByRoomTypeId.Text) ? Convert.ToInt32(searchByRoomTypeId.Text) : null,
                RoomPricePerDay = !String.IsNullOrEmpty(searchByRoomPricePerDay.Text) ? Convert.ToInt32(searchByRoomPricePerDay.Text) : null,
            };
        }

        public void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void CheckDecimalFromInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            Close();
            mainWindow.Show();
        }
    }
}
