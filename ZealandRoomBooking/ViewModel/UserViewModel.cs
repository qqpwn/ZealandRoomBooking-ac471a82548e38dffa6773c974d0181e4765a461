using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Media.Miracast;
using Eventmaker.Common;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;
using ZealandRoomBooking.View;

namespace ZealandRoomBooking.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        #region Properties
        private static ObservableCollection<Lokaler> _listOfRooms = new ObservableCollection<Lokaler>();
        private static ObservableCollection<Bookinger> _listOfBookinger = new ObservableCollection<Bookinger>();
        private static ObservableCollection<LokaleBookinger> _listOfLokaleBookinger = new ObservableCollection<LokaleBookinger>();
        private string _dateBarString;

        public static ObservableCollection<Lokaler> ListOfRooms
        {
            get { return _listOfRooms; }
        }

        public static ObservableCollection<Bookinger> ListOfBookinger
        {
            get { return _listOfBookinger; }
        }

        public static ObservableCollection<LokaleBookinger> ListOfLokaleBookinger
        {
            get { return _listOfLokaleBookinger; }
        }

        public static User RefUser = new User();
        public ICommand BookRoomCommand { get; set; }
        public int SelectedRoom { get; set; }
        public ICommand DayForwardCommand { get; set; }
        public ICommand DayBackwardsCommand { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public int DaysAdded { get; set; } = 0;

        public string DateBarString
        {
            get => _dateBarString;
            set
            {
                _dateBarString = value;
                OnPropertyChanged("DateBarString");
            }
        } 
        #endregion

        #region Constructor
        public UserViewModel()
        {
            HentAlleCollections();
            DateToString();
            BookRoomCommand = new RelayCommand(BookRoom);
            DayForwardCommand = new RelayCommand(DayForward);
            DayBackwardsCommand = new RelayCommand(DayBackwards);
        } 
        #endregion

        #region BookingDateMethods
        public void DayForward()
        {
            if (DaysAdded < 30)
            {
                BookingDate = BookingDate.AddDays(1);
                DaysAdded++;
                SetRoomStatus();
            }
            DateToString();
        }

        public void DayBackwards()
        {
            if (DaysAdded > 0)
            {
                BookingDate = BookingDate.AddDays(-1);
                DaysAdded--;
                SetRoomStatus();
            }
            DateToString();
        }

        public void DateToString()
        {
            DateBarString = $"{BookingDate.Day}-{BookingDate.Month}-{BookingDate.Year}";
        }
        #endregion

        #region GetCollectionsMethod

        public async void HentAlleCollections()
        {
            ObservableCollection<Lokaler> tempLCollection =
                await PersistencyService<Lokaler>.GetObjects("Lokaler");
            _listOfRooms = tempLCollection;

            ObservableCollection<Bookinger> tempBCollection =
                await PersistencyService<Bookinger>.GetObjects("Bookinger");
            _listOfBookinger = tempBCollection;

            ObservableCollection<LokaleBookinger> tempLBCollection = await PersistencyService<LokaleBookinger>.GetObjects("LokaleBookinger");
            _listOfLokaleBookinger = tempLBCollection;

            SetRoomStatus();
        }
        #endregion

        #region SetRoomStatusMethod
        public async void SetRoomStatus()
        {
            var userInt = 0;
            foreach (var booking in ListOfBookinger)
            {
                if (booking.Date.Day == BookingDate.Day && booking.Date.Month == BookingDate.Month && booking.Date.Year == BookingDate.Year)
                {
                    var tempUser = await PersistencyService<User>.GetObjectFromId(booking.UserId, "User");
                    if (tempUser.Usertype == "Elev")
                    {
                        userInt++;
                    }
                    else
                    {
                        userInt = userInt + 3;
                    }

                    foreach (var lokaleBooking in ListOfLokaleBookinger)
                    {
                        if (lokaleBooking.BookingId == booking.BookingId)
                        {
                            foreach (var room in ListOfRooms)
                            {
                                if (room.LokaleId == lokaleBooking.LokaleId)
                                {
                                    room.BookingStatus = userInt;
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        } 
        #endregion

        #region RoomBookingMethods

        public static Lokaler selectedRoom = new Lokaler();

        public void SelectedTempRoom()
        {
            selectedRoom = ListOfRooms[SelectedRoom];
        }

        public void BookRoom()
        {
            SelectedTempRoom();
            if (selectedRoom.Type == "Klasselokale")
            {
                if (RefUser.CheckedUser.Usertype == "Elev" && selectedRoom.BookingStatus <= 1)
                {
                    BookingCheckElev();
                }
                else
                {
                    if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus == 0)
                    {
                        BookingCheckLærer();
                    }
                    else
                    {
                        //Work in progress, lærer skal ikke kunne overskrive en anden lærers booking
                        //og den skal også slette elevens booking
                        if (BookingDate.Day >= DateTime.Now.AddDays(3).Day && selectedRoom.BookingStatus <= 2)
                        {
                            BookingCheckLærer();
                            DeleteElevBooking();
                        }
                    }
                }
            }
            else
            {
                if (selectedRoom.Type == "Møderum")
                {
                    if (RefUser.CheckedUser.Usertype == "Elev" && selectedRoom.BookingStatus == 0)
                    {
                        BookingCheckElev();
                    }
                    else
                    {
                        if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus == 0)
                        {
                            BookingCheckLærer();
                        }
                        else
                        {
                            //Work in progress, lærer skal ikke kunne overskrive en anden lærers booking
                            //og den skal også slette elevens booking
                            if (BookingDate.Day >= DateTime.Now.AddDays(3).Day && selectedRoom.BookingStatus <= 2)
                            {
                                BookingCheckLærer();
                                DeleteElevBooking();
                            }
                        }
                    }
                }
            }
        }

        //Kun brugt når en lærer overskriver elevers booking
        public void DeleteElevBooking()
        {
            var roomStatus = selectedRoom.BookingStatus;
            foreach (var booking in ListOfBookinger)
            {
                if (booking.Date.Day == BookingDate.Day && booking.Date.Month == BookingDate.Month && booking.Date.Year == BookingDate.Year)
                {
                    
                    foreach (var lokaleBooking in ListOfLokaleBookinger)
                    {
                        if (lokaleBooking.BookingId == booking.BookingId && lokaleBooking.LokaleId == selectedRoom.LokaleId)
                        {
                            if (selectedRoom.Type == "Klasselokale")
                            {
                                if (roomStatus == 1)
                                {
                                    PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                                    PersistencyService<Bookinger>.DeleteObject(booking.BookingId, "Bookinger");
                                    break;
                                }
                                else
                                {
                                    if (roomStatus == 2)
                                    {
                                        PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                                        PersistencyService<Bookinger>.DeleteObject(booking.BookingId, "Bookinger");
                                        roomStatus--;
                                    }
                                }
                            }
                            else
                            {
                                if (selectedRoom.Type == "Møderum")
                                {
                                    PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                                    PersistencyService<Bookinger>.DeleteObject(booking.BookingId, "Bookinger");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BookingCheckElev()
        {
            var bookingOnThisDate = 0;
            foreach (var booking in ListOfBookinger)
            {
                if (bookingOnThisDate == 0)
                {
                    if (booking.UserId == RefUser.CheckedUser.UserId && booking.Date.Day == BookingDate.Day
                                                                     && booking.Date.Month == BookingDate.Month && booking.Date.Year == BookingDate.Year)
                    {
                        bookingOnThisDate++;
                    }
                }
                else
                {
                    break;
                }
            }
            if (bookingOnThisDate == 0)
            {
                CreateRoomBooking();
            }
        }

        public void BookingCheckLærer()
        {
            var bookingOnThisDate = 0;
            foreach (var booking in ListOfBookinger)
            {
                if (bookingOnThisDate <= 2)
                {
                    if (booking.UserId == RefUser.CheckedUser.UserId && booking.Date.Day == BookingDate.Day
                                                                     && booking.Date.Month == BookingDate.Month && booking.Date.Year == BookingDate.Year)
                    {
                        bookingOnThisDate++;
                    }
                }
                else
                {
                    break;
                }
            }
            if (bookingOnThisDate <= 2)
            {
                CreateRoomBooking();
            }
        }

        public async void CreateRoomBooking()
        {
            Bookinger booking = new Bookinger(BookingDate, RefUser.CheckedUser.UserId);

            await PersistencyService<Bookinger>.PostObject("Bookinger", booking);

            var getBooking = await PersistencyService<Bookinger>.GetObjects("Bookinger");
            int getId = getBooking.Last().BookingId;

            LokaleBookinger lokalebooking = new LokaleBookinger(getId, selectedRoom.LokaleId);
            Lokaler tempLokale = new Lokaler(selectedRoom.Etage, $"{selectedRoom.Type}", $"{selectedRoom.Navn}", $"{selectedRoom.Bygning}");

            await PersistencyService<LokaleBookinger>.PostObject("LokaleBookinger", lokalebooking);
        }
        #endregion

        #region PropertyChangedSupport
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
