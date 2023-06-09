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
        private RestResponse? _response;
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
            _response?.StatusCode.Should().Be((HttpStatusCode?)statusCode);
        }

        [Then(@"the response body should contain a list of users")]
        public void ThenTheResponseBodyShouldContainAListOfUsers()
        {
            if(_response?.Content is not null )
            {
                UserData? usersResponse = JsonSerializer.Deserialize<UserData>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                List<User>? usersDetails = usersResponse?.data;
                using (new AssertionScope()) 
                {
                    usersDetails.Should().NotBeNull();
                    usersDetails?.Select(user => user.id.Should().BeGreaterThan(0));
                    usersDetails?.Select(user => user.first_name.Should().NotBeNullOrEmpty());
                }
            }
        }

        [Then(@"the response data should be empty")]
        public void ThenTheResponseDataShouldBeEmpty()
        {
            if (_response?.Content is not null)
            {
                var usersResponse = JsonSerializer.Deserialize<User>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                usersResponse.Should().NotBeNull();
            }
        }

        [Then(@"the response data should be based on the filter criteria applied")]
        public void ThenTheResponseDataShouldBeBasedOnTheFilterCriteriaApplied()
        {
            var userId = _scenarioContext.Get<string>("expectedUser");
            if (_response?.Content is not null)
            {
                _response.Content.Should().NotBeNullOrEmpty();
                _response.Content.Should().NotBeNullOrEmpty();
                _response.Content.Should().Contain(userId);
            }
        }

        [Then(@"the response data should contain (.*) and (.*)")]
        public void ThenTheResponseDataShouldContainTheUser(string firstname, string lastname)
        {
            if (_response?.Content is not null)
            {
                UserData? usersResponse = JsonSerializer.Deserialize<UserData>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                int? totalPages = usersResponse?.total_pages;
                int? currentPage = usersResponse?.page;

                if (usersResponse?.data?.Count > 0)
                {
                    TestContext.WriteLine(currentPage);
                    foreach (User? user in usersResponse.data)
                    {
                        if(!String.IsNullOrEmpty(user.first_name) && !String.IsNullOrEmpty(user.last_name))
                        if (user.first_name.Equals(firstname) && user.last_name.Equals(lastname))
                        {
                            Assert.Pass($"User {firstname} {lastname} Exists.");
                            break;
                        }
                    }
                    if (currentPage < totalPages)
                    {
                        currentPage++;
                        _response = _helper.GetMethod($"?page={currentPage}");
                        ThenTheResponseDataShouldContainTheUser(firstname, lastname);
                    }
                }
                else
                {
                    Assert.Fail($"User Response is not available");
                }
            }
        }
    }
}
