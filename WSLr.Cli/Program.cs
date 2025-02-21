using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WSLr.Cli;
using WSLr.Cli.Commands;
using WSLr.Cli.Commands.Handlers;

var parser = BuildCommandLineParser();
return parser.Invoke(args);

Parser BuildCommandLineParser() =>
    new CommandLineBuilder(new RootCommand
        {
            new GenerateShimCommand(),
        })
        .UseDefaults()
        .UseHost(
            args => new HostBuilder().ConfigureDefaults(args),
            hostBuilder => hostBuilder
                .ConfigureServices(services => services.AddSingleton(new CommandRuntime<Runtime>(Runtime.New())))
                .ConfigureServices(ConfigureServices<Runtime>)
                .UseCommandHandler<GenerateShimCommand, GenerateShimCommandHandler<Runtime>>()
                .ConfigureLogging(loggingBuilder => loggingBuilder
                    .AddFilter("Microsoft", LogLevel.Warning)))
        .Build();

void ConfigureServices<RT>(HostBuilderContext hostContext, IServiceCollection services)
    where RT : struct, HasCancel<RT>
{
    services.AddNullServices<RT>();
    services.AddApplicationServices<RT>();
}
