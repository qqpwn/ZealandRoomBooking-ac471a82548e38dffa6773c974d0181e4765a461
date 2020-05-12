using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
