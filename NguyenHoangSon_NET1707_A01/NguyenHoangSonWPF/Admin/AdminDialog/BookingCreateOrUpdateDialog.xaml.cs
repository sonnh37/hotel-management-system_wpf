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
        private readonly IRoomRepository _roomRepository;
        private readonly RoomManagementPage _roomManagementPage;
        private RoomInformation? room;
        public BookingCreateOrUpdateDialog(IRoomRepository roomRepository, RoomManagementPage roomManagementPage, RoomInformation roomInformation)
        {
            InitializeComponent();
            _roomRepository = roomRepository;
            _roomManagementPage = roomManagementPage;
            room = roomInformation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (room != null)
            {
                RoomView view = _roomManagementPage.ConvertModelToView(room);
                LoadRoomInDialog(view);

                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                labelStatus.Visibility = Visibility.Visible;
                cboStatus.Visibility = Visibility.Visible;

                btnCreateOrUpdate.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (room != null)
            {
                // Update
                RoomInformation roomGetById = _roomRepository.GetById(Convert.ToInt32(room.RoomId));
                _roomRepository.Update(UpdateRoomFromInput(roomGetById));
            }
            else
            {
                _roomRepository.Add(UpdateRoomFromInput(new RoomInformation()));
            }
            this.Close();
            _roomManagementPage.RefreshListView();
        }

        private RoomInformation UpdateRoomFromInput(RoomInformation c)
        {
            c.RoomNumber = txtBoxRoomNumber.Text;
            c.RoomDetailDescription = txtBoxRoomDetailDescription.Text;
            c.RoomMaxCapacity = Convert.ToInt32(txtBoxRoomMaxCapacity.Text);

            if(c.RoomTypeId != Convert.ToInt32(txtBoxRoomTypeId.Text))
            {
                c.RoomTypeId = Convert.ToInt32(txtBoxRoomTypeId.Text);
            }
            
            c.RoomStatus = ShareService.GetStatus(cboStatus.Text);
            c.RoomPricePerDay = Convert.ToDecimal(txtBoxRoomPricePerDay.Text);

            return c;
        }

        private void LoadRoomInDialog(RoomView view)
        {
            txtBoxRoomNumber.Text = view.RoomNumber;
            txtBoxRoomDetailDescription.Text = view.RoomDetailDescription;
            txtBoxRoomMaxCapacity.Text = view.RoomMaxCapacity.ToString();
            txtBoxRoomTypeId.Text = view.RoomTypeId.ToString();
            cboStatus.Text = view.RoomStatus;
            txtBoxRoomPricePerDay.Text = view.RoomPricePerDay.ToString();
            txtBoxId.Text = view.RoomId.ToString();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckDecimalFromInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^(\d+(\.\d{0,2})?)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
