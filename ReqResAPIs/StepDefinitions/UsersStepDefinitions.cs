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
        private readonly ScenarioContext _scenarioContext;
        private RestResponse _response;
        private readonly APIHelper<UserData> _helper;

        public UsersStepDefinitions(ScenarioContext context)
        {
            _scenarioContext = context;
            _helper = new APIHelper<UserData>($"{BaseURL}/users");
        }

        [When(@"I send a GET request to users")]
        public void WhenISendAGETRequestToUsers()
        {
            _response = _helper.GetMethod(String.Empty);
        }

        [When(@"I send a GET request to users with id as (.*)")]
        public void WhenISendAGETRequestToWithIdAs(string id)
        {
            _response = _helper.GetMethod($"{id}");
            _scenarioContext.Add("expectedUser", id);
        }

        [Then(@"the response should have a status code of (.*)")]
        public void ThenTheResponseShouldHaveAStatusCodeOf(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode?)statusCode);
        }

        [Then(@"the response body should contain a list of users")]
        public void ThenTheResponseBodyShouldContainAListOfUsers()
        {
            var usersResponse = JsonSerializer.Deserialize<UserData>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TestContext.WriteLine(_response.Content);
            using (new AssertionScope()) 
            {
                usersResponse.data.Should().NotBeNull();
                usersResponse.data.Select(user => user.id.Should().BeGreaterThan(0));
                usersResponse.data.Select(user => user.first_name.Should().NotBeNullOrEmpty());
            }
        }

        [Then(@"the response data should be empty")]
        public void ThenTheResponseDataShouldBeEmpty()
        {
            var usersResponse = JsonSerializer.Deserialize<User>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            usersResponse.Should().NotBeNull();
        }

        [Then(@"the response data should be based on the filter criteria applied")]
        public void ThenTheResponseDataShouldBeBasedOnTheFilterCriteriaApplied()
        {
            _response.Content.Should().NotBeNullOrEmpty();
            var userId = _scenarioContext.Get<string>("expectedUser");
            _response.Content.Should().NotBeNullOrEmpty();
            _response.Content.Should().Contain(userId);
        }

    }
}
