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

        public LokaleBookinger()
        {
            LBId = 1;
            //LBId++;
        }
    }
}
