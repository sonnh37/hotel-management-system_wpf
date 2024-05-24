using BusinessObject.Models;
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
        private readonly MainWindow mainWindow;
        public Home()
        {
            InitializeComponent();
        }
        public Home(MainWindow _mainWindow,
            ICustomerRepository _customerRepository,
            IBookingRepository _bookingRepository,
            IRoomRepository _roomRepository)
        {
            this.mainWindow = _mainWindow;
            this.customerRepository = _customerRepository;
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
            //ListProduct.ItemsSource = productRepository.GetAll();
            //Session.carts = new List<BookingDetail>();
            //UpdateCartQuantity();
        }

        private void Home_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Show();
        }

        public void UpdateCartQuantity()
        {
            //CartCount.Text = Session.carts.Sum(product => product.Quantity).ToString();
        }
    }
}
