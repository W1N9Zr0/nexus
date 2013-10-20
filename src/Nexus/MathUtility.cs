namespace Nexus
{
	public static class MathUtility
	{
		public const double PI = (double) System.Math.PI;
		public const double PI_OVER_2 = (double) (System.Math.PI / 2.0);
		public const double PI_OVER_4 = (double) (System.Math.PI / 4.0);
		public const double TWO_PI = (double)(System.Math.PI * 2.0);

		/// <summary>
		/// Clamps a value to an interval.
		/// </summary>
		/// <param name="value">The input parameter.</param>
		/// <param name="min">The lower clamp threshold.</param>
		/// <param name="max">The upper clamp threshold.</param>
		/// <returns>The clamped value.</returns>
		public static double Clamp(double value, double min, double max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;
			return value;
		}

		/// <summary>
		/// Clamps a value to an interval.
		/// </summary>
		/// <param name="value">The input parameter.</param>
		/// <param name="min">The lower clamp threshold.</param>
		/// <param name="max">The upper clamp threshold.</param>
		/// <returns>The clamped value.</returns>
		public static int Clamp(int value, int min, int max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;
			return value;
		}

		/// <summary>
		/// Clamps a value to an interval.
		/// </summary>
		/// <param name="value">The input parameter.</param>
		/// <param name="min">The lower clamp threshold.</param>
		/// <param name="max">The upper clamp threshold.</param>
		/// <returns>The clamped value.</returns>
		public static byte Clamp(byte value, byte min, byte max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;
			return value;
		}

		public static int Floor(double value)
		{
			return (int) System.Math.Floor(value);
		}

		public static int Ceiling(double value)
		{
			return (int)System.Math.Ceiling(value);
		}

		/// <summary>
		/// Returns a mod b. This differs from the % operator with respect to negative numbers.
		/// </summary>
		/// <param name="a">The dividend.</param>
		/// <param name="b">The divisor.</param>
		/// <returns>a mod b</returns>
		public static double Mod(double a, double b)
		{
			int n = (int)(a / b);

			a -= n * b;
			if (a < 0)
				return a + b;
			return a;
		}

		/// <summary>
		/// Returns a mod b. This differs from the % operator with respect to negative numbers.
		/// </summary>
		/// <param name="a">The dividend.</param>
		/// <param name="b">The divisor.</param>
		/// <returns>a mod b</returns>
		public static int Mod(int a, int b)
		{
			int n = a / b;

			a -= n * b;
			if (a < 0)
				return a + b;
			return a;
		}

		/// <summary>
		/// Interpolates linearly between the supplied values.
		/// </summary>
		/// <param name="value1">The lower interpolation bound.</param>
		/// <param name="value2">The upper interpolation bound.</param>
		/// <param name="amount">The interpolation parameter.</param>
		/// <returns>The interpolated value.</returns>
		public static double Lerp(double value1, double value2, double amount)
		{
			return (1 - amount) * value1 + amount * value2;
			//return Lerp(value1, value2, 0, 1, amount); // This is the same thing
		}

		/// <summary>
		/// Interpolates linearly between the supplied values.
		/// </summary>
		/// <param name="value1">The lower interpolation bound.</param>
		/// <param name="value2">The upper interpolation bound.</param>
		/// <param name="amount">The interpolation parameter.</param>
		/// <returns>The interpolated value.</returns>
		public static int Lerp(int value1, int value2, double amount)
		{
			return (int) ((1 - amount) * value1 + amount * value2);
		}

		/// <summary>
		/// Interpolates linearly between the supplied values.
		/// </summary>
		/// <param name="value1">The lower interpolation bound.</param>
		/// <param name="value2">The upper interpolation bound.</param>
		/// <param name="amount">The interpolation parameter.</param>
		/// <returns>The interpolated value.</returns>
		public static byte Lerp(byte value1, byte value2, double amount)
		{
			return (byte)((1 - amount) * value1 + amount * value2);
		}

		public static double Lerp(double value1, double value2, double startAmount, double endAmount, double amount)
		{
			return (((value2 - value1) * amount) + ((value1 * endAmount) + (value2 * startAmount))) / (endAmount - startAmount);
		}

		public static double Log2(double d)
		{
			return (double) System.Math.Log(d, 2.0);
		}

		public static double PerspectiveInterpolate(double value1, double value2, double w1, double w2, double amountRangeStart, double amountRangeEnd, double amount)
		{
			// Don't ask me how I got this... it's mostly from page 124 of 3D Game Engine Design,
			// with a correction as noted on http://www.geometrictools.com/Books/GameEngineDesign2/BookCorrections.html.
			double numerator = (((value2 * w1) - (value1 * w2)) * amount) + ((value1 * w2 * amountRangeEnd) - (value2 * w1 * amountRangeStart));
			double denominator = ((w1 - w2) * amount) + ((w2 * amountRangeEnd) - (w1 * amountRangeStart));
			return numerator / denominator;
		}

		public static int Round(double value)
		{
			return (int) System.Math.Round(value);
		}

		/// <summary>
		/// A smoothed step function. A cubic function is used to smooth the step between two thresholds.
		/// </summary>
		/// <param name="a">The lower threshold position.</param>
		/// <param name="b">The upper threshold position.</param>
		/// <param name="x">The input parameter.</param>
		/// <returns>The interpolated value.</returns>
		public static double SmoothStep(double a, double b, double x)
		{
			if (x < a)
				return 0;
			if (x >= b)
				return 1;
			x = (x - a) / (b - a);
			return x * x * (3 - 2 * x);
		}

		public static double Saturate(double value)
		{
			value = (value > 1.0) ? 1.0 : value;
			value = (value < 0.0) ? 0.0 : value;
			return value;
		}

		public static double Exp(double value)
		{
			return (double)System.Math.Exp(value);
		}

		public static double Pow(double x, double y)
		{
			return (double)System.Math.Pow(x, y);
		}

		public static double Sqrt(double value)
		{
			return (double) System.Math.Sqrt(value);
		}

		public static double ToDegrees(double radians)
		{
			return (radians * 57.29578);
		}

		public static double ToRadians(double degrees)
		{
			return (degrees * 0.01745329);
		}

		public static double Cos(double radians)
		{
			return (double) System.Math.Cos(radians);
		}

		public static double Acos(double radians)
		{
			return (double)System.Math.Acos(radians);
		}

		public static double Sin(double radians)
		{
			return (double) System.Math.Sin(radians);
		}

		public static double Asin(double sin)
		{
			return (double) System.Math.Asin(sin);
		}

		public static double Tan(double radians)
		{
			return (double) System.Math.Tan(radians);
		}

		public static double Atan2(double y, double x)
		{
			return (double)System.Math.Atan2(y, x);
		}

		public static void Swap(ref double v1, ref double v2)
		{
			double temp = v1;
			v1 = v2;
			v2 = temp;
		}

		public static bool IsZero(double value)
		{
			return (System.Math.Abs(value) < 1.0E-6);
		}

		public static bool Quadratic(double A, double B, double C, out double t0, out double t1)
		{
			// Find quadratic discriminant
			double discrim = B * B - 4.0 * A * C;
			if (discrim < 0.0)
			{
				t0 = t1 = double.MinValue;
				return false;
			}
			double rootDiscrim = Sqrt(discrim);

			// Compute quadratic _t_ values
			double q;
			if (B < 0) q = -0.5 * (B - rootDiscrim);
			else q = -0.5 * (B + rootDiscrim);
			t0 = q / A;
			t1 = C / q;
			if (t0 > t1) Swap(ref t0, ref t1);
			return true;
		}
	}
}