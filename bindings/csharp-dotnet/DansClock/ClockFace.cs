using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using rpi_rgb_led_matrix_sharp;

namespace DansClock
{
    public class ClockFace : BackgroundService
    {
        private readonly RGBLedMatrixOptionsFactory _optionsFactory;
        private readonly IConfiguration _config;

        public ClockFace(RGBLedMatrixOptionsFactory optionsFactory, IConfiguration config)
        {
            _optionsFactory = optionsFactory;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting...");
            RGBLedMatrixOptions opts = _optionsFactory.Build();

            var fontFile = "6x13O.bdf";
            var hexTextColor = _config.GetValue<string>("color");
            var hexBgColor = _config.GetValue<string>("bg");

            var textColor = Color.HexToColor(hexTextColor, new Color(0, 128, 0));
            var bgColor = Color.HexToColor(hexBgColor, new Color(128, 00, 0));

            var matrix = new RGBLedMatrix(opts);
            var canvas = matrix.CreateOffscreenCanvas();
            var font = new RGBLedFont(fontFile);
            var glyphWidth = 6;
            var glyphHeight = 9;

            int y = (canvas.Height + glyphHeight) / 2;
            int x = (canvas.Width - (8 * glyphWidth)) / 2;
            var top = 0;
            var left = 0;
            var bottom = canvas.Height - 1;
            var right = canvas.Width - 1;

            while (!cancellationToken.IsCancellationRequested)
            {
                canvas.Clear();
                var currentTime = DateTime.Now;

                canvas.DrawText(font, x, y - 8, textColor, currentTime.ToLongTimeString());
                DrawBorder(bgColor, canvas, top, left, bottom, right);
                DrawBars(canvas, left+2, right-2, bottom-2, currentTime.Hour, currentTime.Minute, currentTime.Second);
                matrix.SwapOnVsync(canvas);
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            }
        }

        private void DrawBars(RGBLedCanvas canvas, int left, int right, int bottom, int hours, int minutes, int seconds)
        {
            var hourColor = new Color(128, 0, 128);
            var minuteColor = new Color(128, 128, 0);
            var secondColor = new Color(0, 128, 128);
            var maxLength = right - left;
            var hourLength = (int)(maxLength * (hours / 24.0));
            var minuteLength = (int)(maxLength * (minutes / 60.0));
            var secondLength = (int)(maxLength * (seconds/ 60.0));

            canvas.DrawLine(left, bottom - 5, left + hourLength, bottom - 5, hourColor);
            canvas.DrawLine(left, bottom - 4, left + hourLength, bottom - 4, hourColor);
            canvas.DrawLine(left, bottom - 3, left + minuteLength, bottom - 3, minuteColor);
            canvas.DrawLine(left, bottom - 2, left + minuteLength, bottom - 2, minuteColor);
            canvas.DrawLine(left, bottom - 1, left + secondLength, bottom - 1, secondColor);
            canvas.DrawLine(left, bottom - 0, left + secondLength, bottom - 0, secondColor);
        }

        private static void DrawBorder(Color bgColor, RGBLedCanvas canvas, int top, int left, int bottom, int right)
        {
            canvas.DrawLine(left, top, right, top, bgColor);
            canvas.DrawLine(right, top, right, bottom, bgColor);
            canvas.DrawLine(left, top, left, bottom, bgColor);
            canvas.DrawLine(left, bottom, right, bottom, bgColor);
        }
    }
}
