using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.Model
{
    public class Bookinger
    {
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string DateString { get; set; }

        public Bookinger(DateTime date, int userId)
        {
            Date = date;
            UserId = userId;
            DateStringMethod();
        }

        public void DateStringMethod()
        {
            DateString = $"{Date.Year}-{Date.Month}-{Date.Day}";

        }
    }
}
