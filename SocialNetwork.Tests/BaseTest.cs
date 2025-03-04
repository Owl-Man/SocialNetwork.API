namespace SocialNetwork.Tests;
using Xunit;
using Microsoft.Extensions.Logging;

public class BaseTest
{
    
    private readonly ILogger<BaseTest> logger;

    public BaseTest()
    {
        logger = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddConsole();
        }).CreateLogger<BaseTest>();
    }

    [Fact]
    public void TwoPlusTwo_ShouldBeFour()
    {
        int a = 2;
        int b = 2;
        
        logger.LogDebug("Debug: Starting test with {A} + {B}", a, b);
        logger.LogInformation("Info: Performing addition");

        int result = a + b;

        if (result != 4)
        {
            logger.LogWarning("Warning: Expected 4, but got {Result}", result);
            logger.LogError("Error: Incorrect addition result!");
        }
        else
        {
            logger.LogDebug("Debug: Verification successful, result is correct");
        }

        Assert.Equal(4, result);
        logger.LogInformation("Test passed successfully!");
    }
    
}