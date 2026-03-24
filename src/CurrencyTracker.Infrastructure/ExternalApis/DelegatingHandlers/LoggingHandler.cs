using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CurrencyTracker.Infrastructure.ExternalApis;

public class LoggingHandler : DelegatingHandler
{
   private readonly ILogger<LoggingHandler> _logger;

   public LoggingHandler(ILogger<LoggingHandler> logger)
   {
      _logger = logger;
   }    


   protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
   {
       var sw = Stopwatch.StartNew();
       var correlationId = Guid.NewGuid();

       try
        {
            _logger.LogInformation(
                "[{CorrelationId}] External API Request: {Method} {Url}",
                correlationId, request.Method, request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            _logger.LogInformation(
                "[{CorrelationId}] External API Response: {StatusCode} {ElapsedMilliseconds}ms",
                correlationId, response.StatusCode, sw.ElapsedMilliseconds);

            return response;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "[{CorrelationId}] External API Error: {ElapsedMilliseconds}ms", correlationId, sw.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            sw.Stop();
        }
       
   }
}
