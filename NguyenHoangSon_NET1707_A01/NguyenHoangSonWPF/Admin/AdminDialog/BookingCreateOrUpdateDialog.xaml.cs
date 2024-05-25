using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
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

                btnCreateOrUpdadte.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (room != null)
            {
                // Update
                Room roomGetById = _roomRepository.GetById(Convert.ToInt32(room.RoomId));
                _roomRepository.Update(UpdateRoomFromInput(roomGetById));
            }
            else
            {
                _roomRepository.Add(UpdateRoomFromInput(new Room()));
            }
            this.Close();
            _roomManagementPage.RefreshListView();
        }

        private Room UpdateRoomFromInput(Room c)
        {
            c.RoomFullName = txtBoxFullName.Text;
            c.Telephone = txtBoxTelephone.Text;
            c.EmailAddress = txtBoxEmailAddress.Text;
            c.RoomBirthday = txtBoxBirthday.SelectedDate.Value;
            c.RoomStatus = ShareService.GetStatus(cboStatus.Text);
            c.Password = txtBoxPassword.Password;
            return c;
        }

        private void LoadRoomInDialog(RoomView view)
        {
            txtBoxFullName.Text = view.RoomFullName;
            txtBoxTelephone.Text = view.Telephone;
            txtBoxEmailAddress.Text = view.EmailAddress;
            txtBoxBirthday.SelectedDate = view.RoomBirthday;
            cboStatus.Text = view.RoomStatus;
            txtBoxPassword.Password = view.Password;
            txtBoxId.Text = view.RoomId.ToString();
        }
    }
}
