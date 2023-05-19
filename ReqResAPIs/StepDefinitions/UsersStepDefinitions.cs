using Microsoft.Extensions.Configuration;
using System.Text.Json;
using ReqResAPIs.Models;
using ReqResAPIs.Support;
using RestSharp;
using System.Net;
using NUnit.Framework;
using FluentAssertions.Execution;

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

        [When(@"I send a GET request to '([^']*)' with invalid page as (.*)")]
        public void WhenISendAGETRequestToWithInvalidPageAs(string listusers, int expected)
        {
            restRequest = new RestRequest(listusers+$"?page={expected}", Method.Get);
            restResponse = restClient.Execute(restRequest);
            _scenarioContext.Add("response", restResponse);
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
            TestContext.WriteLine(usersResponse.Data);
            using (new AssertionScope()) 
            {
                usersResponse.Data.Should().NotBeNull();
                usersResponse.Data.Select(user => user.Id.Should().BeGreaterThan(0));
                usersResponse.Data.Select(user => user.FirstName.Should().NotBeNullOrEmpty());
            }
        }
           
        [Then(@"the response body should contain a filtered list of users with the specified parameters")]
        public void ThenTheResponseBodyShouldContainAFilteredListOfUsersWithTheSpecifiedParameters()
        {
            var response = _scenarioContext.Get<RestResponse>("response");
            var usersResponse = JsonSerializer.Deserialize<UserData>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                usersResponse.Data.Count.Should().Be(0);
        }

    }
}
