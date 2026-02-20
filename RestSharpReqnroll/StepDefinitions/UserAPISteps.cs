using Reqnroll;
using FluentAssertions;
using RestSharpReqnroll.Helpers;
using RestSharpReqnroll.Models;
using NUnit.Framework;

namespace RestSharpReqnroll.StepDefinitions
{
    [Binding]
    public class UserAPISteps
    {
        private readonly ScenarioContextHelper _scenarioContext;
        private RestClientHelper? _restClient;
        private string? _baseUrl;

        public UserAPISteps(ScenarioContextHelper scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I have the API base URL \"(.*)\"")]
        public void GivenApiBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            _restClient = new RestClientHelper(baseUrl);
            _scenarioContext.Set("RestClient", _restClient);
            _scenarioContext.Set("BaseUrl", baseUrl);
            
            TestLogger.LogStep($"API base URL set to: {baseUrl}");
            TestLogger.AddParameter("BaseUrl", baseUrl);
        }

        [When("I get user with ID \"(.*)\"")]
        public async Task WhenGetUserById(string userId)
        {
            _restClient ??= _scenarioContext.Get<RestClientHelper>("RestClient");
            
            TestLogger.LogStep($"Fetching user with ID: {userId}");
            TestLogger.AddParameter("UserId", userId);

            var user = await _restClient.GetAsync<UserModel>($"/users/{userId}");
            _scenarioContext.Set("LastUser", user);
            _scenarioContext.Set("LastResponse", _restClient.ResponseContent);
            
            if (_restClient.ResponseContent != null)
            {
                TestLogger.AttachText(_restClient.ResponseContent, $"User_{userId}_Response.json");
            }
        }

        [When("I get all users")]
        public async Task WhenGetAllUsers()
        {
            _restClient ??= _scenarioContext.Get<RestClientHelper>("RestClient");
            
            TestLogger.LogStep("Fetching all users");

            var users = await _restClient.GetAsync<List<UserModel>>("/users");
            _scenarioContext.Set("UsersList", users);
            _scenarioContext.Set("LastResponse", _restClient.ResponseContent);
            
            if (_restClient.ResponseContent != null)
            {
                TestLogger.AttachText(_restClient.ResponseContent, "AllUsers_Response.json");
            }
        }

        [Then("the response status should be success")]
        public void ThenResponseStatusSuccess()
        {
            var response = _scenarioContext.Get<string>("LastResponse");
            TestLogger.LogStep("Verifying response is successful");
            response.Should().NotBeNullOrEmpty("Response should not be empty");
        }

        [Then("the response should contain user data")]
        public void ThenResponseContainsUserData()
        {
            var user = _scenarioContext.Get<UserModel?>("LastUser");
            TestLogger.LogStep("Verifying user data is present");
            user.Should().NotBeNull("User data should be returned");
            user?.Id.Should().BeGreaterThan(0, "User ID should be valid");
        }

        [Then("the response should contain a list of users")]
        public void ThenResponseContainsUsersList()
        {
            var users = _scenarioContext.Get<List<UserModel>?>("UsersList");
            TestLogger.LogStep("Verifying users list is present and not empty");
            users.Should().NotBeNull("Users list should not be empty");
            users?.Count.Should().BeGreaterThan(0, "Users list should not be empty");
        }

        [Then("the user should have name \"(.*)\"")]
        public void ThenUserHasName(string expectedName)
        {
            var user = _scenarioContext.Get<UserModel?>("LastUser");
            TestLogger.LogStep($"Verifying user name is '{expectedName}'");
            TestLogger.AddParameter("ExpectedName", expectedName);
            user?.Name.Should().Be(expectedName, "User name should match");
        }

        [Then("the user should have email \"(.*)\"")]
        public void ThenUserHasEmail(string expectedEmail)
        {
            var user = _scenarioContext.Get<UserModel?>("LastUser");
            TestLogger.LogStep($"Verifying user email is '{expectedEmail}'");
            TestLogger.AddParameter("ExpectedEmail", expectedEmail);
            user?.Email.Should().Be(expectedEmail, "User email should match");
        }
    }
}
