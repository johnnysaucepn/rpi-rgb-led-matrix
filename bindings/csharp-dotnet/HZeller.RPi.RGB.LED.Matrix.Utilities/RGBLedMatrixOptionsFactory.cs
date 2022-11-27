using Microsoft.Extensions.Configuration;

using rpi_rgb_led_matrix_sharp;

namespace HZeller.RPi.RGB.LED.Matrix.Utilities
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
            var rows = _config.GetValue<int?>("led-rows") ?? _config.GetValue<int?>("LED_ROWS");
            var cols = _config.GetValue<int?>("led-cols") ?? _config.GetValue<int?>("LED_COLS");
            var chainLength = _config.GetValue<int?>("led-chain") ?? _config.GetValue<int?>("LED_CHAIN");
            var parallelChains = _config.GetValue<int?>("led-parallel") ?? _config.GetValue<int?>("LED_PARALLEL");
            var hardwareMapping = _config.GetValue<string>("led-gpio-mapping") ?? _config.GetValue<string>("LED_GPIO_MAPPING");
            var rgbSequence = _config.GetValue<string>("led-rgb-sequence") ?? _config.GetValue<string>("LED_RGB_SEQUENCE");
            var brightness = _config.GetValue<int?>("led-brightness") ?? _config.GetValue<int?>("LED_BRIGHTNESS");
            var gpioSlowdown = _config.GetValue<int?>("led-slowdown-gpio") ?? _config.GetValue<int?>("LED_SLOWDOWN_GPIO");

            var opts = new RGBLedMatrixOptions();
            if (rows.HasValue) opts.Rows = rows.Value;
            if (cols.HasValue) opts.Cols = cols.Value;
            if (chainLength.HasValue) opts.ChainLength = chainLength.Value;
            if (parallelChains.HasValue) opts.Parallel = parallelChains.Value;
            if (hardwareMapping is not null) opts.HardwareMapping = hardwareMapping;
            if (rgbSequence is not null) opts.LedRgbSequence = rgbSequence;
            if (brightness.HasValue) opts.Brightness = brightness.Value;
            if (gpioSlowdown.HasValue) opts.GpioSlowdown = gpioSlowdown.Value;

            return opts;
        }
    }
}
