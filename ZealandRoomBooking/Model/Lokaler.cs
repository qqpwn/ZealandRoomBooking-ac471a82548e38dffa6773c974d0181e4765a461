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
        private SolidColorBrush _color;
        public int BookingStatus { get; set; } = 0;
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }

        public SolidColorBrush Color
        {
            get
            {
                AddtilList();
                return Test();
            }
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




        private static readonly ObservableCollection<LokaleBookinger> _alleLokaleBookingers = new ObservableCollection<LokaleBookinger>();
        private static readonly ObservableCollection<Bookinger> _alleBookingers = new ObservableCollection<Bookinger>();
        private static readonly ObservableCollection<Lokaler> _alleLokaler = new ObservableCollection<Lokaler>();


        public ObservableCollection<LokaleBookinger> MineLokalerBookingers
        {
            get { return _alleLokaleBookingers; }
        }

        public ObservableCollection<Bookinger> MineBookingers
        {
            get { return _alleBookingers; }

        }
        public ObservableCollection<Lokaler> MineLokaler
        {
            get { return _alleLokaler; }
        }

        public void AddtilList()
        {

            ObservableCollection<LokaleBookinger> test1 = UserViewModel.ListOfLokaleBookinger;
            ObservableCollection<Bookinger> test2 = UserViewModel.ListOfBookinger;
            ObservableCollection<Lokaler> test3 = UserViewModel.ListOfRooms;
            _alleLokaleBookingers.Clear();
            foreach (var o in test1)
            {
                _alleLokaleBookingers.Add(o);
            }
            _alleBookingers.Clear();

            foreach (var o in test2)
            {
                _alleBookingers.Add(o);
            }

            _alleLokaler.Clear();
            foreach (var o in test3)
            {
                _alleLokaler.Add(o);
            }
        }


        public int fick = 0;

        public SolidColorBrush Test()
        {

            RefBookinger = new Bookinger();
            RefLokaleBookinger = new LokaleBookinger();
            RefUser = new User();

            LedighedsSortCheckBookinger();
            foreach (var a in _alleLokaler)
            {
                foreach (var b in CheckedBookinger)
                {
                    if (a.LokaleId == LokaleId && a.Type == "Klasselokale" && RefUser.CheckedUser.Usertype == "Elev" && a.BookingStatus <= 1 && b.Date == Dato)
                    {
                        a._color = new SolidColorBrush(Colors.GreenYellow);
                        _color = a._color;
                        OnPropertyChanged(nameof(Color));
                        return _color;

                    }
                    else if (a.LokaleId == LokaleId && a.Type == "Klasselokale" && RefUser.CheckedUser.Usertype == "Elev" && a.BookingStatus > 1 && b.Date == Dato)
                    {
                        a._color = new SolidColorBrush(Colors.Red);
                        _color = a._color;
                        OnPropertyChanged(nameof(Color));
                        return _color;

                    }
                    else if (a.LokaleId == LokaleId && a.Type == "Møderum" && RefUser.CheckedUser.Usertype == "Elev")
                    {

                        if (a.BookingStatus >= 0 && b.Date == Dato)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(Color));
                            return _color;
                        }

                    }
                }
            }
            return _color;
        }

        public static DateTime Dato = DateTime.Now;
        public LokaleBookinger RefLokaleBookinger { get; set; }
        public static Bookinger RefBookinger { get; set; }
        public User RefUser { get; set; }
        public UserViewModel RefUserViewModel { get; set; }

        public static List<Bookinger> CheckedBookinger = new List<Bookinger>();

        public List<Bookinger> LedighedsSortCheckBookinger()
        {
            CheckedBookinger.Clear();
            if (_alleBookingers.Count != 0)
            {
                for (int i = 0; i < _alleBookingers.Count; i++)
                {
                    foreach (var a in MineBookingers)
                    {
                        if (a.Date.Date == Dato.Date)
                        {
                            CheckedBookinger.Add(a);
                        }
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
