using HZeller.RPi.RGB.LED.Matrix.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rpi_rgb_led_matrix_sharp;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddSingleton<RGBLedMatrixOptionsFactory>();
    })
    .Build();

RGBLedMatrixOptions opts = host.Services.GetRequiredService<RGBLedMatrixOptionsFactory>().Build();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

var fontFile = config.GetValue("font", "4x6.bdf");
var text = config.GetValue("text", "Hello World!");
var hexColor = config.GetValue<string>("color");

var color = ColorExtensions.HexToColor(hexColor, new Color(0, 255, 0));

if (text is null)
{
    Console.WriteLine("font-example.exe --text <text> [--font <font_path>] [--color <hex_color>]");
    return -1;
}

var matrix = new RGBLedMatrix(opts, Environment.GetCommandLineArgs()[1..]);
var canvas = matrix.CreateOffscreenCanvas();
var font = new RGBLedFont(fontFile);

canvas.DrawText(font, 1, 6, color, text);
matrix.SwapOnVsync(canvas);

while (!Console.KeyAvailable)
{
    Thread.Sleep(250);
}

await host.StartAsync();

return 0;
