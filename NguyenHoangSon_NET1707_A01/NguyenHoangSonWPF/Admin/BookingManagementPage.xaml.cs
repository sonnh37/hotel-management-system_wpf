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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF.Admin
{
    /// <summary>
    /// Interaction logic for BookingManagementPage.xaml
    /// </summary>
    public partial class BookingManagementPage : Page
    {
        private readonly IBookingRepository bookingRepository;
        public BookingManagementPage(IBookingRepository _bookingRepository)
        {
            InitializeComponent();
            this.bookingRepository = _bookingRepository;
        }
    }
}
