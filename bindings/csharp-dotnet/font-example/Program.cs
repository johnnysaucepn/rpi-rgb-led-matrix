using rpi_rgb_led_matrix_sharp;

if (args.Length < 2)
{
    Console.WriteLine("font-example.exe [font_path] <text>");
    return -1;
}
string text = "Hello World!";
if (args.Length >= 2)
    text = args[1];
string fontFile = "4x6.bdf";
if (args.Length >= 3)
    fontFile = args[2];

var matrix = new RGBLedMatrix(new RGBLedMatrixOptions(), Environment.GetCommandLineArgs()[3..]);
var canvas = matrix.CreateOffscreenCanvas();
var font = new RGBLedFont(fontFile);

canvas.DrawText(font, 1, 6, new Color(0, 255, 0), text);
matrix.SwapOnVsync(canvas);

while (!Console.KeyAvailable)
{
    Thread.Sleep(250);
}

return 0;
