using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<string>
    {
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Type { get; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public User()
        {
            
        }
        public User(string userName, string email) : base(userName, email)
        {
        }
        
        public User(string userName) : base(userName)
        {
        }
    }


    public class ApplicationRole : MongoIdentityRole<string>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
