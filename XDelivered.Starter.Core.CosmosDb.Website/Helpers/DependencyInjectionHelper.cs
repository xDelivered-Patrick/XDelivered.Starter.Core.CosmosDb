using System;
using Microsoft.Extensions.DependencyInjection;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static IServiceScope ServiceScope { get; set; }
        public static IServiceProvider ApplicationServices { get; set; }
    }
}