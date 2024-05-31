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
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;

        private readonly Home home;
        public Customer customer;

        public CartWindow(Home _home, Customer customer, IBookingRepository _bookingRepository, IRoomRepository _roomRepository, IBookingDetailRepository _bookingDetailRepository)
        {
            InitializeComponent();
            this.bookingRepository = _bookingRepository;
            this.roomRepository = _roomRepository;
            this.bookingDetailRepository = _bookingDetailRepository;
            this.customer = customer;
            this.home = _home;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.carts == null)
            {
                Session.carts = new List<RoomInformation>();
            }
            UpdateCarts();
        }

        private void UpdateCarts()
        {
            listView.ItemsSource = Session.carts.ToList();
            txtBoxTotalPrice.Text = Session.carts.Sum(room => room.RoomPricePerDay).ToString();
            home.UpdateCartQuantity();
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                RoomInformation room = Session.carts.Where(cart => cart.RoomId == (int)button.Tag).SingleOrDefault();
                if (room != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to remove this room from the cart?",
                        "Confirm Removal",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Session.carts.Remove(room);
                        UpdateCarts();
                    }
                }
            }
        }

        private void Button_Checkout(object sender, RoutedEventArgs e)
        {
            CreateNewBookingReservationToDb();
            this.Close();
        }

        private void CreateNewBookingReservationToDb()
        {
            // implements create new BookingReservation(newbrId,newDate,price = 0,cusId,status 1) by bookingRepository
            BookingReservation bookingReservation = GetBookingReservation();

            // implements create new BookingDetail(brId,RoomId,StartDate,EndDate,PriceRoom) by bookingDetailRepository
            List<BookingDetail> bookingDetails = new List<BookingDetail>();

            // set bookingDetails and update price total bookingReservation

            if (Session.carts.Count() > 0)
            {
                if (txtStartDate.SelectedDate.HasValue && txtEndDate.SelectedDate.HasValue)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Are you sure you want to proceed with the checkout?",
                        "Confirm Checkout",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (RoomInformation room in Session.carts)
                        {
                            bookingDetails.Add(GetBookingDetail(bookingReservation.BookingReservationId
                                , room, txtStartDate.SelectedDate.Value, txtEndDate.SelectedDate.Value));
                            bookingReservation.TotalPrice += room.RoomPricePerDay;
                        }

                        bookingRepository.Add(bookingReservation);

                        foreach (BookingDetail bookingDetail in bookingDetails)
                        {
                            bookingDetailRepository.Add(bookingDetail);
                        }

                        Session.carts = new List<RoomInformation>();
                        UpdateCarts();

                        MessageBox.Show("Booking successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Pls select Start and End Date", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Pls add more room", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private BookingReservation GetBookingReservation()
        {
            return new BookingReservation
            {
                BookingReservationId = bookingRepository.GetAll().Count(),
                BookingDate = DateTime.UtcNow,
                TotalPrice = 0,
                CustomerId = customer.CustomerId,
                BookingStatus = Convert.ToByte(1),
            };
        }

        private BookingDetail GetBookingDetail(int bookingReservationId, RoomInformation room, DateTime startDate, DateTime endDate)
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