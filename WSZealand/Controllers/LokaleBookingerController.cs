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
    public class LokaleBookingerController : ApiController
    {
        // GET: api/LokaleBookinger
        public IEnumerable<LokaleBookinger> Get()
        {
            return new ManageLokaleBookinger().GetAllLokaleBookinger();
        }

        // GET: api/LokaleBookinger/5
        public LokaleBookinger Get(int id)
        {
            return new ManageLokaleBookinger().GetLokaleBookingerFromId(id);
        }

        // POST: api/LokaleBookinger
        public void Post([FromBody]LokaleBookinger value)
        {
            new ManageLokaleBookinger().CreateLokaleBookinger(value);
        }

        // PUT: api/LokaleBookinger/5
        public void Put(int id, [FromBody]LokaleBookinger value)
        {
            new ManageLokaleBookinger().UpdateLokaleBookinger(value, id);
        }

        // DELETE: api/LokaleBookinger/5
        public void Delete(int id)
        {
            new ManageLokaleBookinger().DeleteLokaleBookinger(id);
        }
    }
}
