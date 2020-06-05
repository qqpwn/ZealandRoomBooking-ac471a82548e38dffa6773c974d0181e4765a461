using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Appointments.DataProvider;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Media.Miracast;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }

        private string _dateBarString;

        public static ObservableCollection<Lokaler> ListOfRooms { get; set; } = new ObservableCollection<Lokaler>();
        public static ObservableCollection<Bookinger> ListOfBookinger { get; set; } = new ObservableCollection<Bookinger>();
        public static ObservableCollection<LokaleBookinger> ListOfLokaleBookinger { get; set; } = new ObservableCollection<LokaleBookinger>();

        public Lokaler RefLokaler = new Lokaler();
        public static User RefUser = new User();
        public int SelectedRoom { get; set; }
        public ICommand BookRoomCommand { get; set; }
        public ICommand DayForwardCommand { get; set; }
        public ICommand DayBackwardsCommand { get; set; }
        public ICommand SortAvailabilitySwitchCommand { get; set; }
        public ICommand SortBuildingSwitchCommand { get; set; }
        public ICommand SortFloorSwitchCommand { get; set; }
        public static DateTime BookingDate { get; set; } = DateTime.Now;
        public static int DaysAdded { get; set; } = 0;
        public int SortByAvailability { get; set; } = 0;
        public int SortByBuilding { get; set; } = 0;
        public int SortByFloor { get; set; } = 0;
        public SolidColorBrush AvailabilitySwitchColor { get; set; } = new SolidColorBrush(Colors.LightGray);
        public SolidColorBrush BuildingSwitchColor { get; set; } = new SolidColorBrush(Colors.LightGray);
        public SolidColorBrush FloorSwitchColor { get; set; } = new SolidColorBrush(Colors.LightGray);
        #endregion

        #region DateBarString
        public string DateBarString
        {
            get => _dateBarString;
            set
            {
                _dateBarString = value;
                OnPropertyChanged(nameof(DateBarString));
            }
        }
        #endregion

        #region Constructor
        public UserViewModel()
        {
            HentAlleBookinger();
            SortRoomList();
            SetRoomStatus();
            DateToString();
            BookRoomCommand = new RelayCommand(BookRoom);
            DayForwardCommand = new RelayCommand(DayForward);
            DayBackwardsCommand = new RelayCommand(DayBackwards);
            SortAvailabilitySwitchCommand = new RelayCommand(SortByAvailabilitySwitch);
            SortBuildingSwitchCommand = new RelayCommand(SortByBuildingSwitch);
            SortFloorSwitchCommand = new RelayCommand(SortByFloorSwitch);
            RefLokaler.AddtilList();
        }
        #endregion

        #region BookingDateMethods
        //Sætter datobaren en dag frem / tilbage 
        public void DayForward()
        {
            if (DaysAdded < 30)
            {
                BookingDate = BookingDate.AddDays(1);
                DaysAdded++;
                SetRoomStatus();
                if (SortByAvailability == 1)
                {
                    SortByAvailabilitySwitch();
                }
                if (SortByBuilding == 1)
                {
                    SortByBuildingSwitch();
                }
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
                if (SortByAvailability == 1)
                {
                    SortByAvailabilitySwitch();
                }
                if (SortByBuilding == 1)
                {
                    SortByBuildingSwitch();
                }
            }
            DateToString();
        }

        public void DateToString()
        {
            DateBarString = $"{BookingDate.Day}-{BookingDate.Month}-{BookingDate.Year}";
        }
        #endregion

        #region GetCollectionsMethod

        public void HentAlleBookinger()
        {
            ListOfBookinger = HomeViewModel.BookingerCollection;
            ListOfLokaleBookinger = HomeViewModel.LokaleBookingerCollection;
        }
        #endregion

        public void SortByAvailabilitySwitch()
        {
            if (SortByAvailability == 0)
            {
                SortByAvailability++;
                AvailabilitySwitchColor = new SolidColorBrush(Colors.ForestGreen);
                OnPropertyChanged(nameof(AvailabilitySwitchColor));
            }
            else
            {
                SortByAvailability--;
                AvailabilitySwitchColor = new SolidColorBrush(Colors.LightGray);
                OnPropertyChanged(nameof(AvailabilitySwitchColor));

                ListOfRooms = HomeViewModel.LokaleCollection;
            }
            SortRoomList();
            var userViewModel = new UserViewModel();
        }

        public void SortByBuildingSwitch()
        {
            if (SortByBuilding == 0)
            {
                SortByBuilding++;
                BuildingSwitchColor = new SolidColorBrush(Colors.ForestGreen);
                OnPropertyChanged(nameof(BuildingSwitchColor));
            }
            else
            {
                SortByBuilding--;
                BuildingSwitchColor = new SolidColorBrush(Colors.LightGray);
                OnPropertyChanged(nameof(BuildingSwitchColor));
            }
            SortRoomList();
            var userViewModel = new UserViewModel();
        }

        public void SortByFloorSwitch()
        {
            if (SortByFloor == 0)
            {
                SortByFloor++;
                FloorSwitchColor = new SolidColorBrush(Colors.ForestGreen);
                OnPropertyChanged(nameof(FloorSwitchColor));
            }
            else
            {
                SortByFloor--;
                FloorSwitchColor = new SolidColorBrush(Colors.LightGray);
                OnPropertyChanged(nameof(FloorSwitchColor));
            }
            SortRoomList();
            var userViewModel = new UserViewModel();
        }

        public void SortRoomList()
        {
            if (SortByAvailability == 1)
            {
                var tempAvailableCollection = new ObservableCollection<Lokaler>();
                if (RefUser.CheckedUser.Usertype == "Elev")
                {
                    foreach (var room in HomeViewModel.LokaleCollection)
                    {
                        if (room.Type == "Klasselokale" && room.BookingStatus <= 1)
                        {
                            tempAvailableCollection.Add(room);
                        }
                        else
                        {
                            if (room.Type == "Møderum" && room.BookingStatus == 0)
                            {
                                tempAvailableCollection.Add(room);
                            }
                        }
                    }
                }
                else if (RefUser.CheckedUser.Usertype == "Lære")
                {
                    foreach (var room in HomeViewModel.LokaleCollection)
                    {
                        if (BookingDate.Date <= DateTime.Now.AddDays(2).Date && room.BookingStatus == 0)
                        {
                            tempAvailableCollection.Add(room);
                        }
                        else if (BookingDate.Date >= DateTime.Now.AddDays(3).Date && room.BookingStatus <= 2)
                        {
                            tempAvailableCollection.Add(room);
                        }
                    }
                }

                if (SortByFloor == 1)
                {
                    var tempFloorCollection = new ObservableCollection<Lokaler>();
                    List<Lokaler> tempSortedFloorList = tempAvailableCollection.OrderBy(o => o.Etage).ToList();
                    foreach (var room in tempSortedFloorList)
                    {
                        tempFloorCollection.Add(room);
                    }

                    if (SortByBuilding == 1)
                    {
                        var tempBuildingCollection = new ObservableCollection<Lokaler>();
                        List<Lokaler> tempSortedBuildingList = tempFloorCollection.OrderBy(o => o.Bygning).ToList();
                        foreach (var room in tempSortedBuildingList)
                        {
                            tempBuildingCollection.Add(room);
                        }

                        ListOfRooms = tempBuildingCollection;
                    }
                    else
                    {
                        ListOfRooms = tempFloorCollection;
                    }
                }
                else if (SortByBuilding == 1)
                {
                    var tempBuildingCollection = new ObservableCollection<Lokaler>();
                    List<Lokaler> tempSortedBuildingList = tempAvailableCollection.OrderBy(o => o.Bygning).ToList();
                    foreach (var room in tempSortedBuildingList)
                    {
                        tempBuildingCollection.Add(room);
                    }

                    ListOfRooms = tempBuildingCollection;
                }
                else
                {
                    ListOfRooms = tempAvailableCollection;
                }
            }
            else if (SortByFloor == 1)
            {
                var tempFloorCollection = new ObservableCollection<Lokaler>();
                List<Lokaler> tempSortedFloorList = HomeViewModel.LokaleCollection.OrderBy(o => o.Etage).ToList();
                foreach (var room in tempSortedFloorList)
                {
                    tempFloorCollection.Add(room);
                }

                if (SortByBuilding == 1)
                {
                    var tempBuildingCollection = new ObservableCollection<Lokaler>();
                    List<Lokaler> tempSortedBuildingList = tempFloorCollection.OrderBy(o => o.Bygning).ToList();
                    foreach (var room in tempSortedBuildingList)
                    {
                        tempBuildingCollection.Add(room);
                    }

                    ListOfRooms = tempBuildingCollection;
                }
                else
                {
                    ListOfRooms = tempFloorCollection;
                }
            }
            else if (SortByBuilding == 1)
            {
                var tempBuildingCollection = new ObservableCollection<Lokaler>();
                List<Lokaler> tempSortedBuildingList = HomeViewModel.LokaleCollection.OrderBy(o => o.Bygning).ToList();
                foreach (var room in tempSortedBuildingList)
                {
                    tempBuildingCollection.Add(room);
                }

                ListOfRooms = tempBuildingCollection;
            }
            else
            {
                ListOfRooms = HomeViewModel.LokaleCollection;
            }
            OnPropertyChanged(nameof(ListOfRooms));
        }

        #region SetRoomStatusMethod
        //Sætter BookingStatus på hver lokale 
        public async void SetRoomStatus()
        {
            foreach (var room in ListOfRooms)
            {
                room.BookingStatus = 0;
                RefLokaler.ColorCodes(room);
            }
            foreach (var booking in ListOfBookinger)
            {
                if (booking.Date.Date == BookingDate.Date)
                {
                    var tempUser = await PersistencyService<User>.GetObjectFromId(booking.UserId, "User");
                    foreach (var lokaleBooking in ListOfLokaleBookinger)
                    {
                        if (lokaleBooking.BookingId == booking.BookingId)
                        {
                            foreach (var room in ListOfRooms)
                            {
                                if (room.LokaleId == lokaleBooking.LokaleId)
                                {
                                    if (tempUser.Usertype == "Elev")
                                    {
                                        room.BookingStatus++;
                                        RefLokaler.ColorCodes(room);
                                        break;
                                    }
                                    else if (tempUser.Usertype == "Lære")
                                    {
                                        room.BookingStatus = room.BookingStatus + 3;
                                        RefLokaler.ColorCodes(room);
                                        break;
                                    }
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

        //Booker lokaler med checks
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
                        if (BookingDate.Date >= DateTime.Now.AddDays(3).Date && selectedRoom.BookingStatus <= 2 && RefUser.CheckedUser.Usertype == "Lære")
                        {
                            DeleteElevBooking();
                            BookingCheckLærer();
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
                            if (BookingDate.Date >= DateTime.Now.AddDays(3).Date && selectedRoom.BookingStatus <= 2 && RefUser.CheckedUser.Usertype == "Lære")
                            {
                                DeleteElevBooking();
                                BookingCheckLærer();
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
                if (roomStatus > 0 && booking.Date.Date == BookingDate.Date)
                {
                    foreach (var lokaleBooking in ListOfLokaleBookinger)
                    {
                        if (lokaleBooking.BookingId == booking.BookingId && lokaleBooking.LokaleId == selectedRoom.LokaleId)
                        {
                            PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                            PersistencyService<Bookinger>.DeleteObject(booking.BookingId, "Bookinger");
                            roomStatus--;
                            break;
                        }
                    }
                }
                else
                {
                    if (roomStatus == 0)
                    {
                        break;
                    }
                }
            }
        }

        public void BookingCheckElev()
        {
            var bookingOnThisDate = 0;
            var bookingCount = 0;
            foreach (var booking in ListOfBookinger)
            {
                if (bookingOnThisDate == 0 && bookingCount <= 2)
                {
                    if (booking.UserId == RefUser.CheckedUser.UserId)
                    {
                        bookingCount++;
                        if (booking.Date.Date == BookingDate.Date)
                        {
                            bookingOnThisDate++;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            if (bookingOnThisDate == 0 && bookingCount <= 2)
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
                    if (booking.UserId == RefUser.CheckedUser.UserId && booking.Date.Date == BookingDate.Date)
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
