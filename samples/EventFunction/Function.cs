using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Kralizek.Lambda;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EventFunction
{
    public class Function : EventFunction<string>
    {
        protected override void Configure(IConfigurationBuilder builder)
        {
            builder.AddEnvironmentVariables();
        }

        protected override void ConfigureLogging(ILoggerFactory loggerFactory, IExecutionEnvironment executionEnvironment)
        {
            loggerFactory.AddLambdaLogger(new LambdaLoggerOptions
            {
                IncludeCategory = true,
                IncludeLogLevel = true,
                IncludeNewline = true
            });
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            RegisterHandler<EventHandler>(services);
        }
    }

    public class EventHandler : IEventHandler<string>
    {
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(ILogger<EventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(string input, ILambdaContext context)
        {
            _logger.LogInformation(input);
        }
    }
}
