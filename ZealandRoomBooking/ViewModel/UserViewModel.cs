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
        public int LokaleId { get; set; }

        private ObservableCollection<Lokaler> _listOfRooms = new ObservableCollection<Lokaler>();
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
        }

        public async void HentLokaler()
        {
            ObservableCollection<Lokaler> tempCollection = await PersistencyService<Lokaler>.GetObjectFromId(1, "Lokaler");
            _listOfRooms = tempCollection;
        }

        public void CreateRoomBooking()
        {
            //Bookinger bookinger = new Bookinger()
            //{
            //    BookingId = 1,
            //    Date = new DateTime(2020, 5, 5),
            //    UserId = 1
            //};

            //PersistencyService<Bookinger>.PostObject("Bookinger", bookinger);
            
            //Bookinger test = new Bookinger(12,new DateTime(2020,05,22, 00,00,00).ToString("d"), 2);

            //PersistencyService<Bookinger>.PostObject(test, "Bookinger");

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
