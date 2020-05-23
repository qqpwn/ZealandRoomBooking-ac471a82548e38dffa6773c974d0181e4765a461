using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Persistency;
using ZealandRoomBooking.View;
using ZealandRoomBooking.ViewModel;

namespace ZealandRoomBooking.Model
{
    public class Lokaler : INotifyPropertyChanged
    {
        private SolidColorBrush _color = new SolidColorBrush(Colors.GreenYellow);
        private static int _bookingStatus = 0;
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }
        public int BookingStatus { get { return _bookingStatus; } set { _bookingStatus = value; OnPropertyChanged(); } }
        public SolidColorBrush Color
        {
            get { return Test(); }
            set { _color = value; OnPropertyChanged(); }
        }

        public Lokaler(int etage, string type, string navn, string bygning)
        {
            Etage = etage;
            Type = type;
            Navn = navn;
            Bygning = bygning;

        }

        public Lokaler()
        {

        }




        private static readonly List<LokaleBookinger> AlleLokaleBookingers = new List<LokaleBookinger>();
        private static readonly List<Bookinger> AlleBookingers = new List<Bookinger>();


        public static List<LokaleBookinger> MineLokalerBookingers
        {
            get { return AlleLokaleBookingers; }
        }
        public static List<Bookinger> MineBookingers
        {
            get { return AlleBookingers; }
        }

        public async void HentFraPersistency()
        {

            //RefUserViewModel = new UserViewModel();
            //foreach (var a in RefUserViewModel.ListOfRooms)
            //{
            //    _alleLokalers.Add(a);
            //}

            await PersistencyService<LokaleBookinger>.GetObjects("LokaleBookinger");
            ObservableCollection<LokaleBookinger> test1 = PersistencyService<LokaleBookinger>.HentCollection;
            foreach (var o in test1)
            {
                AlleLokaleBookingers.Add(o);
            }

            await PersistencyService<Bookinger>.GetObjects("Bookinger");
            ObservableCollection<Bookinger> test2 = PersistencyService<Bookinger>.HentCollection;
            foreach (var o in test2)
            {
                AlleBookingers.Add(o);
            }
        }


        public int fick = 0;

        public SolidColorBrush Test()
        {

            RefBookinger = new Bookinger();
            RefLokaleBookinger = new LokaleBookinger();
            RefUser = new User();
            LedighedsSortCheckBookinger(AlleLokaleBookingers);
            foreach (var a in SimpletonLokaler.Instance.MineLokaler)
            {
                if (a.LokaleId == LokaleId && a.Type == "Klasselokale" && RefUser.CheckedUser.Usertype == "Elev")
                {
                    foreach (var b in CheckedBookinger)
                    {

                        if (RefUser.CheckedUser.UserId == b.UserId)
                        {
                            a._color = new SolidColorBrush(Colors.Yellow);
                            _color = a._color; OnPropertyChanged(nameof(Color));
                            return _color;
                        }
                    }
                }
                else if (a.LokaleId == LokaleId && RefUser.CheckedUser.Usertype == "Lære" || a.LokaleId == LokaleId && RefUser.CheckedUser.Usertype == "Elev" && a.Type == "Møderum")
                {
                    foreach (var b in CheckedBookinger)
                    {
                        if (RefUser.CheckedUser.UserId == b.UserId)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color; OnPropertyChanged(nameof(Color));
                            return _color;
                        }
                    }
                }
                
            }
            return _color;
        }

        public static DateTime Dato = new DateTime(2020, 05, 20);
        public LokaleBookinger RefLokaleBookinger { get; set; }
        public static Bookinger RefBookinger { get; set; }
        public User RefUser { get; set; }
        //public void LedighedsCheck()
        //{


        //    RefUser = new User();

        //    if (AlleLokaleBookingers.Find(_bookingerPredicate) != null && CheckedBookinger.Date == Dato && RefUser.CheckedUser.UserId == CheckedBookinger.UserId && RefUser.CheckedUser.Usertype == "Elev")
        //    {
        //        fick++;
        //    }
        //    else if (AlleLokaleBookingers.Find(_bookingerPredicate) != null && CheckedBookinger.Date == Dato && RefUser.CheckedUser.UserId == CheckedBookinger.UserId &&
        //             RefUser.CheckedUser.Usertype == "Lære")
        //    {
        //        fick = 2;
        //    }

        //    //if (RefLokaleBookinger.LokaleId == AlleLokalers[SelectedItem].LokaleId && RefBookinger.Date == Dato &&
        //    //    RefUser.CheckedUser.Usertype == "Elev" && AlleLokalers[SelectedItem].Type == "Klasselokale")
        //    //{
        //    //    fick = 1;
        //    //}
        //    //else if (RefLokaleBookinger.LokaleId == AlleLokalers[SelectedItem].LokaleId && RefBookinger.Date == Dato &&
        //    //         RefUser.CheckedUser.Usertype == "Elev" && AlleLokalers[SelectedItem].Type == "Møderum")
        //    //{
        //    //    fick = 2;
        //    //}
        //    //else if (RefLokaleBookinger.LokaleId == AlleLokalers[SelectedItem].LokaleId && RefBookinger.Date == Dato &&
        //    //         RefUser.CheckedUser.Usertype == "Lære" && AlleLokalers[SelectedItem].Type == "Klasselokale")
        //    //{
        //    //    fick = 2;
        //    //}
        //    //else if (RefLokaleBookinger.LokaleId == AlleLokalers[SelectedItem].LokaleId && RefBookinger.Date == Dato &&
        //    //         RefUser.CheckedUser.Usertype == "Lære" && AlleLokalers[SelectedItem].Type == "Møderum")
        //    //{
        //    //    fick = 2;
        //    //}

        //    //return fick = 0;


        //    //if (AlleLokaleBookingers.Find(_lokaleBookingerPredicate) != null && LokaleIdFraViewModel == LokaleId)
        //    //{
        //    //    _color = new SolidColorBrush(Colors.GreenYellow); OnPropertyChanged("Color");
        //    //}
        //    //else
        //    //{
        //    //    //_color = Colors.Blue; OnPropertyChanged("Color");
        //    //    _color = new SolidColorBrush(Colors.Blue); OnPropertyChanged("Color");
        //    //}


        //}

        public static List<Bookinger> CheckedBookinger = new List<Bookinger>();

        public static int SelectedItem { get { return RefUserViewModel.SelectedRoom; } }
        public static UserViewModel RefUserViewModel { get; set; }
        public int LokaleIdFraViewModel { get; set; }

        //private readonly Predicate<List<LokaleBookinger>> _bookingerPredicate = new Predicate<List<LokaleBookinger>>(LedighedsSortCheckBookinger);


        public static List<Bookinger> LedighedsSortCheckBookinger(List<LokaleBookinger> lokaleBookinger)
        {
            for (int i = 0; i < AlleBookingers.Count; i++)
            {
                foreach (var a in MineBookingers)
                {
                    if (a.BookingId == lokaleBookinger[i].BookingId && a.Date == Dato)
                    {
                        CheckedBookinger.Add(a);

                    }
                }
            }
            return CheckedBookinger;
        }



        public override string ToString()
        {
            return $"Navn: {Navn}, Bygning: {Bygning}, Etage: {Etage}, Type: {Type}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
