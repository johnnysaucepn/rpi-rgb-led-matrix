using DansClock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ClockFace>();
        services.AddSingleton<RGBLedMatrixOptionsFactory>();
    })
    .RunConsoleAsync();
