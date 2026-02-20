# RestSharp with Reqnroll BDD Framework

A **clean, minimal, production-ready** REST API testing framework built with **RestSharp** and **Reqnroll** (SpecFlow community fork), featuring BDD test specifications and NUnit test execution.

## 📖 Quick Summary

This is a **REST API test automation framework** that allows you to write test scenarios in human-readable language (Gherkin) and automatically execute them against REST APIs. Non-technical people can read and understand the tests!

### What It Does
- ✅ Tests REST APIs (HTTP GET, POST, PUT, DELETE)
- ✅ Validates response status codes
- ✅ Validates response data (names, emails, IDs, etc.)
- ✅ Logs all test activities for debugging
- ✅ Generates organized test reports

### Real Example
**You write this in plain English:**
```gherkin
Scenario: Get user by ID
  When I get user with ID "1"
  Then the user should have name "Leanne Graham"
  And the user should have email "Sincere@april.biz"
```

**The framework:**
- Makes HTTP GET request to `/users/1`
- Checks response contains correct name and email
- Logs all activities
- Reports pass/fail

---

## 🏗️ Project Structure

```
RestSharpReqnroll/
├── Features/              # Gherkin BDD scenarios (.feature files)
│   └── UserAPI.feature    # API test scenarios
├── StepDefinitions/       # Gherkin step implementations
│   └── UserAPISteps.cs    # Step definitions for UserAPI.feature
├── Hooks/                 # Test lifecycle hooks
│   └── Hooks.cs           # Before/After scenario setup/teardown
├── Models/                # Data models
│   └── User.cs            # User data model
├── Helpers/               # Utility classes
│   ├── RestClientHelper.cs      # HTTP client wrapper
│   ├── ScenarioContext.cs       # Data sharing between steps
│   └── TestLogger.cs            # Structured logging with Serilog
├── GlobalUsings.cs        # Global namespace imports
├── .runsettings           # Test runner configuration ⭐
├── RestSharpReqnroll.csproj    # Project file & dependencies
└── README.md              # This file
```

---

## ⚙️ Configuration

### .runsettings (Primary Configuration File)

The **`.runsettings`** file controls all test execution settings:

```xml
<!-- NUnit Adapter Settings -->
<NUnit>
  <NumberOfTestWorkers>1</NumberOfTestWorkers>  <!-- Sequential execution -->
  <Verbosity>Detailed</Verbosity>               <!-- Detailed logging -->
</NUnit>

<!-- Test Run Settings -->
<RunConfiguration>
  <TestSessionTimeout>600000</TestSessionTimeout>  <!-- 10 minute timeout -->
  <MaxCpuCount>1</MaxCpuCount>                     <!-- Single thread -->
  <DisableParallelization>true</DisableParallelization>
  <ResultsDirectory>.\TestResults</ResultsDirectory>
</RunConfiguration>

<!-- Reqnroll/SpecFlow Settings -->
<Reqnroll>
  <Language>en</Language>                     <!-- English feature files -->
  <StepAssemblyName>RestSharpReqnroll</StepAssemblyName>
  <Trace>
    <TraceSuccessfulSteps>true</TraceSuccessfulSteps>
    <TraceTimings>true</TraceTimings>        <!-- Show step execution times -->
  </Trace>
</Reqnroll>
```

**Why .runsettings?**
- ✅ Single source of truth for all test configuration
- ✅ Recognized by VS Code, Visual Studio, and dotnet CLI
- ✅ Can be changed without recompiling code
- ✅ Supports different environments (.runsettings.production, etc.)

---

## 🧪 Test Scenarios

All tests target **JSONPlaceholder** (https://jsonplaceholder.typicode.com) - a free fake REST API for testing.

### Scenario 1: Get User by ID
```gherkin
@smoke
Scenario: Get a user by ID
  When I get user with ID "1"
  Then the response status should be success
  And the response should contain user data
```
- Tests single user retrieval from `/users/{id}` endpoint
- Validates HTTP 200 response
- Validates user object exists

### Scenario 2: Get All Users
```gherkin
@smoke
Scenario: Get all users
  When I get all users
  Then the response status should be success
  And the response should contain a list of users
```
- Tests bulk user retrieval from `/users` endpoint
- Validates HTTP 200 response
- Validates list is not empty

### Scenario 3: Verify User Details
```gherkin
Scenario: Verify user details
  When I get user with ID "1"
  Then the user should have name "Leanne Graham"
  And the user should have email "Sincere@april.biz"
```
- Tests specific user data validation
- Validates name matches expected value
- Validates email matches expected value

---

## 🔧 Key Components

### RestClientHelper
**Purpose**: Wraps RestSharp for HTTP operations

```csharp
public class RestClientHelper
{
    // GET request - returns deserialized response
    public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
    
    // POST request - sends body and returns response
    public async Task<T?> PostAsync<T>(string endpoint, object? body = null, ...)
    
    // PUT request - updates resource
    public async Task<T?> PutAsync<T>(string endpoint, object? body = null, ...)
    
    // DELETE request - deletes resource
    public async Task<bool> DeleteAsync(string endpoint, Dictionary<string, string>? headers = null)
}
```

**Features:**
- Automatic JSON serialization/deserialization
- Error handling for failed requests
- Header management
- Response content capture

### TestLogger
**Purpose**: Structured logging for test execution

```csharp
public static class TestLogger
{
    // Log a step execution
    public static void LogStep(string message)
    
    // Add test parameters
    public static void AddParameter(string name, string value)
    
    // Attach JSON data
    public static void AttachJson(string title, object data)
    
    // Attach text/files
    public static void AttachText(string title, string content)
}
```

**Features:**
- Logs to console and file (logs/ directory)
- Uses Serilog for structured logging
- Captures step names, parameters, and attachments
- Great for debugging test failures

### ScenarioContext
**Purpose**: Share data between steps in a scenario

```csharp
// Store data
_scenarioContext.Set("RestClient", client);
_scenarioContext.Set("ResponseData", responseData);

// Retrieve data
var client = _scenarioContext.Get<RestClientHelper>("RestClient");
var data = _scenarioContext.Get<User>("ResponseData");
```

**Use Cases:**
- Store API responses for validation in later steps
- Share test data between different steps
- Track state across scenario execution

### User Model
```csharp
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
}
```

---

## 🚀 Running Tests

### Prerequisites
- .NET 8.0 SDK installed
- Terminal/Command line access

### Run All Tests
```bash
cd /Users/gyaneshkamal/Documents/RestSharp/RestSharpReqnroll
dotnet test
```

This automatically uses `.runsettings` configuration.

### Run Specific Tests
```bash
# Run only @smoke tagged tests
dotnet test --filter "Category=smoke"

# Run specific feature file
dotnet test --filter "TestClass=UserAPISteps"
```

### Run with Detailed Output
```bash
dotnet test --verbosity detailed
```

### Run with Custom Configuration
```bash
dotnet test --settings .runsettings
```

### Expected Output
```
Starting test run, please wait...
Running Reqnroll tests...

  Scenario: Get a user by ID
    ✓ PASSED (245ms)
  
  Scenario: Get all users
    ✓ PASSED (312ms)
  
  Scenario: Verify user details
    ✓ PASSED (189ms)

Test Run Successful.
Total tests: 3
Passed: 3
Failed: 0
Duration: 746ms
```

---

## 📊 Test Results

After running tests, results are stored in **TestResults/** directory:

```
TestResults/
├── test-results.trx    # NUnit test results (XML format)
└── logs/               # Test execution logs
    ├── yyyy-MM-dd.log  # Daily log file
    └── yyyy-MM-dd_HH-mm-ss.log
```

View the log files to understand test execution:
```bash
cat TestResults/logs/*.log
```

---

## 🛠️ Building the Project

```bash
# Restore NuGet packages
dotnet restore

# Build project
dotnet build

# Clean build
dotnet clean && dotnet build
```

Expected output:
```
Restore completed in 1.23s
Build succeeded!
```

---

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| **RestSharp** | 107.3.0 | HTTP client for API calls |
| **Reqnroll** | 2.0.0 | BDD framework (SpecFlow fork) |
| **NUnit** | 4.1.0 | Test runner |
| **FluentAssertions** | 6.12.1 | Readable assertions |
| **Serilog** | 3.1.1 | Structured logging |
| **Newtonsoft.Json** | 13.0.4 | JSON serialization |

---

## 🎯 Adding New Tests

### Step 1: Create Feature File
Create `Features/YourFeature.feature`:
```gherkin
Feature: Your Feature Name
  Description of what you're testing

  Scenario: Test scenario name
    Given some precondition
    When I perform an action
    Then I expect a result
```

### Step 2: Implement Step Definitions
Create `StepDefinitions/YourSteps.cs`:
```csharp
using Reqnroll;
using RestSharpReqnroll.Helpers;

[Binding]
public class YourSteps
{
    private readonly ScenarioContextHelper _context;
    
    public YourSteps(ScenarioContextHelper context)
    {
        _context = context;
    }
    
    [Given("some precondition")]
    public void GivenPrecondition()
    {
        TestLogger.LogStep("Setting up test");
        // Your code here
    }
    
    [When("I perform an action")]
    public void WhenAction()
    {
        TestLogger.LogStep("Performing action");
        // Your code here
    }
    
    [Then("I expect a result")]
    public void ThenResult()
    {
        TestLogger.LogStep("Verifying result");
        // Your assertions here
    }
}
```

### Step 3: Run Tests
```bash
dotnet test
```

Reqnroll automatically:
- Detects your new feature file
- Generates code-behind files
- Binds steps to your implementations
- Runs your scenarios

---

## 🐛 Troubleshooting

### Issue: Tests not found
**Solution:** Make sure your feature files end in `.feature` and step classes have `[Binding]` attribute

### Issue: Step definitions not recognized
**Solution:** Verify step definitions have `[Given]`, `[When]`, `[Then]` attributes matching your feature text

### Issue: Timeout errors
**Solution:** Increase `TestSessionTimeout` in `.runsettings` (milliseconds)

### Issue: API returns 404 not found
**Solution:** Check that base URL is correct in feature file or step definition

### Issue: Tests run slowly
**Solution:** Change `.runsettings` to enable parallel execution:
```xml
<ExecutionMode>Parallel</ExecutionMode>
<Threads>4</Threads>
```

---

## 📝 Best Practices

### 1. Keep Tests Readable
Write feature files for **humans**, not code:
```gherkin
# ✅ Good - Clear intent
Given the API is available at https://jsonplaceholder.typicode.com
When I get user with ID "1"
Then the response should contain a user

# ❌ Avoid - Too technical
Given rest client configured with baseUrl
When GET /users/1 returns 200
Then verify response.body.id == 1
```

### 2. Use Data Sharing
Share data between steps with ScenarioContext:
```csharp
// Step 1: Get user
_context.Set("UserId", userId);

// Step 2: Use stored UserId
var userId = _context.Get<int>("UserId");
```

### 3. Use Tags for Organization
```gherkin
@smoke @critical
Scenario: Critical user lookup
  When I get user with ID "1"
  Then the response status should be success
```

Run specific tests:
```bash
dotnet test --filter "Category=smoke"
```

### 4. Log Important Information
```csharp
TestLogger.LogStep("Getting user data");
TestLogger.AddParameter("UserId", userId);
TestLogger.AttachJson("User Response", userResponse);
```

### 5. Use Meaningful Assertions
```csharp
// ✅ Good - Clear what's being tested
user.Name.Should().Be("Leanne Graham");
user.Email.Should().Contain("@");
response.StatusCode.Should().Be(200);

// ❌ Avoid - Unclear
Assert.IsNotNull(user);
Assert.IsTrue(response != null);
```

---

## 🔄 Workflow Example

**1. Write Feature (Plain English)**
```gherkin
Scenario: Verify user email format
  When I get user with ID "1"
  Then the user should have a valid email
```

**2. Implement Steps (C# Code)**
```csharp
[Then("the user should have a valid email")]
public void ThenValidEmail()
{
    var user = _context.Get<User>("CurrentUser");
    user.Email.Should().Contain("@");
    TestLogger.LogStep("Email validation passed");
}
```

**3. Run Tests**
```bash
dotnet test
```

**4. Read Logs**
```bash
cat TestResults/logs/*.log
```

**5. Debug if Needed**
- Check log file for step-by-step execution
- Use TestLogger.AttachJson() to inspect data
- Add breakpoints in step definitions

---

## 📚 Technology Stack

- **Language**: C# (.NET 8.0)
- **HTTP Client**: RestSharp 107.3.0
- **BDD Framework**: Reqnroll 2.0.0 (SpecFlow fork)
- **Test Runner**: NUnit 4.1.0
- **Assertions**: FluentAssertions 6.12.1
- **Logging**: Serilog 3.1.1
- **JSON**: Newtonsoft.Json 13.0.4
- **Configuration**: .runsettings (XML-based)

---

## ✅ Project Status

- ✅ All 3 tests passing (100%)
- ✅ Clean, minimal codebase
- ✅ Production-ready structure
- ✅ Comprehensive logging
- ✅ Well-documented
- ✅ Single configuration file (.runsettings)
- ✅ No unnecessary dependencies

---

## 🤝 Contributing

To add new tests:

1. Create feature file in `Features/`
2. Create step definitions in `StepDefinitions/`
3. Run `dotnet test` to verify
4. Commit both .feature and .cs files
5. Update README if adding new capabilities

---

## 📄 License

This project is open source and available under MIT License.

---

## 🎓 Learning Resources

- **Reqnroll Docs**: https://docs.reqnroll.net/
- **BDD (Behavior Driven Development)**: https://cucumber.io/docs/bdd/
- **Gherkin Language**: https://cucumber.io/docs/gherkin/
- **RestSharp**: https://restsharp.dev/
- **NUnit**: https://docs.nunit.org/
- **FluentAssertions**: https://fluentassertions.com/

---

## 🆘 Support

For issues or questions:
1. Check the **Troubleshooting** section above
2. Review test logs in `TestResults/logs/`
3. Check step definitions match feature file text
4. Ensure API is reachable (https://jsonplaceholder.typicode.com)

---

## 📅 Version History

**v1.0.0 (Current)** - February 20, 2026
- Clean, production-ready project structure
- Single .runsettings configuration file
- Removed unnecessary appsettings.json and specflow.json
- Complete documentation
- All tests passing (3/3)
- Serilog logging integrated
- No unnecessary dependencies
