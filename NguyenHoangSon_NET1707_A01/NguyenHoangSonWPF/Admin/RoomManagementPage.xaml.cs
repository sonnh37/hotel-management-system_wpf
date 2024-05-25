using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using NguyenHoangSonWPF.Admin.AdminDialog;
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
        private readonly IRoomTypeRepository roomTypeRepository;

        public RoomManagementPage(IRoomRepository _roomRepository, IRoomTypeRepository _roomTypeRepository)
        {
            InitializeComponent();
            this.roomRepository = _roomRepository;
            this.listView.SelectionChanged += ListView_SelectionChanged;
            roomTypeRepository = _roomTypeRepository;
        }


        #region Main
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetButtonEnabled(listView.SelectedItems.Count > 0);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = GetListView();
            ClearFieldsExisting();
        }

        public void RefreshListView()
        {
            listView.ItemsSource = null;
            listView.ItemsSource = GetListView();
            ClearFieldsExisting();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = GetListView();
            ClearFieldsExisting();
        }

        private IEnumerable<RoomView> GetListView()
        {
            List<RoomView> views = new List<RoomView>();
            foreach (var item in roomRepository.GetAll())
            {
                if (item.RoomStatus != Convert.ToByte(2))
                {
                    views.Add(ConvertModelToView(item));
                }
            }

            return views;
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            RoomView customerViewFilter = GetRoomViewFilter();
            IEnumerable<RoomInformation> models = roomRepository.GetAllByFilter(customerViewFilter);
            List<RoomView> views = new List<RoomView>();

            foreach (var model in models)
            {
                views.Add(ConvertModelToView((RoomInformation)model));
            }

            listView.ItemsSource = views;
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                List<RoomView> customerViews = listView.SelectedItems.Cast<RoomView>().ToList();
                customerViews.ForEach(customerView =>
                {
                    RoomCreateOrUpdateDialog dialog = new RoomCreateOrUpdateDialog(roomRepository, roomTypeRepository, this, roomRepository.GetById(Convert.ToInt32(customerView.RoomId)));

                    dialog.Show();
                });
            }
            else
            {
                MessageBox.Show("Please select customer");
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove customer seledted?", "Remove customer", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<RoomView> views = listView.SelectedItems.Cast<RoomView>().ToList();
                views.ForEach(view => roomRepository.Update(UpdateRoomStatusDeleted((int)view.RoomId)));

                RefreshListView();
            }
        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            RoomCreateOrUpdateDialog dialog = new RoomCreateOrUpdateDialog(roomRepository, roomTypeRepository, this, null);

            dialog.Show();
        }
        #endregion

        #region Mapping View, Model + Get ViewFilter 
        public RoomView ConvertModelToView(RoomInformation model)
        {
            RoomTypeView roomTypeView = ConvertModelToViewByRoomType(model.RoomType);
            return new RoomView()
            {
                RoomId = model.RoomId,
                RoomNumber = model.RoomNumber,
                RoomDetailDescription = model.RoomDetailDescription,
                RoomMaxCapacity = model.RoomMaxCapacity,
                RoomTypeId = model.RoomTypeId,
                RoomStatus = ShareService.SetStatus((byte)model.RoomStatus),
                RoomPricePerDay = model.RoomPricePerDay,
                // Assuming RoomTypeView can be converted similarly
                RoomType = roomTypeView,
                // BookingDetails not included as it's commented out in RoomView
            };
        }

        public RoomTypeView ConvertModelToViewByRoomType(RoomType model)
        {
            RoomTypeView roomTypeView = new RoomTypeView
            {
                RoomTypeId = model.RoomTypeId,
                RoomTypeName = model.RoomTypeName,
                TypeDescription = model.TypeDescription,
                TypeNote = model.TypeNote,
            };
            return roomTypeView;
        }

        public RoomInformation UpdateRoomStatusDeleted(int roomId)
        {
            RoomInformation room = roomRepository.GetById(Convert.ToInt32(roomId));
            room.RoomStatus = Convert.ToByte(2);

            return room;
        }

        private RoomView GetRoomViewFilter()
        {
            return new RoomView()
            {
                RoomId = !String.IsNullOrEmpty(searchById.Text) ? int.Parse(searchById.Text) : null,
                RoomNumber = !String.IsNullOrEmpty(searchByRoomNumber.Text) ? searchByRoomNumber.Text : null,
                RoomDetailDescription = !String.IsNullOrEmpty(searchByRoomDetailDescription.Text) ? searchByRoomDetailDescription.Text : null,
                RoomMaxCapacity = !String.IsNullOrEmpty(searchByRoomMaxCapacity.Text) ? Convert.ToInt32(searchByRoomMaxCapacity.Text) : null,
                RoomTypeId = !String.IsNullOrEmpty(searchByRoomTypeId.Text) ? Convert.ToInt32(searchByRoomTypeId.Text) : null,
                RoomPricePerDay = !String.IsNullOrEmpty(searchByRoomPricePerDay.Text) ? Convert.ToInt32(searchByRoomPricePerDay.Text) : null,
            };
        }

        #endregion

        #region other func
        public void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void CheckDecimalFromInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void SetButtonEnabled(bool enabled)
        {
            btnEdit.IsEnabled = enabled;
            btnDelete.IsEnabled = enabled;
        }

        private void ClearFieldsExisting()
        {
            searchById.Clear();
            searchByRoomNumber.Clear();
            searchByRoomDetailDescription.Clear();
            searchByRoomMaxCapacity.Clear();
            searchByRoomTypeId.Clear();
            searchByRoomPricePerDay.Clear();
        }
        #endregion
    }
}
