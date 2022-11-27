using HZeller.RPi.RGB.LED.Matrix.Utilities;
using rpi_rgb_led_matrix_sharp;

namespace TestProject
{
    public class HexToColorTests
    {
        private Color Black = new Color(0, 0, 0);
        [Fact]
        public void TestRed()
        {
            var red = ColorExtensions.HexToColor("#ff0000", Black);
            Assert.Equal(255, red.R);
            Assert.Equal(0, red.G);
            Assert.Equal(0, red.B);
        }

        [Fact]
        public void TestGreen()
        {
            var red = ColorExtensions.HexToColor("#00ff00", Black);
            Assert.Equal(0, red.R);
            Assert.Equal(255, red.G);
            Assert.Equal(0, red.B);
        }

        [Fact]
        public void TestBlue()
        {
            var red = ColorExtensions.HexToColor("#0000ff", Black);
            Assert.Equal(0, red.R);
            Assert.Equal(0, red.G);
            Assert.Equal(255, red.B);
        }

        [Fact]
        public void TestPurple()
        {
            var red = ColorExtensions.HexToColor("#800080", Black);
            Assert.Equal(128, red.R);
            Assert.Equal(0, red.G);
            Assert.Equal(128, red.B);
        }

        [Fact]
        public void TestYellow()
        {
            var red = ColorExtensions.HexToColor("808000", Black);
            Assert.Equal(128, red.R);
            Assert.Equal(128, red.G);
            Assert.Equal(0, red.B);
        }
    }
}
