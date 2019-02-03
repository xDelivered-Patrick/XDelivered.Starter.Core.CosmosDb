using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Controllers
{

    [Route("api/entities")]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class EntitiesController : Controller
    {
        [HttpGet("")]
        [Produces(typeof(OperationResult<List<EntityModel>>))]
        [SwaggerOperation(nameof(AllEntities))]
        public List<EntityModel> AllEntities()
        {
            return new List<EntityModel>()
            {
                new EntityModel()
                {
                    Id = 1.ToString(),
                    Name = "name1"
                },
                new EntityModel()
                {
                    Id = 2.ToString(),
                    Name = "name2"
                }
            };
        }
    }
}