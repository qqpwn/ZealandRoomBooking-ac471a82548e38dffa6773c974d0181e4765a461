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
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<User> Get()
        {
            return new ManageUser().GetAllUser();
        }

        // GET: api/User/5
        public User Get(int id)
        {
            return new ManageUser().GetUserFromId(id);
        }

        // POST: api/User
        public void Post([FromBody]User value)
        {
            new ManageUser().CreateUser(value);
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]User value)
        {
            new ManageUser().UpdateUser(value, id);
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
            new ManageUser().DeleteUser(id);
        }
    }
}
