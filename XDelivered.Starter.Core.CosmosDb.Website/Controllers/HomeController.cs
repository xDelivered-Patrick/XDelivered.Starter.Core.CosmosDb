using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public ActionResult Ok()
        {
            return Ok("starterkits api");
        }
    }
}