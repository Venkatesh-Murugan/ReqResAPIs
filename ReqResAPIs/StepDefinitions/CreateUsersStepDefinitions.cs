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
        private RestRequest restRequest;
        private RestResponse restResponse;
        private RestClient restClient;
        private readonly ScenarioContext _scenarioContext;

        public CreateUsersStepDefinitions(ScenarioContext _context)
        {
            _scenarioContext = _context;
            restClient = new RestClient(BaseURL);
        }

        [Given(@"I create a request payload for '([^']*)' as below")]
        public void GivenICreateARequestPayload(string users, Table table)
        {
            var data = table.CreateInstance<NewUser>();
            restRequest = new RestRequest(users, Method.Post);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(data);
        }

        [When(@"I send a POST request to the users endpoint")]
        public void WhenISendAPOSTRequestToTheUsersEndpoint()
        {
            restResponse = restClient.Execute(restRequest);
        } 

        [Then(@"the status code should be (.*)")]
        public void ThenTheStatusCodeShouldBe(int code)
        {
            var expectedCode = code;
            restResponse.StatusCode.Should().Be((HttpStatusCode)expectedCode);
        }

        [Then(@"the response should have the new user details")]
        public void ThenTheResponseShouldHaveTheNewUserDetails()
        {
            var res = JsonSerializer.Deserialize<UserResponse>(restResponse.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            res.id.Should().NotBeNull();
            res.name.Should().NotBeNullOrEmpty();
            TestContext.WriteLine($"{ res.id },{ res.name}");
        }

        [Then(@"the job value should be null")]
        public void ThenTheJobValueShouldBeNull()
        {
            var res = JsonSerializer.Deserialize<UserResponse>(restResponse.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            res.job.Should().BeNull();
        }

    }
}
