using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.Model
{
    public class Tid
    {
        public int TidId { get; set; }
        public TimeSpan TidFra { get; set; }
        public TimeSpan TidTil { get; set; }

        public Tid()
        {

        }

    }
}
