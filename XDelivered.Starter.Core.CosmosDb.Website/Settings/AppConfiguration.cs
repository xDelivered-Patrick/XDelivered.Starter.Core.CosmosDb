using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Settings
{

    public class AppConfiguration
    {
        /// <summary>
        ///     Signing key used for JWT Token issue
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        ///     Expected URL of the site
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// Number of days until Jwt Tokens expire
        /// </summary>
        public int TokenExpiryDays { get; set; }
    }
}
