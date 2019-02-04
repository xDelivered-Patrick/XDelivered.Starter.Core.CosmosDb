using System;
using System.ComponentModel.DataAnnotations;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    public abstract class EntityBase
    {
        [Key]
        public int Key { get; set; }
        public DateTime Created { get; set; }
    }
}
