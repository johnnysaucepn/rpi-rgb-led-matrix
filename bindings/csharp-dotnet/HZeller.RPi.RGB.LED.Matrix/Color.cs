namespace rpi_rgb_led_matrix_sharp
{
    public struct Color
    {
        public Color (int r, int g, int b)
        {
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
        }
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        public byte R;
        public byte G;
        public byte B;

        public static Color HexToColor(string hexColor, Color defaultValue)
        {
            if (hexColor is null) return defaultValue;

            try
            {
                var cleanHexColor = hexColor.Replace("#", "");
                if (cleanHexColor.Length == 6)
                {
                    var red = Convert.ToInt32(cleanHexColor[0..1], 16);
                    var blue = Convert.ToInt32(cleanHexColor[2..3], 16);
                    var green = Convert.ToInt32(cleanHexColor[4..5], 16);
                    return new Color(red, green, blue);
                }
                else if (hexColor.Length == 3)
                {
                    var red = Convert.ToInt32(new string(cleanHexColor[0], 2), 16);
                    var green = Convert.ToInt32(new string(cleanHexColor[1], 2), 16);
                    var blue = Convert.ToInt32(new string(cleanHexColor[2], 2), 16);
                    return new Color(red, green, blue);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine($"Could not parse color '{hexColor}'.");
                return defaultValue;
            }
            return defaultValue;
        }
    }
}
