using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Persistency;
using ZealandRoomBooking.ViewModel;

namespace ZealandRoomBooking.Model
{
    public class Lokaler
    {
        public int LokaleId { get; set; }
        public int Etage { get; set; }
        public string Type { get; set; }
        public string Navn { get; set; }
        public string Bygning { get; set; }

        public Lokaler(int lokaleId, int etage, string type, string navn, string bygning)
        {
            LokaleId = lokaleId;
            Etage = etage;
            Type = type;
            Navn = navn;
            Bygning = bygning;
        }

        

        

        private static readonly List<Lokaler> AlleLokalers = new List<Lokaler>();
        private static readonly List<LokaleBookinger> AlleLokaleBookingers = new List<LokaleBookinger>();

        public static List<Lokaler> MineLokalers
        {
            get { return AlleLokalers; }
        }
        public static List<LokaleBookinger> MineLokalerBookingers
        {
            get { return AlleLokaleBookingers; }
        }

        public async void LedighedsCheck()
        {
            await PersistencyService<Lokaler>.GetObjects("Lokaler");
            await PersistencyService<LokaleBookinger>.GetObjects("LokaleBookinger");
            ObservableCollection<Lokaler> test = PersistencyService<Lokaler>.HentCollection;
            foreach (var o in test)
            {
                AlleLokalers.Add(o);
            }

            ObservableCollection<LokaleBookinger> test1 = PersistencyService<LokaleBookinger>.HentCollection;
            foreach (var o in test1)
            {
                AlleLokaleBookingers.Add(o);
            }

            if (AlleLokaleBookingers.Find(_lokaleBookingerPredicate) != null && LokaleIdFraViewModel == LokaleId)
            {
                Color.FromArgb(0,255,0,0);
            }
            else
            {
                Color.FromArgb(0,0,255,0);
            }


        }

        public static int SelectedItem { get { return RefUserViewModel.SelectedRoom; } }
        public static UserViewModel RefUserViewModel { get; set; }
        public static int LokaleIdFraViewModel { get { return RefUserViewModel.LokaleId; } }

        private readonly Predicate<LokaleBookinger> _lokaleBookingerPredicate = new Predicate<LokaleBookinger>(LedighedsSortCheck);

        public static bool LedighedsSortCheck(LokaleBookinger lokaleBookinger)
        {
            foreach (var a in MineLokalers)
            {
                if (lokaleBookinger.LokaleId == a.LokaleId)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Navn: {Navn}, Bygning: {Bygning}, Etage: {Etage}, Type: {Type}";
        }
    }
}
