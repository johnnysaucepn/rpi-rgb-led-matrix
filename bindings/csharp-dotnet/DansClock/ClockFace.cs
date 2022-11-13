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

            var fontFile = "6x12.bdf";
            var hexColor = _config.GetValue<string>("color");

            var color = Color.HexToColor(hexColor, new Color(0, 255, 0));

            var matrix = new RGBLedMatrix(opts);
            var canvas = matrix.CreateOffscreenCanvas();
            var font = new RGBLedFont(fontFile);
            var glyphWidth = 6;
            var glyphHeight = 12;

            int y = (canvas.Height - glyphHeight) / 2;
            int x = (canvas.Width - (8 * glyphWidth)) / 2;

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Tick...");
                canvas.Clear();
                var currentTime = DateTime.Now.ToLongTimeString(); 
                canvas.DrawText(font, 1, 6, color, currentTime);
                matrix.SwapOnVsync(canvas);
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            }
        }

    }
}
