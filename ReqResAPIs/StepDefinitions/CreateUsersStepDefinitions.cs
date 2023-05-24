using NUnit.Framework;
using ReqResAPIs.Models;
using ReqResAPIs.Support;
using RestSharp;
using System.Net;
using System.Text.Json;
using TechTalk.SpecFlow.Assist;

namespace ReqResAPIs.StepDefinitions
{
    [Binding]
    public class CreateUsersStepDefinitions : BaseAPITest
    {
        private RestResponse _response;
        private readonly ScenarioContext _scenarioContext;
        private APIHelper<UserData> _helper;
        private NewUser requestPayload;

        public CreateUsersStepDefinitions(ScenarioContext _context)
        {
            _scenarioContext = _context;
            _helper = new APIHelper<UserData>($"{BaseURL}/users");
        }

        [Given(@"I create a request payload for users as below")]
        public void GivenICreateARequestPayloadForUsersAsBelow(Table table)
        {
            var data = table.CreateInstance<NewUser>();
            requestPayload = new NewUser()
            {
                name = data.name,
                job = data.job
            };
        }

        [When(@"I send a POST request to the users endpoint")]
        public void WhenISendAPOSTRequestToTheUsersEndpoint()
        {
            _response = _helper.PostMethod(String.Empty, requestPayload);
        } 

        [Then(@"the status code should be (.*)")]
        public void ThenTheStatusCodeShouldBe(int code)
        {
            var expectedCode = code;
            _response.StatusCode.Should().Be((HttpStatusCode)expectedCode);
        }

        [Then(@"the response should have the new user details")]
        public void ThenTheResponseShouldHaveTheNewUserDetails()
        {
            var res = JsonSerializer.Deserialize<UserResponse>(_response.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            TestContext.WriteLine(_response.Content);
            res.job.Should().Be(requestPayload.job);
            res.name.Should().Be(requestPayload.name);
        }

        [Then(@"the job value should be null")]
        public void ThenTheJobValueShouldBeNull()
        {
            var res = JsonSerializer.Deserialize<UserResponse>(_response.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            res.job.Should().BeNull();
        }

    }
}
