using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealandRoomBooking.Model
{
    public class LokaleBookinger
    {
        public int LBId { get; set; }
        public int BookingId { get; set; }
        public int LokaleId { get; set; }

        public LokaleBookinger(int bookingId, int lokaleId)
        {
            BookingId = bookingId;
            LokaleId = lokaleId;
        }

        public LokaleBookinger()
        {
            
        }
    }
}
