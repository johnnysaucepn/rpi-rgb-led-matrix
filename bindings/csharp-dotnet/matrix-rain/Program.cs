using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rpi_rgb_led_matrix_sharp;
using System.Diagnostics;

using IHost host = Host.CreateDefaultBuilder(args).Build();

IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

const int MAX_HEIGHT = 16;
const int COLOR_STEP = 15;
const int FRAME_STEP = 1;

var rows = config.GetValue<int>("rows", 32);
var cols = config.GetValue<int>("cols", 64);
var hardwareMapping = config.GetValue<string>("led-gpio-mapping");
var ledRgbSequence = config.GetValue<string>("led-rgb-sequence");

var opts = new RGBLedMatrixOptions();
opts.Rows = rows;
opts.Cols = cols;
if (!string.IsNullOrEmpty(hardwareMapping)) opts.HardwareMapping = hardwareMapping;
if (!string.IsNullOrEmpty(ledRgbSequence)) opts.LedRgbSequence = ledRgbSequence;

var matrix = new RGBLedMatrix(new RGBLedMatrixOptions(), Environment.GetCommandLineArgs()[1..]);
var canvas = matrix.CreateOffscreenCanvas();
var rnd = new Random();
var points = new List<Point>();
var recycled = new Stack<Point>();
int frame = 0;
var stopwatch = new Stopwatch();

while (!Console.KeyAvailable)
{
    stopwatch.Restart();

    frame++;

    if (frame % FRAME_STEP == 0)
    {
        if (recycled.Count == 0)
            points.Add(new Point(rnd.Next(0, canvas.Width - 1), 0));
        else
        {
            var point = recycled.Pop();
            point.x = rnd.Next(0, canvas.Width - 1);
            point.y = 0;
            point.recycled = false;
        }
    }
    canvas.Clear();

    foreach (var point in points)
    {
        if (!point.recycled)
        {
            point.y++;

            if (point.y - MAX_HEIGHT > canvas.Height)
            {
                point.recycled = true;
                recycled.Push(point);
            }

            for (var i = 0; i < MAX_HEIGHT; i++)
            {
                canvas.SetPixel(point.x, point.y - i, new Color(0, 255 - i * COLOR_STEP, 0));
            }
        }
    }

    canvas = matrix.SwapOnVsync(canvas);

    // force 30 FPS
    var elapsed = stopwatch.ElapsedMilliseconds;
    if (elapsed < 33)
    {
        Thread.Sleep(33 - (int)elapsed);
    }
}

await host.StartAsync();
