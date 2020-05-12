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
    public class TidController : ApiController
    {
        // GET: api/Tid
        public IEnumerable<Tid> Get()
        {
            return new ManageTid().GetAllTid();
        }

        // GET: api/Tid/5
        public Tid Get(int id)
        {
            return new ManageTid().GetTidFromId(id);
        }

        // POST: api/Tid
        public void Post([FromBody]Tid value)
        {
            new ManageTid().CreateTid(value);
        }

        // PUT: api/Tid/5
        public void Put(int id, [FromBody]Tid value)
        {
            new ManageTid().UpdateTid(value, id);
        }

        // DELETE: api/Tid/5
        public void Delete(int id)
        {
            new ManageTid().DeleteTid(id);
        }
    }
}
