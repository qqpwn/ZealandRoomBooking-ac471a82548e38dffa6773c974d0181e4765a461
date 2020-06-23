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
using System.Linq.Expressions;
using Windows.UI.Popups;

namespace ZealandRoomBooking.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        #region Properties
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
        public string AvailabilitySwitchColor { get; set; } = "#fff387";
        public string BuildingSwitchColor { get; set; } = "#fff387";
        public string FloorSwitchColor { get; set; } = "#fff387";
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
        public async void DayForward()
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
            else
            {
                var messageDialog = new MessageDialog("Du kan kun booke et lokale op til 30 dage frem.");
                messageDialog.Commands.Add(new UICommand("Ok", null));
                await messageDialog.ShowAsync();
            }
            DateToString();
        }

        public async void DayBackwards()
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
            else
            {
                var messageDialog = new MessageDialog("Du kan ikke gå tilbage i tiden og booke et lokale.");
                messageDialog.Commands.Add(new UICommand("Ok", null));
                await messageDialog.ShowAsync();
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

        #region SortingMethods

        public void SortByAvailabilitySwitch()
        {
            if (SortByAvailability == 0)
            {
                SortByAvailability++;
                AvailabilitySwitchColor = $"{Colors.Green}";
                OnPropertyChanged(nameof(AvailabilitySwitchColor));
            }
            else
            {
                SortByAvailability--;
                AvailabilitySwitchColor = "#fff387";
                OnPropertyChanged(nameof(AvailabilitySwitchColor));
            }
            SortRoomList();
            var userViewModel = new UserViewModel();
        }

        public void SortByBuildingSwitch()
        {
            if (SortByBuilding == 0)
            {
                SortByBuilding++;
                BuildingSwitchColor = $"{Colors.Green}";
                OnPropertyChanged(nameof(BuildingSwitchColor));
            }
            else
            {
                SortByBuilding--;
                BuildingSwitchColor = "#fff387";
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
                FloorSwitchColor = $"{Colors.Green}";
                OnPropertyChanged(nameof(FloorSwitchColor));
            }
            else
            {
                SortByFloor--;
                FloorSwitchColor = "#fff387";
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
        #endregion

        #region SetRoomStatusMethod
        //Sætter BookingStatus på hver lokale 
        public async void SetRoomStatus()
        {
            var NoErrorsBookinger = ListOfBookinger;
            foreach (var room in ListOfRooms)
            {
                room.BookingStatus = 0;
                RefLokaler.ColorCodes(room);
            }
            foreach (var booking in NoErrorsBookinger)
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
        public async void BookRoom()
        {
            SelectedTempRoom();
            if (selectedRoom.Type == "Klasselokale")
            {
                if (RefUser.CheckedUser.Usertype == "Elev" && selectedRoom.BookingStatus <= 1)
                {
                    BookingCheckElev();
                }
                else if (RefUser.CheckedUser.Usertype == "Elev" && selectedRoom.BookingStatus >= 2)
                {
                    var messageDialog = new MessageDialog("Dette lokale er allerede booket. Book et andet lokale, som er ledigt.");
                    messageDialog.Commands.Add(new UICommand("Ok", null));
                    await messageDialog.ShowAsync();
                }
                else
                {
                    if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus == 0)
                    {
                        BookingCheckLærer();
                    }
                    else if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus > 0 && BookingDate.Date < DateTime.Now.AddDays(3).Date)
                    {
                        var messageDialog = new MessageDialog("Dette lokale er allerede booket. Book et andet lokale, som er ledigt.");
                        messageDialog.Commands.Add(new UICommand("Ok", null));
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        if (BookingDate.Date >= DateTime.Now.AddDays(3).Date && selectedRoom.BookingStatus <= 2 && RefUser.CheckedUser.Usertype == "Lære")
                        {
                            var messageDialog = new MessageDialog("Er du sikker på at du vil overskrive den studerendes booking?");

                            messageDialog.Commands.Add(new UICommand("Ja", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                            messageDialog.Commands.Add(new UICommand("Nej", null));

                            await messageDialog.ShowAsync();
                        }
                        else if (selectedRoom.BookingStatus == 3)
                        {
                            var messageDialog = new MessageDialog("Lokalet er optaget");
                            messageDialog.Commands.Add(new UICommand("Ok", null));
                            await messageDialog.ShowAsync();
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
                    else if (RefUser.CheckedUser.Usertype == "Elev" && selectedRoom.BookingStatus >= 1)
                    {
                        var messageDialog = new MessageDialog("Dette lokale er allerede booket. Book et andet lokale, som er ledigt.");
                        messageDialog.Commands.Add(new UICommand("Ok", null));
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus == 0)
                        {
                            BookingCheckLærer();
                        }
                        else if (RefUser.CheckedUser.Usertype == "Lære" && selectedRoom.BookingStatus > 0 && BookingDate.Date < DateTime.Now.AddDays(3).Date)
                        {
                            var messageDialog = new MessageDialog("Dette lokale er allerede booket. Book et andet lokale, som er ledigt.");
                            messageDialog.Commands.Add(new UICommand("Ok", null));
                            await messageDialog.ShowAsync();
                        }
                        else
                        {
                            if (BookingDate.Date >= DateTime.Now.AddDays(3).Date && selectedRoom.BookingStatus <= 2 && RefUser.CheckedUser.Usertype == "Lære")
                            {
                                var messageDialog = new MessageDialog("Er du sikker på at du vil overskrive den studerendes booking?");

                                messageDialog.Commands.Add(new UICommand("Ja", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                                messageDialog.Commands.Add(new UICommand("Nej", null));

                                await messageDialog.ShowAsync();
                            }
                            else if (selectedRoom.BookingStatus == 3)
                            {
                                var messageDialog = new MessageDialog("Lokalet er optaget");
                                messageDialog.Commands.Add(new UICommand("Ok", null));
                                await messageDialog.ShowAsync();
                            }
                        }
                    }
                }
            }
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            BookingCheckLærer();
        }


        //Kun brugt når en lærer overskriver elevers booking
        public void DeleteElevBooking()
        {
            var NoErrorsLokaleBookinger = ListOfLokaleBookinger;
            var NoErrorsBookinger = ListOfBookinger;
            var roomStatus = selectedRoom.BookingStatus;
            foreach (var booking in NoErrorsBookinger)
            {
                if (roomStatus > 0 && booking.Date.Date == BookingDate.Date)
                {
                    foreach (var lokaleBooking in NoErrorsLokaleBookinger)
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



        public async void BookingCheckElev()
        {
            var bookingOnThisDate = 0;
            var bookingCount = 0;
            foreach (var booking in ListOfBookinger)
            {
                if (bookingOnThisDate == 0)
                {
                    if (bookingCount <= 2)
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
                }

            }
            if (bookingOnThisDate == 0 && bookingCount <= 2)
            {
                CreateRoomBooking();
            }
            else if (bookingCount > 2)
            {
                var messageDialog = new MessageDialog("Du har allerede booket 3 lokaler, det er ikke muligt at booke flere end 3.");
                messageDialog.Commands.Add(new UICommand("Ok", null));
                await messageDialog.ShowAsync();
            }
            else
            {
                var messageDialog = new MessageDialog("Du har allerede booket ét lokale idag.Det er kun muligt at booke ét lokale på samme dag.");
                messageDialog.Commands.Add(new UICommand("Ok", null));
                await messageDialog.ShowAsync();
            }
        }

        public async void BookingCheckLærer()
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
                DeleteElevBooking();
                CreateRoomBooking();
            }
            else
            {
                var messageDialog = new MessageDialog("Du har allerede booket 3 lokaler idag, det er ikke muligt at booke flere end 3 på denne dato.");
                messageDialog.Commands.Add(new UICommand("Ok", null));
                await messageDialog.ShowAsync();
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
            ((Frame)Window.Current.Content).Navigate(typeof(View.MyBookingsPage));
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
