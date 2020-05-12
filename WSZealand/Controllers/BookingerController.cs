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
    public class BookingerController : ApiController
    {
        // GET: api/Bookinger
        public IEnumerable<Bookinger> Get()
        {
            return new ManageBookinger().GetAllBookinger();
        }

        // GET: api/Bookinger/5
        public Bookinger Get(int id)
        {
            return new ManageBookinger().GetBookingerFromId(id);
        }

        // POST: api/Bookinger
        public void Post([FromBody]Bookinger value)
        {
            new ManageBookinger().CreateBookinger(value);
        }

        // PUT: api/Bookinger/5
        public void Put(int id, [FromBody]Bookinger value)
        {
            new ManageBookinger().UpdateBookinger(value, id);
        }

        // DELETE: api/Bookinger/5
        public void Delete(int id)
        {
            new ManageBookinger().DeleteBookinger(id);
        }
    }
}
