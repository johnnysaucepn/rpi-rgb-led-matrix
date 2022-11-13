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
            var glyphHeight = 13;

            int y = (canvas.Height + glyphHeight) / 2;
            int x = (canvas.Width - (8 * glyphWidth)) / 2;
            var top = 0;
            var left = 0;
            var bottom = canvas.Height - 1;
            var right = canvas.Width - 1;

            while (!cancellationToken.IsCancellationRequested)
            {
                canvas.Clear();
                var currentTime = DateTime.Now.ToLongTimeString(); 
                canvas.DrawText(font, x, y, textColor, currentTime);
                canvas.DrawLine(left, top, right, top, bgColor);
                canvas.DrawLine(right, top, right, bottom, bgColor);
                canvas.DrawLine(left, top, left, bottom, bgColor);
                canvas.DrawLine(left, bottom, right, bottom, bgColor);
                matrix.SwapOnVsync(canvas);
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            }
        }

    }
}
