using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Eventmaker.Common;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.ViewModel
{
    class MyBookingsViewModel
    {
        public string RoomName { get; set; }
        private ObservableCollection<Bookinger> _myBookingsList = new ObservableCollection<Bookinger>();
        public ObservableCollection<Bookinger> MyBookingsList
        {
            get { return _myBookingsList; }
        }
        public ObservableCollection<Bookinger> AllBookings = new ObservableCollection<Bookinger>();
        public ObservableCollection<LokaleBookinger> AllLokaleBookings = new ObservableCollection<LokaleBookinger>();
        public ObservableCollection<Lokaler> AllRooms = new ObservableCollection<Lokaler>();
        public int SelectedBooking { get; set; }
        User refUser = new User();
        public ICommand DeleteBookingCommand { get; set; }

        public MyBookingsViewModel()
        {
            GetAllBookingInfo();
            DeleteBookingCommand = new RelayCommand(DeleteBooking);
        }

        public async void GetAllBookingInfo()
        {
            ObservableCollection<Bookinger> tempBookingCollection = await PersistencyService<Bookinger>.GetObjects("Bookinger");
            AllBookings = tempBookingCollection;

            ObservableCollection<LokaleBookinger> tempLokaleBookingCollection = await PersistencyService<LokaleBookinger>.GetObjects("LokaleBookinger");
            AllLokaleBookings = tempLokaleBookingCollection;

            ObservableCollection<Lokaler> tempRoomCollection = await PersistencyService<Lokaler>.GetObjects("Lokaler");
            AllRooms = tempRoomCollection;

            GetMyBookings();
        }

        public void GetMyBookings()
        {
            foreach (var booking in AllBookings)
            {
                if (booking.UserId == refUser.CheckedUser.UserId)
                {
                    foreach (var lokaleBooking in AllLokaleBookings)
                    {
                        if (lokaleBooking.BookingId == booking.BookingId)
                        {
                            foreach (var room in AllRooms)
                            {
                                if (room.LokaleId == lokaleBooking.LokaleId)
                                {
                                    booking.RoomName = room.Navn;
                                    MyBookingsList.Add(booking);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void DeleteBooking()
        {
            foreach (var lokaleBooking in AllLokaleBookings)
            {
                if (lokaleBooking.BookingId == MyBookingsList[SelectedBooking].BookingId)
                {
                    PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                    PersistencyService<Bookinger>.DeleteObject(MyBookingsList[SelectedBooking].BookingId, "Bookinger");
                    break;
                }
            }
        }
    }
}
