using ReqResAPIs.Models;
using ReqResAPIs.Support;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace ReqResAPIs.StepDefinitions
{
    [Binding]
    public sealed class UpdatePatchStepDefinitions : BaseAPITest
    {
        private readonly ScenarioContext _scenarioContext;
        private RestResponse _response;
        private readonly APIHelper<UserData> _helper;

        public UpdatePatchStepDefinitions(ScenarioContext context)
        {
            _scenarioContext = context;
            _helper = new APIHelper<UserData>($"{BaseURL}/users");
        }


        [When(@"I update the user with id (.*) using PUT method")]
        public void WhenIUpdateTheUserWithIdUsingPUTMethod(int id)
        {
            string name = Utility.GenerateString();
            var requestData = new
            {
                name = name,
                job = "tester"
            };
            _response = _helper.PutMethod($"{id}", requestData);
            _scenarioContext.Add("newname", name);
        }

        [Then(@"user details should be updated")]
        public void ThenUserDetailsShouldBeUpdated()
        {
            string name = _scenarioContext.Get<string>("newname");
            var response = JsonSerializer.Deserialize<UserResponse>(_response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.name.Should().Be(name);
        }
    }
}
