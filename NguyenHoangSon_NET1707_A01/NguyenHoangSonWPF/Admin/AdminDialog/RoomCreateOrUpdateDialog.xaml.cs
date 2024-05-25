using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// Interaction logic for RoomCreateOrUpdateDialog.xaml
    /// </summary>
    public partial class RoomCreateOrUpdateDialog : Window
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly RoomManagementPage _roomManagementPage;

        private RoomInformation? room;
        private RoomView roomView;
        public List<RoomTypeView> roomTypeViews;

        public RoomCreateOrUpdateDialog(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, RoomManagementPage roomManagementPage, RoomInformation roomInformation)
        {
            InitializeComponent();
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _roomManagementPage = roomManagementPage;
            room = roomInformation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (room != null)
            {
                roomView = _roomManagementPage.ConvertModelToView(room);
                
                LoadRoomInDialog(roomView);

                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                labelStatus.Visibility = Visibility.Visible;
                cboStatus.Visibility = Visibility.Visible;

                btnCreateOrUpdate.Content = "Update";
            }
            LoadRoomTypeInDialog();
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
            c.RoomTypeId = Convert.ToInt32(cboBoxRoomTypeId.SelectedValue);
            c.RoomStatus = ShareService.GetStatus(cboStatus.Text);
            c.RoomPricePerDay = Convert.ToDecimal(txtBoxRoomPricePerDay.Text);

            return c;
        }

        private void LoadRoomTypeInDialog()
        {
            // Convert RoomType To RoomTypeView
            IEnumerable<RoomType> roomTypes = _roomTypeRepository.GetAll();
            roomTypeViews = new List<RoomTypeView>();
            foreach (var roomType in roomTypes)
            {
                roomTypeViews.Add(_roomManagementPage.ConvertModelToViewByRoomType(roomType));
            }

            cboBoxRoomTypeId.ItemsSource = roomTypeViews;
            cboBoxRoomTypeId.DisplayMemberPath = "RoomTypeName";
            cboBoxRoomTypeId.SelectedValuePath = "RoomTypeId";

            // Set select default of room type from room to UPDATE
            if(roomView != null)
            {
                foreach (var roomTypeView in roomTypeViews)
                {
                    if (roomTypeView.RoomTypeId == roomView.RoomType.RoomTypeId)
                    {
                        cboBoxRoomTypeId.SelectedValue = roomTypeView.RoomTypeId;
                    }
                }
            }
            
        }

        private void LoadRoomInDialog(RoomView view)
        {
            txtBoxRoomNumber.Text = view.RoomNumber;
            txtBoxRoomDetailDescription.Text = view.RoomDetailDescription;
            txtBoxRoomMaxCapacity.Text = view.RoomMaxCapacity.ToString();
            cboBoxRoomTypeId.Text = view.RoomTypeId.ToString();
            cboStatus.Text = view.RoomStatus;
            txtBoxRoomPricePerDay.Text = view.RoomPricePerDay.ToString();
            txtBoxId.Text = view.RoomId.ToString();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _roomManagementPage.PreviewTextInput(sender, e);
        }

        private void CheckDecimalFromInput(object sender, TextCompositionEventArgs e)
        {
            _roomManagementPage.CheckDecimalFromInput(sender, e);
        }
    }
}
