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
    public partial class BookingDetailManagement : Window
    {
        private IBookingRepository _bookingRepository;
        private IBookingDetailRepository _bookingDetailRepository;
        private BookingManagementPage _bookingManagementPage;
        public List<BookingDetailView> bookingDetailViews;
        private BookingReservation? booking;

        public BookingDetailManagement(IBookingRepository bookingRepository, IBookingDetailRepository bookingDetailRepository, BookingManagementPage bookingManagementPage, BookingReservation? booking)
        {
            InitializeComponent();
            _bookingRepository = bookingRepository;
            _bookingDetailRepository = bookingDetailRepository;
            _bookingManagementPage = bookingManagementPage;
            this.booking = booking;
            this.listView.SelectionChanged += ListView_SelectionChanged;
        }

        #region Main
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetButtonEnabled(listView.SelectedItems.Count > 0);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = GetListView();
            ClearFieldsExisting();
        }

        public void RefreshListView()
        {
            listView.ItemsSource = GetListView();
            ClearFieldsExisting();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = GetListView();
            // clear
            ClearFieldsExisting();
        }

        private IEnumerable<BookingDetailView> GetListView()
        {
            List<BookingDetail> bookingDetails = booking.BookingDetails.ToList();
            bookingDetailViews = new List<BookingDetailView>();
            foreach (BookingDetail view in bookingDetails)
            {
                bookingDetailViews.Add(_bookingManagementPage.ConvertModelToViewByBookingDetail(view));
            }

            return bookingDetailViews;
        }


        private void Button_Search(object sender, RoutedEventArgs e)
        {
            BookingDetailView bookingViewFilter = GetBookingViewFilter();
            IEnumerable<BookingDetail> models = _bookingDetailRepository.GetAllByFilter(bookingViewFilter);
            List<BookingDetailView> views = new List<BookingDetailView>();

            foreach (var model in models)
            {
                views.Add(_bookingManagementPage.ConvertModelToViewByBookingDetail((BookingDetail)model));
            }

            listView.ItemsSource = views;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove customer seledted?", "Remove customer", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<BookingDetailView> views = listView.SelectedItems.Cast<BookingDetailView>().ToList();
                views.ForEach(view => _bookingDetailRepository.Delete(_bookingDetailRepository.GetByBookingAndRoomId((int)view.BookingReservationId, (int)view.RoomId)));

                RefreshListView();
            }
        }

        #endregion

        #region Mapping View, Model + Get ViewFilter 

        private BookingDetailView GetBookingViewFilter()
        {
            return new BookingDetailView()
            {
                BookingReservationId = !String.IsNullOrEmpty(booking.BookingReservationId.ToString()) ? booking.BookingReservationId : null,
                StartDate = searchByStartDate.SelectedDate.HasValue ? searchByStartDate.SelectedDate.Value : null,
                EndDate = searchByEndDate.SelectedDate.HasValue ? searchByEndDate.SelectedDate.Value : null,
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
            btnDelete.IsEnabled = enabled;
        }

        private void ClearFieldsExisting()
        {
            searchByStartDate.SelectedDate = null;
            searchByEndDate.SelectedDate = null;
        }
        #endregion
    }
}
