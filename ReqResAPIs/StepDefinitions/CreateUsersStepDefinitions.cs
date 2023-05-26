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
        private RestResponse? _response;
        private APIHelper<UserData> _helper;
        private ScenarioContext _scenarioContext;

        public CreateUsersStepDefinitions(ScenarioContext context)
        {
            _scenarioContext = context;
            _helper = new APIHelper<UserData>($"{BaseURL}/users");
        }

        [Given(@"I create a request payload for users as below")]
        public void GivenICreateARequestPayloadForUsersAsBelow(Table table)
        {
            var data = table.CreateInstance<NewUser>();
            var requestPayload = new NewUser()
            {
                name = data.name,
                job = data.job
            };
            _scenarioContext.Add("request", requestPayload);
        }

        [When(@"I send a POST request to the users endpoint")]
        public void WhenISendAPOSTRequestToTheUsersEndpoint()
        {
            var payload = _scenarioContext.Get<NewUser>("request");
            _response = _helper.PostMethod(String.Empty, payload);
        }

        [Then(@"the status code should be (.*)")]
        public void ThenTheStatusCodeShouldBe(int code)
        {
            var expectedCode = code;
            _response?.StatusCode.Should().Be((HttpStatusCode)expectedCode);
        }

        [Then(@"the response should have the new user details")]
        public void ThenTheResponseShouldHaveTheNewUserDetails()
        {
            var payload = _scenarioContext.Get<NewUser>("request");
            if(_response?.Content is not null)
            {
                UserResponse? res = JsonSerializer.Deserialize<UserResponse>(_response.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                res?.job.Should().Be(payload.job);
                res?.name.Should().Be(payload.name);
            }
        }

        [Then(@"the job value should be null")]
        public void ThenTheJobValueShouldBeNull()
        {
            if (_response?.Content is not null)
            {
                var res = JsonSerializer.Deserialize<UserResponse>(_response.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                res?.job.Should().BeNull();
            }
        }
    }
}
