using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBContext;
using ModelKlasser;

namespace WSZealand.Controllers
{
    public class LokalerController : ApiController
    {
        // GET: api/Lokaler
        public IEnumerable<Lokaler> Get()
        {
            return new ManageLokaler().GetAllLokaler();
        }

        // GET: api/Lokaler/5
        public Lokaler Get(int id)
        {
            return new ManageLokaler().GetLokalerFromId(id);
        }

        // POST: api/Lokaler
        public void Post([FromBody]Lokaler value)
        {
            new ManageLokaler().CreateLokaler(value);
        }

        // PUT: api/Lokaler/5
        public void Put(int id, [FromBody]Lokaler value)
        {
            new ManageLokaler().UpdateLokaler(value, id);
        }

        // DELETE: api/Lokaler/5
        public void Delete(int id)
        {
            new ManageLokaler().DeleteLokaler(id);
        }
    }
}
