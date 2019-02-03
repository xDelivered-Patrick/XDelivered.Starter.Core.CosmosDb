using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Tests.Integration.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;
using Xunit;

namespace Tests.Integration.Tests
{
    public class HomeTests : IntegrationTestBase
    {
        [Fact]
        public async Task WhenUserRegisters_Success_UserAddedToIdentityDb()
        {
            var result = await base._client.GetAsync("");
            await base.Assert(result);
        }
    }
}
