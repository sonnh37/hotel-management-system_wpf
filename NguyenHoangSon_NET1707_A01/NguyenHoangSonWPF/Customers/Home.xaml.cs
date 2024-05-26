using BusinessObject.Models;
using DataAccess.IRepositories;
using NguyenHoangSonWPF.Admin;
using NguyenHoangSonWPF.Admin.AdminDialog;
using NguyenHoangSonWPF.Customers;
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
            ListProduct.ItemsSource = roomRepository.GetAll();
            Session.carts = new List<BookingDetail>();
        }

        private void Home_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Show();
        }

        private void Button_OpenMyOrder(object sender, RoutedEventArgs e)
        {
            BookingDetailManagement bookingDetailManagement = new BookingDetailManagement(
                bookingRepository, bookingDetailRepository, 
                new BookingManagementPage(bookingRepository, bookingDetailRepository, 
                    new CustomerManagementPage(customerRepository)), 
                new BookingReservation { Customer = customer});
        }

        private void Button_Order(object sender, RoutedEventArgs e)
        {
            // open 
            
            if (sender is Button button)
            {
                BookingReservationDateDialog booking = new BookingReservationDateDialog((int)button.Tag, customer, bookingRepository, roomRepository, bookingDetailRepository);
                booking.Show();
                
            }
        }
    }
}
