using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.Model
{
    class SimpletonLokaler
    {
        private static SimpletonLokaler instance = null;
        public static ObservableCollection<Lokaler> _lokaleCollection;
        public SimpletonLokaler()
        {
           HentCollection();
        }

        public ObservableCollection<Lokaler> MineLokaler { get { return _lokaleCollection; } }
        public static SimpletonLokaler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SimpletonLokaler();
                }

                return instance;
            }

        }

        public async void HentCollection()
        {
            await PersistencyService<Lokaler>.GetObjects("Lokaler");
            _lokaleCollection = PersistencyService<Lokaler>.HentCollection;
        }
    }
}
