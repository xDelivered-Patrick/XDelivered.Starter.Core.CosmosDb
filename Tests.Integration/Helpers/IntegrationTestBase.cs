using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Mongo2Go;
using XDelivered.StarterKits.NgCoreCosmosDb;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace Tests.Integration.Helpers
{

    public class IntegrationTestBase
    {
        protected TestServer _server;
        protected HttpClient _client;
        private MongoDbRunner _runner;

        public IntegrationTestBase()
        {
            ServerHelper.IntegrationTests = true;

            StartMongoInstance();

            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();

            var baseUri = _server.BaseAddress.ToString();
            baseUri = baseUri.Remove(baseUri.Length - 1);
            _client.BaseAddress = new Uri(baseUri);
        }

        private void StartMongoInstance()
        {
            _runner = MongoDbRunner.Start();
            ServerHelper.IntegrationTestConnectionString = _runner.ConnectionString;
        }

        protected async Task Assert(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                var because = await result.Content.ReadAsStringAsync();
                var reason = $"({result.StatusCode}) : {because}";
                result.IsSuccessStatusCode.Should().BeTrue(reason);
            }


        }

        protected async Task LoginStandardUser()
        {
            var result = await _client.PostAsJsonAsync("/api/account/login", new LoginRequestModel()
            {
                Email = "standard@xdelivered.com",
                Password = "xdelivered99"
            });
            OperationResult<LoginResponse> response =  await result.Content.ReadAsAsync<OperationResult<LoginResponse>>();

            AddBearer(response);
        }

        protected void RegisterAndLogin()
        {
            LoginStandardUser().Wait();
        }
        protected T Resolve<T>()
        {
            return (T)DependencyInjectionHelper.ApplicationServices.GetService(typeof(T));
        }

        protected void LoginOwner()
        {
            LoginOwnerUser().Wait();
        }

        private async Task LoginOwnerUser()
        {
            var result = await _client.PostAsJsonAsync("/api/account/login", new LoginRequestModel()
            {
                Email = "owner@xdelivered.com",
                Password = "xdelivered99"
            });
            OperationResult<LoginResponse> response = await result.Content.ReadAsAsync<OperationResult<LoginResponse>>();

            AddBearer(response);
        }

        private void AddBearer(OperationResult<LoginResponse> response)
        {
            if (response.IsSuccess)
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Data.Token}");
            }
        }

        protected void LoginAdmin()
        {
            LoginAdminUser().Wait();
        }

        private async Task LoginAdminUser()
        {
            var result = await _client.PostAsJsonAsync("/api/account/login", new LoginRequestModel()
            {
                Email = "admin@xdelivered.com",
                Password = "xdelivered99"
            });
            OperationResult<LoginResponse> response = await result.Content.ReadAsAsync<OperationResult<LoginResponse>>();

            AddBearer(response);
        }

    }
}
