using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public class ControllerBase : Controller
    {
        public string UserId => (base.User.Identity as ClaimsIdentity).Claims.SingleOrDefault(x=>x.Type == "UserId").Value;
    }
}
