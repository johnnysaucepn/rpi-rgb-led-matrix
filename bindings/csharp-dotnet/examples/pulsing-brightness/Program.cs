using demo_utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rpi_rgb_led_matrix_sharp;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddSingleton<RGBLedMatrixOptionsFactory>();
    })
    .Build();

RGBLedMatrixOptions opts = host.Services.GetRequiredService<RGBLedMatrixOptionsFactory>().Build();

var matrix = new RGBLedMatrix(opts, Environment.GetCommandLineArgs()[1..]);
var canvas = matrix.CreateOffscreenCanvas();
var maxBrightness = matrix.Brightness;
var count = 0;
const int c = 255;

while (!Console.KeyAvailable)
{
    if (matrix.Brightness < 1)
    {
        matrix.Brightness = maxBrightness;
        count += 1;
    }
    else
    {
        matrix.Brightness -= 1;
    }

    switch (count % 4)
    {
        case 0:
            canvas.Fill(new Color(c, 0, 0));
            break;
        case 1:
            canvas.Fill(new Color(0, c, 0));
            break;
        case 2:
            canvas.Fill(new Color(0, 0, c));
            break;
        case 3:
            canvas.Fill(new Color(c, c, c));
            break;
    }

    canvas = matrix.SwapOnVsync(canvas);

    Thread.Sleep(20);
}

await host.StartAsync();

return 0;

