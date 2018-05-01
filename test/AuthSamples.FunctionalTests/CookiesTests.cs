// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AuthSamples.FunctionalTests
{
    public class CookiesTests : IClassFixture<WebApplicationFactory<Cookies.Startup>>
    {
        public CookiesTests(WebApplicationFactory<Cookies.Startup> fixture)
            => Client = fixture.CreateClient();

        public HttpClient Client { get; }

        [Fact]
        public async Task DefaultReturns200()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MyClaimsRedirectsToLoginPageWhenNotLoggedIn()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/Home/MyClaims");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Log in</button>", content);
        }

        [Fact]
        public async Task MyClaimsShowsClaimsWhenLoggedIn()
        {
            // Arrange & Act
            var signIn = await SignIn(Client, "Dude");
            Assert.Equal(HttpStatusCode.OK, signIn.StatusCode);

            var response = await Client.GetAsync("/Home/MyClaims");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("<h2>HttpContext.User.Claims</h2>", content);
            Assert.Contains("<dd>Dude</dd>", content); // Ensure user name shows up as a claim
        }

        [Fact]
        public async Task LogoutClearsCookie()
        {
            // Arrange & Act
            var signIn = await SignIn(Client, "Dude");
            Assert.Equal(HttpStatusCode.OK, signIn.StatusCode);

            var response = await Client.GetAsync("/Home/MyClaims");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("<h2>HttpContext.User.Claims</h2>", content);
            Assert.Contains("<dd>Dude</dd>", content); // Ensure user name shows up as a claim

            response = await Client.GetAsync("/Account/Logout");
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await Client.GetAsync("/Home/MyClaims");
            content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Log in</button>", content);
        }

        internal static async Task<HttpResponseMessage> SignIn(HttpClient client, string userName)
        {
            var goToSignIn = await client.GetAsync("/account/login");
            var signIn = await TestAssert.IsHtmlDocumentAsync(goToSignIn);

            var form = TestAssert.HasForm(signIn);
            return await client.SendAsync(form, new Dictionary<string, string>()
            {
                ["username"] = userName,
                ["password"] = userName // this test doesn't care what the password is
            });
        }

    }
}
