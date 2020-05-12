using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModelKlasser;
using DBContext;

namespace WSZealand.Controllers
{
    public class BookingSmartboardController : ApiController
    {
        // GET: api/BookingSmartboard
        public IEnumerable<BookingSmartboard> Get()
        {
            return new ManageBookingSmartboard().GetAllBookingSmartboard();
        }

        // GET: api/BookingSmartboard/5
        public BookingSmartboard Get(int id)
        {
            return new ManageBookingSmartboard().GetBookingSmartboardFromId(id);
        }

        // POST: api/BookingSmartboard
        public void Post([FromBody]BookingSmartboard value)
        {
            new ManageBookingSmartboard().CreateBookingSmartboard(value);
        }

        // PUT: api/BookingSmartboard/5
        public void Put(int id, [FromBody]BookingSmartboard value)
        {
            new ManageBookingSmartboard().UpdateBookingSmartboard(value,id);
        }

        // DELETE: api/BookingSmartboard/5
        public void Delete(int id)
        {
            new ManageBookingSmartboard().DeleteBookingSmartboard(id);
        }
    }
}
