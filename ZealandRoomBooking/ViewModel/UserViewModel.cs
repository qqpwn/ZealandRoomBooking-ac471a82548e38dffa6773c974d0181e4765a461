using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Miracast;
using Eventmaker.Common;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public User refUser { get; set; }
        private static ObservableCollection<Lokaler> _listOfRooms;
        public static ObservableCollection<Lokaler> ListOfRooms
        {
            get { return _listOfRooms; }
        }
        public ICommand BookRoomCommand { get; set; }
        public int SelectedRoom { get; set; }

        public UserViewModel()
        {
            HentLokaler();
            BookRoomCommand = new RelayCommand(CreateRoomBooking);
        }

        public async void HentLokaler()
        {
            ObservableCollection<Lokaler> tempCollection = await PersistencyService<Lokaler>.GetObjects("Lokaler");
            _listOfRooms = tempCollection;
        }

        public async void CreateRoomBooking()
        {
            refUser = new User();
            Bookinger booking = new Bookinger(new DateTime(2020, 7, 5), refUser.CheckedUser.UserId);

            await PersistencyService<Bookinger>.PostObject("Bookinger", booking);

            var getBooking = await PersistencyService<Bookinger>.GetObjects("Bookinger");
            int getId = getBooking.Last().BookingId;
            int BStatus = 1;
            LokaleBookinger lokalebooking = new LokaleBookinger(getId, ListOfRooms[SelectedRoom].LokaleId);
            Lokaler tempLokale = new Lokaler(ListOfRooms[SelectedRoom].Etage, $"{ListOfRooms[SelectedRoom].Type}", $"{ListOfRooms[SelectedRoom].Navn}", $"{ListOfRooms[SelectedRoom].Bygning}", BStatus);
            PersistencyService<Lokaler>.PutObject(ListOfRooms[SelectedRoom].LokaleId, "Lokaler", tempLokale);

            await PersistencyService<LokaleBookinger>.PostObject("LokaleBookinger", lokalebooking);

        }

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
