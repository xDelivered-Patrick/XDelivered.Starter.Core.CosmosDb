using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Tests.Integration.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;
using Xunit;

namespace Tests.Integration.Tests
{
    public class AccountTests : IntegrationTestBase, IClassFixture<IntegrationTestBase>
    {
        private string _validPassword = "Fdsklgfd3432@@";
        private string _validName = "patrick";
        private string _validEmail = "patrick@xdelivered.com";

        [Fact]
        public async Task WhenUserRegisters_Success_UserAddedToIdentityDb()
        {
            var result = await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });
            await base.Assert(result);
        }


        [Fact]
        public async Task WhenUserRegisters_EasyPassword_Fail()
        {
            var result = await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = "pass"
            });
            result.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task WhenUserRegisters_NonEmail_Fail()
        {
            var result = await RegisterTyped(new RegisterRequestModel()
            {
                Name = _validName,
                Email = "failingemail",
                Password = _validPassword
            });
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("is invalid");
        }

        [Fact]
        public async Task WhenUserRegisterTwice_Fail()
        {
            var result = await RegisterTyped(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });
            result.IsSuccess.Should().BeTrue();


            result = await RegisterTyped(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("is already taken");
        }

        [Fact]
        public async Task WhenUserRegisters_CanLogin_Success()
        {
            await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });

            HttpResponseMessage loginResponse = await Login(new LoginRequestModel()
            {
                Email = _validEmail,
                Password = _validPassword
            });
            loginResponse.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task WhenUserRegisters_LoginSuccess_GetsJwtToken()
        {
            await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });

            var loginResponse = await Login<LoginResponse>(new LoginRequestModel()
            {
                Email = _validEmail,
                Password = _validPassword
            });
            loginResponse.Data.Token.Should().NotBeNullOrEmpty();
            loginResponse.Data.Expiration.Should().BeAfter(DateTime.UtcNow.AddDays(7));
        }

        [Fact]
        public async Task WhenUserRegisters_BadLogin_Fails()
        {
            await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });

            var loginResponse = await Login(new LoginRequestModel()
            {
                Email = _validEmail,
                Password = "invalidpassword"
            });

            loginResponse.IsSuccessStatusCode.Should().Be(false);
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        [Fact]
        public async Task WhenUserHasLoggedIn_CanRetrieveUserInfo()
        {
            await Register(new RegisterRequestModel()
            {
                Name = _validName,
                Email = _validEmail,
                Password = _validPassword
            });

            var loginResponse = await Login<LoginResponse>(new LoginRequestModel()
            {
                Email = _validEmail,
                Password = _validPassword
            });

            var token = loginResponse.Data.Token;

            OperationResult<UserInfoResponseModel> userInfo = await GetUserInfo<UserInfoResponseModel>(token);
            userInfo.IsSuccess.Should().Be(true);
            userInfo.Data.Email.Should().Be(_validEmail);
        }


        [Fact]
        public async Task CanRetrieveUserInfo_BadAuthToken_Fail()
        {
            OperationResult<UserInfoResponseModel> userInfo = await GetUserInfo<UserInfoResponseModel>("invalidToken");
            userInfo.Should().BeNull();
        }

        private Task<HttpResponseMessage> Register(RegisterRequestModel requestModel)
        {
            return base._client.PostAsJsonAsync("/api/account/register", requestModel);
        }
        private Task<HttpResponseMessage> Login(LoginRequestModel requestModel)
        {
            return base._client.PostAsJsonAsync("/api/account/login", requestModel);
        }
        private async Task<OperationResult> RegisterTyped(RegisterRequestModel requestModel)
        {
            var response =  await base._client.PostAsJsonAsync("/api/account/register", requestModel);
            return await response.Content.ReadAsAsync<OperationResult>();
        }

        private async Task<OperationResult<T>> Login<T>(LoginRequestModel requestModel, bool assertResult = true)
        {
            var result = await base._client.PostAsJsonAsync("/api/account/login", requestModel);
            if (assertResult) await base.Assert(result);
            return await result.Content.ReadAsAsync<OperationResult<T>>();
        }
        private async Task<OperationResult<T>> GetUserInfo<T>(string token)
        {
            base._client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
            var result = await base._client.GetAsync("/api/account/info");
            return await result.Content.ReadAsAsync<OperationResult<T>>();
        }
    }
}
