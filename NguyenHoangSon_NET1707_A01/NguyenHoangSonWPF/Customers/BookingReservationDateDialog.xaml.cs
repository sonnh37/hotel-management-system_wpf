using BusinessObject.Models;
using DataAccess.IRepositories;
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

namespace NguyenHoangSonWPF.Customers
{
    /// <summary>
    /// Interaction logic for BookingReservationDateDialog.xaml
    /// </summary>
    public partial class BookingReservationDateDialog : Window
    {
        public RoomInformation room;
        public Customer customer;
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;

        public BookingReservationDateDialog(int roomId, Customer customer, IBookingRepository _bookingRepository, IRoomRepository _roomRepository, IBookingDetailRepository _bookingDetailRepository)
        {
            InitializeComponent();
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
            this.bookingDetailRepository = _bookingDetailRepository;
            this.room = _roomRepository.GetById(roomId);
            this.customer = customer;
        }

        private void Button_Checkout(object sender, RoutedEventArgs e)
        {
            // get StartDate && EndDate
            if (!txtStartDate.SelectedDate.HasValue && !@txtEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Pls select Start and End Date when you choose a room in here");
                return;
            }
            DateTime startDate = txtStartDate.SelectedDate.Value;
            DateTime endDate = txtEndDate.SelectedDate.Value;
            // implements check customer ordered this room available bookingId
            // if avaiable bookingId check roomId, else create new bookingId
            BookingReservation bookingReservation = null;
            IEnumerable<BookingReservation> bookingReservations = bookingRepository.FindAllByCustomerId(customer.CustomerId);
            foreach (BookingReservation _booking in bookingReservations)
            {
                if (DateTime.Compare(DateTime.Now.Date, (DateTime)_booking.BookingDate) == 0)
                {
                    bookingReservation = _booking; break;
                }
            }
            
            // it means bookingId has value in BookingReservation ( not need create BookingReservation)
            if (bookingReservation != null)
            {
                CreateWithHasBookingReservationToDb(bookingReservation);
            }
            else
            {
                CreateNewBookingReservationToDb();
            }
        }

        private void CreateWithHasBookingReservationToDb(BookingReservation bookingReservation)
        {
            // if available roomId in BookingRerservation create new bookingId, find bookingId to add with roomId
            try
            {
                BookingDetail _bookingDetail = null;
                foreach(BookingDetail x in bookingReservation.BookingDetails)
                {
                    if(x.RoomId == room.RoomId)
                    {
                        _bookingDetail = x; break;
                    }
                }

                if(_bookingDetail != null)
                {
                    MessageBox.Show($"Room {room.RoomNumber} was ordered in today", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // new room
                BookingDetail bookingDetail = GetBookingDetail(bookingReservation.BookingReservationId, txtStartDate.SelectedDate.Value, txtEndDate.SelectedDate.Value);
                bookingDetailRepository.Add(bookingDetail);

                // update totalPrice
                bookingReservation.TotalPrice += room.RoomPricePerDay;
                bookingRepository.Update(bookingReservation);

                MessageBox.Show("Booking successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateNewBookingReservationToDb()
        {
            // implements create new BookingReservation(newbrId,newDate,price = 0,cusId,status 1) by bookingRepository
            BookingReservation bookingReservation = GetBookingReservation();

            // implements create new BookingDetail(brId,RoomId,StartDate,EndDate,PriceRoom) by bookingDetailRepository
            BookingDetail bookingDetail = GetBookingDetail(bookingRepository.GetAll().Count(), txtStartDate.SelectedDate.Value, txtEndDate.SelectedDate.Value);

            try
            {
                if (bookingDetail.BookingReservationId != null && bookingDetail.RoomId != null)
                {
                    bookingRepository.Add(bookingReservation);
                    bookingDetailRepository.Add(bookingDetail);

                    MessageBox.Show("Booking successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("BookingReservationId or RoomId is null. Please check the details and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtRoomNumber.Text = room.RoomNumber;
        }

        private BookingReservation GetBookingReservation()
        {
            return new BookingReservation
            {
                BookingReservationId = bookingRepository.GetAll().Count(),
                BookingDate = DateTime.UtcNow,
                TotalPrice = room.RoomPricePerDay,
                CustomerId = customer.CustomerId,
                BookingStatus = Convert.ToByte(1),
            };
        }

        private BookingDetail GetBookingDetail(int bookingReservationId, DateTime startDate, DateTime endDate)
        {
            return new BookingDetail
            {
                BookingReservationId = bookingReservationId,
                RoomId = room.RoomId,
                StartDate = startDate,
                EndDate = endDate,
                ActualPrice = room.RoomPricePerDay,
            };
        }
    }
}
