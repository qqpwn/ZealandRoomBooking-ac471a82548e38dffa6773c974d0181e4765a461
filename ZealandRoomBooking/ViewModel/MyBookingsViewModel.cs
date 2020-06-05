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
using System.ServiceModel.Channels;
using Windows.UI.Popups;
using ZealandRoomBooking.View;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZealandRoomBooking.Annotations;

namespace ZealandRoomBooking.ViewModel
{
    class MyBookingsViewModel : INotifyPropertyChanged
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


        //Henter den loggede inds brugers bookinger
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

        ////Delete booking
        public async void DeleteBooking()
        {
            if (MyBookingsList.Count == 0)
            {
                var messageDialog = new MessageDialog("Du har ingen bookinger");
                messageDialog.Commands.Add(new UICommand("OK", null));
                await messageDialog.ShowAsync();

            }
            else if (MyBookingsList.Count > 0)
            {
                var messageDialog = new MessageDialog("Er du sikker på at du vil slette din valgte booking?");

                messageDialog.Commands.Add(new UICommand("Ja", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand("Nej", null));

                await messageDialog.ShowAsync();
            }


        }

        private void CommandInvokedHandler(IUICommand command)
        {
            foreach (var lokaleBooking in AllLokaleBookings)
            {
                if (lokaleBooking.BookingId == MyBookingsList[SelectedBooking].BookingId)
                {
                    PersistencyService<LokaleBookinger>.DeleteObject(lokaleBooking.LBId, "LokaleBookinger");
                    PersistencyService<Bookinger>.DeleteObject(MyBookingsList[SelectedBooking].BookingId, "Bookinger");
                    MyBookingsList.Remove(MyBookingsList[SelectedBooking]);
                    OnPropertyChanged(nameof(MyBookingsList));
                }
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
