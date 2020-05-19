using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Eventmaker.Common;
using ZealandRoomBooking.Model;

namespace ZealandRoomBooking.ViewModel
{
    public class BookingerViewModel
    {
        public int BookingId { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }


        //public void DeleteBooking()
        //{
        //    User user = new User();

        //    Bookinger bookinger = new Bookinger()
        //    {
        //        UserId = user.UserId
        //    };

        //    Persistency.PersistencyService<Bookinger>.DeleteObject(objectId:,objstring:);

        //    LokaleBookinger lokaleBookinger = new LokaleBookinger()
        //    {
        //        BookingId = bookinger.BookingId,
        //        LokaleId = 1
        //    };

        //    Persistency.PersistencyService<LokaleBookinger>.DeleteObject(bookinger.BookingId,lokaleBookinger.LokaleId);
        //}


        //private ICommand _deleteBookingCommand;
        //private ICommand _selectBookingCommand;
        //private ICommand _createBookingCommand;

        //public ICommand DeleteBookingCommand
        //{
        //    get { return _deleteBookingCommand ?? (_deleteBookingCommand = new RelayCommand(Persistency.PersistencyService<Bookinger>.DeleteObject())); }
            
        //    set { _deleteBookingCommand = value; }
        //}

        //public ICommand SelectBookingCommand
        //{
        //    get { return _selectBookingCommand = new RelayCommand()}
        //    set { _selectBookingCommand = value; }
        //}
    }
}
