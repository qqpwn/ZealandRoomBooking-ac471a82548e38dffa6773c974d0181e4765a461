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
        private SolidColorBrush _color = new SolidColorBrush(Colors.Green);
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
                //Methode som sætter ListViewItem Background color
                return ColorCode(this);
            }
            set { _color = value; OnPropertyChanged(nameof(_color)); }
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

        //Henter Listerne fra UserViewModel, så vi sikre på det de samme lister
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

        //Methode som sætter ListViewItem Background color
        public SolidColorBrush ColorCode(Lokaler a)
        {
            foreach (var b in _alleLokaler)
            {
                if (a.LokaleId == b.LokaleId)
                {
                    if (RefUser.CheckedUser.Usertype == "Elev")
                    {

                        if (a.Type == "Klasselokale" && b.BookingStatus <= 1)
                        {
                            a._color = new SolidColorBrush(Colors.Green);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;

                        }
                        else if (a.Type == "Klasselokale" && b.BookingStatus > 1)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;

                        }
                        else if (a.Type == "Møderum" && b.BookingStatus > 0)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                    }
                    else if (RefUser.CheckedUser.Usertype == "Lære")
                    {
                        if (a.Type == "Klasselokale" && b.BookingStatus >= 1 && Dato.Date < DatoDageFrem.AddDays(3).Date)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                        else if (a.Type == "Klasselokale" && b.BookingStatus >= 1 && b.BookingStatus < 3 && DateTime.Today.Date <= DatoDageFrem.AddDays(3))
                        {
                            a._color = new SolidColorBrush(Colors.Yellow);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                        else if (a.Type == "Klasselokale" && b.BookingStatus > 2)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                        else if (a.Type == "Møderum" && b.BookingStatus > 0 && Dato.Date < DatoDageFrem.AddDays(3).Date)
                        {
                            a._color = new SolidColorBrush(Colors.Red);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                        else if (a.Type == "Møderum" && b.BookingStatus >= 1 && b.BookingStatus < 3 && DateTime.Today.Date <= DatoDageFrem.AddDays(3))
                        {
                            a._color = new SolidColorBrush(Colors.Yellow);
                            _color = a._color;
                            OnPropertyChanged(nameof(_color));
                            return _color;
                        }
                       
                    }
                }
            }

            return _color;
        }


        public DateTime Dato = UserViewModel.BookingDate;
        public DateTime DatoDageFrem = DateTime.Today;
        public User RefUser { get; set; }


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
