﻿using System.CommandLine;
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
using WSLr.Implementations.DotnetPublishShimBuilder;
using WSLr.Implementations.FileOutputWriter;

// using WSLr.Implementations.RoslynShimBuilder;

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
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("WSLr", LogLevel.Debug)
                    .AddConsole())
        )
        .Build();

void ConfigureServices<RT>(HostBuilderContext hostContext, IServiceCollection services)
    where RT : struct, HasCancel<RT>
{
    services.AddNullServices<RT>();
    // services.AddRoslynShimBuilder<RT>(); // currently does not work due to nuget resolution not working
    services.AddDotnetPublishShimBuilder<RT>();
    services.AddFileOutputWriter<RT>();
    services.AddApplicationServices<RT>();
}
