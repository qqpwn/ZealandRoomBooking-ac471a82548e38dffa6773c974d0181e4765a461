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
using Windows.UI;
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
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }
        

        public int LokaleId1
        {
            set { RefLokaler.LokaleIdFraViewModel = _listOfRooms[SelectedRoom].LokaleId; }
        }
        public User RefUser { get; set; }
        public Lokaler RefLokaler = new Lokaler();
        private static ObservableCollection<Lokaler> _listOfRooms;
        public ObservableCollection<Lokaler> ListOfRooms
        {
            get { return _listOfRooms; }
        }
        public ICommand BookRoomCommand { get; set; }
        public int SelectedRoom { get; set; }

        public UserViewModel()
        {
            HentLokaler();
            BookRoomCommand = new RelayCommand(CreateRoomBooking);
            RefLokaler = new Lokaler();
            RefLokaler.HentFraPersistency();
            
        }

        public SolidColorBrush Color { get { return RefLokaler.Color; } set { RefLokaler.Color = value; OnPropertyChanged(); } }

        
        public  void HentLokaler()
        {
            _listOfRooms = SimpletonLokaler.Instance.MineLokaler;
            
        }

       

        public async void CreateRoomBooking()
        {
            RefUser = new User();
            Bookinger booking = new Bookinger(new DateTime(2020, 7, 5), RefUser.CheckedUser.UserId);

            await PersistencyService<Bookinger>.PostObject("Bookinger", booking);

            var getBooking = await PersistencyService<Bookinger>.GetObjects("Bookinger");
            int getId = getBooking.Last().BookingId;
            LokaleBookinger lokalebooking = new LokaleBookinger(getId, ListOfRooms[SelectedRoom].LokaleId);
            Lokaler tempLokale = new Lokaler(ListOfRooms[SelectedRoom].Etage, $"{ListOfRooms[SelectedRoom].Type}", $"{ListOfRooms[SelectedRoom].Navn}", $"{ListOfRooms[SelectedRoom].Bygning}");
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
