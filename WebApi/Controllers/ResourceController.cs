using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize]
    public class ResourceController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = new String[] { "First", "Second" };

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet()]
        public IHttpActionResult GetRessourceById(int id)
        {
            var obj = new { FistName = "khaled", LastName = "oueslati" };
            return Ok(obj);

        }
    }
}
