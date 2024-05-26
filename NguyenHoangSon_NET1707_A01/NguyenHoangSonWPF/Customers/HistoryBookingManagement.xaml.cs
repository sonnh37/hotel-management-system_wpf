using BusinessObject.Models;
using BusinessObject.Shared;
using BusinessObject.Views;
using DataAccess.IRepositories;
using NguyenHoangSonWPF.Admin.AdminDialog;
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
    /// Interaction logic for HistoryBookingManagement.xaml
    /// </summary>
    public partial class HistoryBookingManagement : Window
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly CustomerManagementPage customerManagementPage;
        private readonly Customer customer;
        public HistoryBookingManagement(IBookingRepository _bookingRepository, IBookingDetailRepository _bookingDetailRepository, CustomerManagementPage customerManagementPage, Customer _customer)
        {
            InitializeComponent();
            this.bookingRepository = _bookingRepository;
            bookingDetailRepository = _bookingDetailRepository;
            this.customerManagementPage = customerManagementPage;
            this.listView.SelectionChanged += ListView_SelectionChanged;
            this.customer = _customer;
        }

        private void Button_BookingDetail(object sender, RoutedEventArgs e)
        {
            BookingView view = listView.SelectedItem as BookingView;
            BookingReservation booking = bookingRepository.GetById((int)view.BookingReservationId);
            HistoryBookingDetailManagement bookingDetailManagement = new HistoryBookingDetailManagement(bookingRepository, bookingDetailRepository, this, booking);
            bookingDetailManagement.Show();
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
            SetButtonEnabledForustomer(false);
        }

        private void SetButtonEnabledForustomer(bool enabled)
        {
            SetButtonEnabled(enabled);
            btnDelete.Visibility = Visibility.Hidden;
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

        private IEnumerable<BookingView> GetListView()
        {
            List<BookingView> views = new List<BookingView>();
            foreach (var item in customer.BookingReservations)
            {
                if (item.BookingStatus != Convert.ToByte(2))
                {
                    views.Add(ConvertModelToView(item));
                }
            }

            return views;
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            BookingView bookingViewFilter = GetBookingViewFilter();
            IEnumerable<BookingReservation> models = bookingRepository.GetAllByFilter(bookingViewFilter);
            List<BookingView> views = new List<BookingView>();

            foreach (var model in models)
            {
                views.Add(ConvertModelToView((BookingReservation)model));
            }

            listView.ItemsSource = views;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove customer seledted?", "Remove customer", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<BookingView> views = listView.SelectedItems.Cast<BookingView>().ToList();
                views.ForEach(view => bookingRepository.Update(UpdateBookingStatusDeleted((int)view.CustomerId)));

                RefreshListView();
            }
        }


        #endregion



        #region Mapping View, Model + Get ViewFilter 
        public BookingView ConvertModelToView(BookingReservation model)
        {
            CustomerView customerView = customerManagementPage.ConvertModelToView(model.Customer);

            List<BookingDetailView> bookingDetails = new List<BookingDetailView>();
            foreach (BookingDetail detail in model.BookingDetails)
            {
                bookingDetails.Add(ConvertModelToViewByBookingDetail(detail));
            }

            return new BookingView()
            {
                BookingReservationId = model.BookingReservationId,
                BookingDate = model.BookingDate,
                TotalPrice = model.TotalPrice,
                CustomerId = model.CustomerId,
                BookingStatus = ShareService.SetStatus((byte)model.BookingStatus),

                Customer = customerView,
                BookingDetails = bookingDetails,
            };
        }

        public BookingDetailView ConvertModelToViewByBookingDetail(BookingDetail model)
        {
            BookingDetailView roomTypeView = new BookingDetailView
            {
                BookingReservationId = model.BookingReservationId,
                RoomId = model.RoomId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ActualPrice = model.ActualPrice,
            };
            return roomTypeView;
        }

        public BookingReservation UpdateBookingStatusDeleted(int roomId)
        {
            BookingReservation room = bookingRepository.GetById(Convert.ToInt32(roomId));
            room.BookingStatus = Convert.ToByte(2);

            return room;
        }

        private BookingView GetBookingViewFilter()
        {
            return new BookingView()
            {
                BookingReservationId = !String.IsNullOrEmpty(searchByBookingReservationId.Text) ? int.Parse(searchByBookingReservationId.Text) : null,
                CustomerId = !String.IsNullOrEmpty(searchByCustomerId.Text) ? int.Parse(searchByCustomerId.Text) : null,
                BookingDate = searchByBookingDate.SelectedDate.HasValue ? searchByBookingDate.SelectedDate.Value : null,
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
            btnBookingDetail.IsEnabled = enabled;
        }

        private void ClearFieldsExisting()
        {
            searchByCustomerId.Clear();
            searchByBookingReservationId.Clear();
            searchByBookingDate.SelectedDate = null;
        }
        #endregion
    }
}
