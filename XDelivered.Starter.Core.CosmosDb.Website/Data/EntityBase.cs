using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    public abstract class EntityBase
    {
        [Key]
        public int Key { get; set; }
        public DateTime Created { get; set; }
    }
}
