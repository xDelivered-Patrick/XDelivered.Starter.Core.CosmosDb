using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public static class EntityFrameworkConnectionHelper
    {
        /// <summary>
        /// Tells the server to use an in-memory database. This is useful for integration testing.
        /// </summary>
        public static bool UseInMemory { get; set; } = false;

        public static bool UseRealServerConnection => !UseInMemory;
    }
    public static class ServerHelper
    {
        /// <summary>
        /// Tells the server to use an in-memory database. This is useful for integration testing.
        /// </summary>
        public static bool IntegrationTests { get; set; } = false;
    }
}
