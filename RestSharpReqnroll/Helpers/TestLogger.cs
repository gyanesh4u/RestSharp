using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace RestSharpReqnroll.Helpers
{
    public class TestLogger
    {
        private static ILogger _logger;

        static TestLogger()
        {
            // Initialize Serilog for structured logging
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "test-logs-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();
        }

        /// <summary>
        /// Logs a step to test report
        /// </summary>
        public static void LogStep(string stepName)
        {
            _logger.Information("✓ Step: {StepName}", stepName);
            TestContext.WriteLine($"✓ Step: {stepName}");
        }

        /// <summary>
        /// Logs a step with status
        /// </summary>
        public static void LogStep(string stepName, string status)
        {
            _logger.Information("Step [{Status}]: {StepName}", status, stepName);
            TestContext.WriteLine($"Step [{status}]: {stepName}");
        }

        /// <summary>
        /// Attaches a file to the test report
        /// </summary>
        public static void AttachFile(string filePath, string attachmentName)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string attachmentsDir = "allure-results/attachments";
                    Directory.CreateDirectory(attachmentsDir);
                    
                    string fileName = Path.GetFileName(filePath);
                    string destPath = Path.Combine(attachmentsDir, fileName);
                    File.Copy(filePath, destPath, true);
                    
                    _logger.Information("📎 Attachment added: {AttachmentName} -> {FilePath}", attachmentName, fileName);
                    TestContext.WriteLine($"📎 Attachment added: {attachmentName}");
                }
                else
                {
                    _logger.Warning("⚠️ File not found: {FilePath}", filePath);
                    TestContext.WriteLine($"⚠️ File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error attaching file: {AttachmentName}", attachmentName);
                TestContext.WriteLine($"❌ Error attaching file: {ex.Message}");
            }
        }

        /// <summary>
        /// Attaches text content to the test report
        /// </summary>
        public static void AttachText(string content, string attachmentName)
        {
            try
            {
                string attachmentsDir = "logs/attachments";
                Directory.CreateDirectory(attachmentsDir);
                
                string filePath = Path.Combine(attachmentsDir, $"{attachmentName}.txt");
                File.WriteAllText(filePath, content);
                
                _logger.Information("📎 Text attachment added: {AttachmentName}", attachmentName);
                TestContext.WriteLine($"📎 Text attachment added: {attachmentName}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error attaching text: {AttachmentName}", attachmentName);
                TestContext.WriteLine($"❌ Error attaching text: {ex.Message}");
            }
        }

        /// <summary>
        /// Attaches JSON content to the test report
        /// </summary>
        public static void AttachJson(string jsonContent, string attachmentName)
        {
            try
            {
                string attachmentsDir = "logs/attachments";
                Directory.CreateDirectory(attachmentsDir);
                
                string filePath = Path.Combine(attachmentsDir, $"{attachmentName}.json");
                File.WriteAllText(filePath, jsonContent);
                
                _logger.Information("📎 JSON attachment added: {AttachmentName}", attachmentName);
                TestContext.WriteLine($"📎 JSON attachment added: {attachmentName}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error attaching JSON: {AttachmentName}", attachmentName);
                TestContext.WriteLine($"❌ Error attaching JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a link to the test report
        /// </summary>
        public static void AddLink(string url, string linkText, string linkType = "custom")
        {
            try
            {
                _logger.Information("🔗 [{LinkType}] Link added: {LinkText} -> {Url}", linkType, linkText, url);
                TestContext.WriteLine($"🔗 Link added: {linkText}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error adding link: {LinkText}", linkText);
                TestContext.WriteLine($"❌ Error adding link: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a test parameter to the report
        /// </summary>
        public static void AddParameter(string parameterName, string parameterValue)
        {
            try
            {
                _logger.Information("📝 Parameter: {ParameterName} = {ParameterValue}", parameterName, parameterValue);
                TestContext.WriteLine($"📝 Parameter: {parameterName} = {parameterValue}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error adding parameter: {ParameterName}", parameterName);
                TestContext.WriteLine($"❌ Error adding parameter: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the description for the test
        /// </summary>
        public static void SetDescription(string description)
        {
            try
            {
                _logger.Information("📄 Description: {Description}", description);
                TestContext.WriteLine($"📄 Description: {description}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error setting description");
                TestContext.WriteLine($"❌ Error setting description: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the feature for the test
        /// </summary>
        public static void SetFeature(string feature)
        {
            try
            {
                _logger.Information("🏷️ Feature: {Feature}", feature);
                TestContext.WriteLine($"🏷️ Feature: {feature}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error setting feature");
                TestContext.WriteLine($"❌ Error setting feature: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the story for the test
        /// </summary>
        public static void SetStory(string story)
        {
            try
            {
                _logger.Information("📖 Story: {Story}", story);
                TestContext.WriteLine($"📖 Story: {story}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error setting story");
                TestContext.WriteLine($"❌ Error setting story: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a label to the test report
        /// </summary>
        public static void AddLabel(string labelName, string labelValue)
        {
            try
            {
                _logger.Information("🏷️ Label: {LabelName} = {LabelValue}", labelName, labelValue);
                TestContext.WriteLine($"🏷️ Label: {labelName} = {labelValue}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "❌ Error adding label: {LabelName}", labelName);
                TestContext.WriteLine($"❌ Error adding label: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a test as failed with a custom message
        /// </summary>
        public static void FailTest(string failureMessage)
        {
            _logger.Error("❌ Test Failed: {FailureMessage}", failureMessage);
            TestContext.WriteLine($"❌ Test Failed: {failureMessage}");
            Assert.Fail(failureMessage);
        }

        /// <summary>
        /// Closes the logger
        /// </summary>
        public static void CloseLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
