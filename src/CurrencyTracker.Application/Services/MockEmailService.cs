using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class MockEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
       Console.WriteLine("\n================ EMAIL SIMULATOR ================");
       Console.WriteLine($"TO      : {to}");
       Console.WriteLine($"SUBJECT: {subject}");
       Console.WriteLine($"BODY: {body}");
       Console.WriteLine("=================================================\n");

       return Task.CompletedTask;
    }
}
