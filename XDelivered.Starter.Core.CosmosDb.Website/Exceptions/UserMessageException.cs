using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Exceptions
{
    public class UserMessageException : Exception
    {
        public UserMessageException(string message) : base(message)
        {
            
        }
    }
}
