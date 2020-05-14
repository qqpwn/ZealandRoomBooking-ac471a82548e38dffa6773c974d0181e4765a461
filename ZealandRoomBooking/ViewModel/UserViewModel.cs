using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Miracast;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Lokaler> _listOfRooms = new ObservableCollection<Lokaler>();

        public ObservableCollection<Lokaler> ListOfRooms
        {
            get { return _listOfRooms; }
        }

        public UserViewModel()
        {
            HentLokaler();
        }

        public async void HentLokaler()
        {
            ObservableCollection<Lokaler> tempCollection = await PersistencyService<Lokaler>.GetObjects("Lokaler");
            _listOfRooms = tempCollection;
        }

        public void BookLokale()
        {
            User user = new User();

            Bookinger bookinger = new Bookinger()
            {
                UserId = user.UserId
            };

            PersistencyService<Bookinger>.PostObject("Bookinger", bookinger);

            //LokaleBookinger lokaleBookinger = new LokaleBookinger()
            //{
            //    BookingId = bookinger.BookingId,
            //    LokaleId = 1
            //};

            PersistencyService<LokaleBookinger>.PostObject("LokaleBookinger", lokaleBookinger);

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
