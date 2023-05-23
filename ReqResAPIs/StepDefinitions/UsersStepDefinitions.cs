using Microsoft.Extensions.Configuration;
using System.Text.Json;
using ReqResAPIs.Models;
using ReqResAPIs.Support;
using RestSharp;
using System.Net;
using NUnit.Framework;
using FluentAssertions.Execution;
using static ReqResAPIs.Support.Constants;

namespace ReqResAPIs.StepDefinitions
{
    [Binding]
    public sealed class UsersStepDefinitions : BaseAPITest
    {
        private RestRequest restRequest;
        private RestResponse restResponse;
        private RestClient restClient;
        private readonly ScenarioContext _scenarioContext;

        public UsersStepDefinitions(ScenarioContext _context)
        {
            _scenarioContext = _context;
            restClient = new RestClient(BaseURL);
        }

        [When(@"I send a GET request to '([^']*)'")]
        public void WhenISendAGETRequestTo(string listusers)
        {
            restRequest = new RestRequest(listusers, Method.Get);
            restResponse = restClient.Execute(restRequest);
        }

        [When(@"I send a GET request to '([^']*)' with id as (.*)")]
        public void WhenISendAGETRequestToWithIdAs(string listusers, string expected)
        {
            restRequest = new RestRequest(listusers+$"/{expected}", Method.Get);
            restResponse = restClient.Execute(restRequest);
            _scenarioContext.Add("expectedUser", expected);
        }

        [Then(@"the response should have a status code of (.*)")]
        public void ThenTheResponseShouldHaveAStatusCodeOf(int statusCode)
        {
            restResponse.StatusCode.Should().Be((HttpStatusCode?)statusCode);
        }

        [Then(@"the response body should contain a list of users")]
        public void ThenTheResponseBodyShouldContainAListOfUsers()
        {
            var usersResponse = JsonSerializer.Deserialize<UserData>(restResponse.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TestContext.WriteLine(restResponse.Content);
            using (new AssertionScope()) 
            {
                usersResponse.Data.Should().NotBeNull();
                usersResponse.Data.Select(user => user.Id.Should().NotBeNull());
                usersResponse.Data.Select(user => user.FirstName.Should().NotBeNullOrEmpty());
            }
        }

        [Then(@"the response data should be empty")]
        public void ThenTheResponseDataShouldBeEmpty()
        {
            var usersResponse = JsonSerializer.Deserialize<User>(restResponse.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            usersResponse.Should().NotBeNull();
        }

        [Then(@"the response data should be based on the filter criteria applied")]
        public void ThenTheResponseDataShouldBeBasedOnTheFilterCriteriaApplied()
        {
            restResponse.Content.Should().NotBeNullOrEmpty();
            var userId = _scenarioContext.Get<string>("expectedUser");
            UserData? usersResponse = JsonSerializer.Deserialize<UserData>(restResponse.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            usersResponse.Data.ForEach(id => id.Should().Be(userId));
        }

    }
}
