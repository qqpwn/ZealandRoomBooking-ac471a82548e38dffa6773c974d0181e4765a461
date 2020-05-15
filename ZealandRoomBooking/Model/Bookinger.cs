using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealandRoomBooking.Model
{
    public class Bookinger
    {
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public Bookinger()
        {
            //BookingId = 1;
            //BookingId++;
            //Date = DateTime.Now;
        }
    }
}
