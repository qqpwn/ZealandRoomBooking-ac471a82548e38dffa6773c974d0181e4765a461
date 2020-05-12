using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealandRoomBooking.Model
{
    public class BookingSmartboard
    {
        public int BSId { get; set; }
        public int BookingId { get; set; }
        public int TidId { get; set; }
        public int LokaleId { get; set; }

        public BookingSmartboard()
        {
            BSId = 1;
            BSId++;
        }
    }
}
