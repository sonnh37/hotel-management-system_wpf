using DataAccess.IRepositories;
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

namespace NguyenHoangSonWPF.Admin
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly MainWindow mainWindow;
        public AdminPage(MainWindow _mainWindow,
            ICustomerRepository _customerRepository,
            IBookingRepository _bookingRepository,
            IRoomRepository _roomRepository)
        {
            InitializeComponent();
            this.mainWindow = _mainWindow;
            this.customerRepository = _customerRepository;
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
        }

        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            Close();
            mainWindow.Show();
        }

        private void Goto_RoomInformation(object sender, MouseButtonEventArgs e)
        {
            logo.Visibility = Visibility.Hidden;
            RoomManagementPage roomManagementPage = new RoomManagementPage(roomRepository);
            frameMain.Content = roomManagementPage;
        }

        private void Goto_BookingReservation(object sender, MouseButtonEventArgs e)
        {
            logo.Visibility = Visibility.Hidden;
            BookingManagementPage bookingManagement = new BookingManagementPage(bookingRepository);
            frameMain.Content = bookingRepository;
        }

        private void Goto_Customer(object sender, MouseButtonEventArgs e)
        {
            logo.Visibility = Visibility.Hidden;
            CustomerManagementPage customerManagementPage = new CustomerManagementPage(customerRepository);
            frameMain.Content = customerManagementPage;
        }
    }
}
