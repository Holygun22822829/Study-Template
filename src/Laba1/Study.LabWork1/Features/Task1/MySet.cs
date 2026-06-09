using System;
using System.Globalization;

namespace Study.LabWork1.Features.Task1
{
    public sealed class RgbaPixel : IEquatable<RgbaPixel>
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public double Alpha { get; }

        public RgbaPixel(int red, int green, int blue, double alpha)
        {
            Red = ClampByte(red);
            Green = ClampByte(green);
            Blue = ClampByte(blue);
            Alpha = ClampAlpha(alpha);
        }

        private static byte ClampByte(int value)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > 255)
            {
                return 255;
            }

            return (byte)value;
        }

        private static double ClampAlpha(double value)
        {
            if (value < 0.0)
            {
                return 0.0;
            }

            if (value > 1.0)
            {
                return 1.0;
            }

            return value;
        }

        public string ToHex()
        {
            return $"#{Red:X2}{Green:X2}{Blue:X2}";
        }

        public string ToHexWithAlpha()
        {
            int alphaByte = (int)Math.Round(Alpha * 255, MidpointRounding.AwayFromZero);
            alphaByte = Math.Clamp(alphaByte, 0, 255);
            return $"#{Red:X2}{Green:X2}{Blue:X2}{alphaByte:X2}";
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "rgba({0}, {1}, {2}, {3})",
                Red,
                Green,
                Blue,
                Alpha);
        }

        public bool Equals(RgbaPixel? other)
        {
            if (other is null)
            {
                return false;
            }

            return Red == other.Red
                   && Green == other.Green
                   && Blue == other.Blue
                   && Alpha.Equals(other.Alpha);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RgbaPixel);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue, Alpha);
        }

        public static RgbaPixel operator +(RgbaPixel left, RgbaPixel right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new RgbaPixel(
                left.Red + right.Red,
                left.Green + right.Green,
                left.Blue + right.Blue,
                left.Alpha + right.Alpha);
        }

        public static RgbaPixel operator -(RgbaPixel left, RgbaPixel right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new RgbaPixel(
                left.Red - right.Red,
                left.Green - right.Green,
                left.Blue - right.Blue,
                left.Alpha - right.Alpha);
        }

        public static RgbaPixel operator *(RgbaPixel left, RgbaPixel right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new RgbaPixel(
                left.Red * right.Red / 255,
                left.Green * right.Green / 255,
                left.Blue * right.Blue / 255,
                left.Alpha * right.Alpha);
        }

        public static RgbaPixel operator *(RgbaPixel pixel, double scalar)
        {
            ArgumentNullException.ThrowIfNull(pixel);

            return new RgbaPixel(
                (int)Math.Round(pixel.Red * scalar, MidpointRounding.AwayFromZero),
                (int)Math.Round(pixel.Green * scalar, MidpointRounding.AwayFromZero),
                (int)Math.Round(pixel.Blue * scalar, MidpointRounding.AwayFromZero),
                pixel.Alpha * scalar);
        }

        public static RgbaPixel operator *(double scalar, RgbaPixel pixel)
        {
            return pixel * scalar;
        }

        public static RgbaPixel operator /(RgbaPixel pixel, double scalar)
        {
            ArgumentNullException.ThrowIfNull(pixel);

            if (scalar == 0)
            {
                throw new DivideByZeroException("Нельзя делить пиксель на 0.");
            }

            return new RgbaPixel(
                (int)Math.Round(pixel.Red / scalar, MidpointRounding.AwayFromZero),
                (int)Math.Round(pixel.Green / scalar, MidpointRounding.AwayFromZero),
                (int)Math.Round(pixel.Blue / scalar, MidpointRounding.AwayFromZero),
                pixel.Alpha / scalar);
        }

        public static bool operator ==(RgbaPixel? left, RgbaPixel? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(RgbaPixel? left, RgbaPixel? right)
        {
            return !(left == right);
        }
    }
}
