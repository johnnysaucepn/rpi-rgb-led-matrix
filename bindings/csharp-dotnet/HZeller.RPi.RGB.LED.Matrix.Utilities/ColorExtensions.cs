using rpi_rgb_led_matrix_sharp;

namespace HZeller.RPi.RGB.LED.Matrix.Utilities
{
    public static class ColorExtensions
    {
        public static Color HexToColor(string hexColor, Color defaultValue)
        {
            if (hexColor is null) return defaultValue;

            try
            {
                var cleanHexColor = hexColor.Replace("#", "");
                if (cleanHexColor.Length == 6)
                {
                    var red = Convert.ToInt32(cleanHexColor[0..2], 16);
                    var green = Convert.ToInt32(cleanHexColor[2..4], 16);
                    var blue = Convert.ToInt32(cleanHexColor[4..6], 16);
                    return new Color(red, green, blue);
                }
                else if (hexColor.Length == 3)
                {
                    var red = Convert.ToInt32(new string(cleanHexColor[0], 2), 16);
                    var green = Convert.ToInt32(new string(cleanHexColor[1], 2), 16);
                    var blue = Convert.ToInt32(new string(cleanHexColor[2], 2), 16);
                    return new Color(red, green, blue);
                }
                else
                {
                    throw new FormatException("Must be of format #xxx or #xxxxxx");
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Could not parse color '{hexColor}': {ex.Message}");
                return defaultValue;
            }
        }
    }
}
