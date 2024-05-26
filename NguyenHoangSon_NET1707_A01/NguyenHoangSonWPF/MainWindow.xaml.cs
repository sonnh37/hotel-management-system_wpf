using DataAccess.IRepositories;
using Microsoft.Extensions.Configuration;
using NguyenHoangSonWPF.Admin;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRoomTypeRepository roomTypeRepository;
        
        public MainWindow(ICustomerRepository _customerRepository, 
            IBookingRepository _bookingRepository, 
            IRoomRepository _roomRepository,
            IRoomTypeRepository _roomTypeRepository,
            IBookingDetailRepository _bookingDetailRepository
            )
        {
            InitializeComponent();
            this.bookingDetailRepository = _bookingDetailRepository;
            this.customerRepository = _customerRepository;
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
            this.roomTypeRepository = _roomTypeRepository;
            txtBoxUsername.Text = "admin";
            pwdBoxPassword.Password = "admin";
        }

        public void resetFormLogin()
        {
            txtBoxUsername.Text = null;
            pwdBoxPassword.Password = null;
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            string username = txtBoxUsername.Text.ToString();
            string password = pwdBoxPassword.Password.ToString();
            var account = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("account");
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                if (username.Equals(account["username"]) && password.Equals(account["password"]))
                {
                    Session.Username = username;
                    this.Hide();
                    AdminPage adminPage = new AdminPage(this, customerRepository, bookingRepository, bookingDetailRepository, roomRepository, roomTypeRepository);
                    adminPage.Show();
                    resetFormLogin();
                }
                else //if (memberRepository.FindByEmailAndPassword(username, password) != null)
                {
                    Session.Username = username;
                    this.Hide();
                    Home home = new Home(this, customerRepository, bookingRepository, roomRepository);
                    home.Show();
                    resetFormLogin();
                }
                //else
                //{
                //    MessageBox.Show("Please enter username and password");
                //}
            }
            else
            {
                MessageBox.Show("Please enter username and password");
            }
        }
    }
}