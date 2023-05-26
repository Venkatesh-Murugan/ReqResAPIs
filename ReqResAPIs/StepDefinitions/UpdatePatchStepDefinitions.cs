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
        private RestResponse? _response;
        private readonly APIHelper<UserData> _helper;

        public UpdatePatchStepDefinitions(ScenarioContext context)
        {
            _scenarioContext = context;
            _helper = new APIHelper<UserData>($"{BaseURL}/users");
        }

        [When(@"I update the user with id (.*) using PATCH method")]
        [When(@"I update the user with id (.*) using PUT method")]
        public void WhenIUpdateTheUserWithId(int id)
        {
            string name = Utility.GenerateString(5);
            var requestData = new
            {
                name = name,
                job = $"{Utility.GenerateString(5)}"
            };
            _response = _helper.PutMethod($"{id}", requestData);
            _scenarioContext.Add("newname", name);
            _scenarioContext.Add("job", requestData.job);
        }

        [Then(@"user details should be updated")]
        public void ThenUserDetailsShouldBeUpdated()
        {
            string expectedName = _scenarioContext.Get<string>("newname");
            string expectedJob = _scenarioContext.Get<string>("job");
            _response?.StatusCode.Should().Be(HttpStatusCode.OK);
            if(_response?.Content is not null)
            {
                UserResponse? response = JsonSerializer.Deserialize<UserResponse>(_response.Content);
                response?.name.Should().Be(expectedName);
                response?.job.Should().Be(expectedJob);

            }
        }
    }
}
