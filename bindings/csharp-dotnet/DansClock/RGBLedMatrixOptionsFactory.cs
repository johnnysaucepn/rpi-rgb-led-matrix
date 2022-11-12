using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using rpi_rgb_led_matrix_sharp;

namespace DansClock
{
    public class RGBLedMatrixOptionsFactory
    {
        private readonly IConfiguration _config;

        public RGBLedMatrixOptionsFactory(IConfiguration config)
        {
            _config = config;
        }

        public RGBLedMatrixOptions Build()
        {
            var rows = _config.GetValue<int?>("rows") ?? _config.GetValue<int?>("LED_ROWS");
            var cols = _config.GetValue<int?>("cols") ?? _config.GetValue<int?>("LED_COLS");
            var hardwareMapping = _config.GetValue<string>("led-gpio-mapping") ?? _config.GetValue<string>("LED_GPIO_MAPPING");
            var ledRgbSequence = _config.GetValue<string>("led-rgb-sequence") ?? _config.GetValue<string>("LED_RGB_SEQUENCE");

            var opts = new RGBLedMatrixOptions();
            if (rows.HasValue) opts.Rows = rows.Value;
            if (cols.HasValue) opts.Cols = cols.Value;
            if (hardwareMapping is not null) opts.HardwareMapping = hardwareMapping;
            if (ledRgbSequence is not null) opts.LedRgbSequence = ledRgbSequence;

            return opts;
        }
    }
}
