using CurrencyTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
namespace CurrencyTracker.Infrastructure.EmailVerification;

public class MockEmailService :IEmailService
{

    private readonly ILogger _logger;

    public MockEmailService(ILogger logger)
    {
        _logger=logger;
    }
   public Task SendEmailAsync(string to, string subject, string body)
    {
       _logger.LogInformation("\n================ EMAIL SIMULATOR ================");
       _logger.LogInformation($"TO      : {to}");
       _logger.LogInformation($"SUBJECT: {subject}");
       _logger.LogInformation($"BODY: {body}");
       _logger.LogInformation("=================================================\n");

       return Task.CompletedTask;
    }
}
