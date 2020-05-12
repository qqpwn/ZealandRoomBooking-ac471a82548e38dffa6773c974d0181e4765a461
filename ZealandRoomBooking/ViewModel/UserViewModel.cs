using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Model;

namespace ZealandRoomBooking.ViewModel
{
    public class UserViewModel
    {

        public void BookLokale()
        {
            User user = new User();

            Bookinger bookinger = new Bookinger()
            {
                UserId = user.UserId
            };

            Persistency.PersistencyService.PostObject(bookinger);

            LokaleBookinger lokaleBookinger = new LokaleBookinger()
            {
                BookingId = bookinger.BookingId,
                LokaleId = 1
            };

            Persistency.PersistencyService.PostObject(lokaleBookinger);
        }
    }
}
