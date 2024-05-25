using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
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

namespace NguyenHoangSonWPF.Admin.AdminDialog
{
    /// <summary>
    /// Interaction logic for BookingCreateOrUpdateDialog.xaml
    /// </summary>
    public partial class BookingCreateOrUpdateDialog : Window
    {
        public BookingCreateOrUpdateDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
