using System.Runtime.InteropServices;

namespace Nexus.Graphics.Colors
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorRgbF
	{
		public double R;
		public double G;
		public double B;

		public ColorRgbF(double r, double g, double b)
		{
			R = r;
			G = g;
			B = b;
		}

		public static ColorRgbF FromRgbColor(Color value)
		{
			return new ColorRgbF(
				value.R / 255.0,
				value.G / 255.0,
				value.B / 255.0);
		}

		#region Operators

		public static ColorRgbF operator *(ColorRgbF left, ColorRgbF right)
		{
			return new ColorRgbF(
				left.R * right.R,
				left.G * right.G,
				left.B * right.B);
		}

		public static ColorRgbF operator *(ColorRgbF value, double multiplier)
		{
			return new ColorRgbF(
				value.R * multiplier,
				value.G * multiplier,
				value.B * multiplier);
		}

		public static ColorRgbF operator +(ColorRgbF left, ColorRgbF right)
		{
			return new ColorRgbF(
				left.R + right.R,
				left.G + right.G,
				left.B + right.B);
		}

		#endregion
	}
}