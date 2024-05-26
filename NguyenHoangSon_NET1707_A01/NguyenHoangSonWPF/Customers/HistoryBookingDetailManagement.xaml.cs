using BusinessObject.Models;
using BusinessObject.Views;
using DataAccess.IRepositories;
using NguyenHoangSonWPF.Admin;
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

namespace NguyenHoangSonWPF.Customers
{
    /// <summary>
    /// Interaction logic for HistoryBookingDetailManagement.xaml
    /// </summary>
    public partial class HistoryBookingDetailManagement : Window
    {
        private IBookingRepository _bookingRepository;
        private IBookingDetailRepository _bookingDetailRepository;
        private HistoryBookingManagement _bookingManagementPage;
        public List<BookingDetailView> bookingDetailViews;
        private BookingReservation? booking;

        public HistoryBookingDetailManagement(IBookingRepository bookingRepository, IBookingDetailRepository bookingDetailRepository, HistoryBookingManagement bookingManagementPage, BookingReservation? booking)
        {
            InitializeComponent();
            _bookingRepository = bookingRepository;
            _bookingDetailRepository = bookingDetailRepository;
            _bookingManagementPage = bookingManagementPage;
            this.booking = booking;
        }

        #region Main

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

        private void ClearFieldsExisting()
        {
            searchByStartDate.SelectedDate = null;
            searchByEndDate.SelectedDate = null;
        }
        #endregion
    }
}
