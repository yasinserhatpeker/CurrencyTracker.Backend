using System;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class MockEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }
}
