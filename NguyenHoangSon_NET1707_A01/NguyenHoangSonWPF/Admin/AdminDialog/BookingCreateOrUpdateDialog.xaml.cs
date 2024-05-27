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
        private IBookingRepository _bookingRepository;
        private BookingManagementPage _bookingManagementPage;

        private BookingReservation? booking;
        private BookingView bookingView;
        public List<BookingDetailView> bookingDetailViews;

        public BookingCreateOrUpdateDialog(IBookingRepository bookingRepository, BookingManagementPage bookingManagementPage, BookingReservation bookingReservation)
        {
            InitializeComponent();

            this._bookingManagementPage = bookingManagementPage;
            this._bookingRepository = bookingRepository;
            this.booking = bookingReservation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (booking != null)
            {
                bookingView = _bookingManagementPage.ConvertModelToView(booking);

                LoadBookingInDialog(bookingView);

                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                labelStatus.Visibility = Visibility.Visible;
                cboStatus.Visibility = Visibility.Visible;

                btnCreateOrUpdate.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (booking != null)
            {
                // Update
                BookingReservation bookingReservation = _bookingRepository.GetById(Convert.ToInt32(booking.BookingReservationId));
                _bookingRepository.Update(UpdateBookingFromInput(bookingReservation));
            }
            else
            {
                _bookingRepository.Add(UpdateBookingFromInput(new BookingReservation()));
            }
            this.Close();
            _bookingManagementPage.RefreshListView();
        }

        private BookingReservation UpdateBookingFromInput(BookingReservation c)
        {
            c.BookingDate = txtBookingDate.SelectedDate.Value;
            c.TotalPrice = Convert.ToInt32(txtTotalPrice.Text);
            c.CustomerId = Convert.ToInt32(txtCustomerId.Text);
            c.BookingStatus = ShareService.GetStatus(cboStatus.Text);

            return c;
        }

        private void LoadBookingInDialog(BookingView view)
        {
            txtBoxId.Text = view.BookingReservationId.ToString();
            txtBookingDate.Text = view.BookingDate.ToString();
            txtTotalPrice.Text = view.TotalPrice.ToString();
            txtCustomerId.Text = view.CustomerId.ToString();
            cboStatus.Text = view.BookingStatus;
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _bookingManagementPage.PreviewTextInput(sender, e);
        }

        private void CheckDecimalFromInput(object sender, TextCompositionEventArgs e)
        {
            _bookingManagementPage.CheckDecimalFromInput(sender, e);
        }
    }
}
