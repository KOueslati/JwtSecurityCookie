using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok();
        }
    }
}
