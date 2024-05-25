using DataAccess.IRepositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NguyenHoangSonWPF.Admin
{
    /// <summary>
    /// Interaction logic for RoomManagementPage.xaml
    /// </summary>
    public partial class RoomManagementPage : Page
    {
        private readonly IRoomRepository roomRepository;

        public RoomManagementPage(IRoomRepository _roomRepository)
        {
            InitializeComponent();
            this.roomRepository = _roomRepository;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
