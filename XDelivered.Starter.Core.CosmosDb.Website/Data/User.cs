using System;
using AspNetCore.Identity.MongoDB;
using AspNetCore.Identity.MongoDB.Models;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    public class User : MongoIdentityUser
    {
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Type { get; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public User(string userName, string email) : base(userName, email)
        {
        }

        public User(string userName, MongoUserEmail email) : base(userName, email)
        {
        }

        public User(string userName) : base(userName)
        {
        }
    }
}
