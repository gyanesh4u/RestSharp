using Reqnroll;
using RestSharpReqnroll.Helpers;
using NUnit.Framework;

namespace RestSharpReqnroll.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContextHelper _scenarioContext;

        public Hooks(ScenarioContextHelper scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenarioContext.Clear();
            TestContext.WriteLine("Starting new scenario...");
            TestContext.WriteLine($"Scenario: {scenarioContext.ScenarioInfo.Title}");
            
            // Log scenario tags
            if (scenarioContext.ScenarioInfo.Tags.Length > 0)
            {
                TestContext.WriteLine($"Tags: {string.Join(", ", scenarioContext.ScenarioInfo.Tags)}");
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            TestContext.WriteLine("Scenario completed.");
            
            if (scenarioContext.TestError != null)
            {
                TestContext.WriteLine($"Scenario failed with error: {scenarioContext.TestError.Message}");
                TestContext.WriteLine($"Error Details:\n{scenarioContext.TestError.StackTrace}");
            }
            
            _scenarioContext.Clear();
        }
    }
}
