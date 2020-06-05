using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.ViewModel
{
    public class HomeViewModel
    {
        public string LoggedInUserText { get; set; }
        public User RefUser = new User();
        public static ObservableCollection<Lokaler> LokaleCollection = new ObservableCollection<Lokaler>();
        public static ObservableCollection<Bookinger> BookingerCollection = new ObservableCollection<Bookinger>();
        public static ObservableCollection<LokaleBookinger> LokaleBookingerCollection = new ObservableCollection<LokaleBookinger>();

        public HomeViewModel()
        {
            GetBookinger();
            GetLokaler();
            SetUserText();
        }

        //Sætter Home textblock afhængig af bruger type, så man kan se hvilken bruger er logget ind
        public void SetUserText()
        {
            if (RefUser.CheckedUser.Usertype == "Elev")
            {
                LoggedInUserText = "Studerende";
            }
            else
            {
                LoggedInUserText = "Underviser";
            }
        }

        public async void GetBookinger()
        {
            ObservableCollection<Bookinger> tempBCollection = await PersistencyService<Bookinger>.GetObjects("Bookinger");
            BookingerCollection = tempBCollection;

            ObservableCollection<LokaleBookinger> tempLBCollection = await PersistencyService<LokaleBookinger>.GetObjects("LokaleBookinger");
            LokaleBookingerCollection = tempLBCollection;
        }

        public async void GetLokaler()
        {
            ObservableCollection<Lokaler> tempLCollection = await PersistencyService<Lokaler>.GetObjects("Lokaler");
            LokaleCollection = tempLCollection;
        }
    }
}
