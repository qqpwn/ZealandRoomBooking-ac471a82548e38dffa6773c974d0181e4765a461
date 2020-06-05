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
        //Default color for ListViewItem
        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set { _color = value; OnPropertyChanged(nameof(Color)); }
        }
        public int BookingStatus { get; set; } = 0;
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }

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

        public UserViewModel RefUserViewModel { get; set; }
        public void UpdateColorDageFrem()
        {
            RefUserViewModel.DayForward();
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

        //Henter Listerne fra UserViewModel, så vi sikre på det de samme lister
        public void AddtilList()
        {
            ObservableCollection<LokaleBookinger> test1 = HomeViewModel.LokaleBookingerCollection;
            ObservableCollection<Bookinger> test2 = HomeViewModel.BookingerCollection;
            ObservableCollection<Lokaler> test3 = HomeViewModel.LokaleCollection;
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

        //Bruges til at sætte colors på lokaler
        public void ColorCodes(Lokaler room)
        {
            if (RefUser.CheckedUser.Usertype == "Elev")
            {
                if (room.Type == "Klasselokale" && room.BookingStatus <= 1)
                {
                    room.Color = new SolidColorBrush(Colors.Green);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }
                else if (room.Type == "Klasselokale" && room.BookingStatus > 1)
                {
                    room.Color = new SolidColorBrush(Colors.Red);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }
                else if (room.Type == "Møderum" && room.BookingStatus == 0)
                {
                    room.Color = new SolidColorBrush(Colors.Green);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }
                else if (room.Type == "Møderum" && room.BookingStatus > 0)
                {
                    room.Color = new SolidColorBrush(Colors.Red);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }
            }
            else if (RefUser.CheckedUser.Usertype == "Lære")
            {
                if (room.BookingStatus == 0)
                {
                    room.Color = new SolidColorBrush(Colors.Green);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }

                else if (UserViewModel.BookingDate.Date >= DateTime.Now.AddDays(3).Date && room.BookingStatus < 3)
                {
                    room.Color = new SolidColorBrush(Colors.Yellow);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }

                else
                {
                    room.Color = new SolidColorBrush(Colors.Red);
                    Color = room.Color;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public User RefUser = new User();


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
