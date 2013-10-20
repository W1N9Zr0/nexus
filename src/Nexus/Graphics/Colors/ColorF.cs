using System.Runtime.InteropServices;

namespace Nexus.Graphics.Colors
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorF
	{
		#region Fields

		public double A;
		public double B;
		public double G;
		public double R;

		#endregion

		#region Constructor

		public ColorF(double value)
		{
			A = 1.0;
			R = value;
			G = value;
			B = value;
		}

		public ColorF(double r, double g, double b)
		{
			A = 1.0;
			R = r;
			G = g;
			B = b;
		}

		public ColorF(double a, double r, double g, double b)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		public ColorF(ColorRgbF rgb, double a)
		{
			A = a;
			R = rgb.R;
			G = rgb.G;
			B = rgb.B;
		}

		#endregion

		#region Methods

		public double Min()
		{
			return System.Math.Min(System.Math.Min(R, G), B);
		}

		public double Max()
		{
			return System.Math.Max(System.Math.Max(R, G), B);
		}

		public static ColorF Min(ColorF value1, ColorF value2)
		{
			return new ColorF(
				System.Math.Min(value1.R, value2.R),
				System.Math.Min(value1.G, value2.G),
				System.Math.Min(value1.B, value2.B));
		}

		public static ColorF Max(ColorF value1, ColorF value2)
		{
			return new ColorF(
				System.Math.Max(value1.R, value2.R),
				System.Math.Max(value1.G, value2.G),
				System.Math.Max(value1.B, value2.B));
		}

		public static ColorF Exp(ColorF value)
		{
			return new ColorF(
				(double) System.Math.Exp(value.R),
				(double) System.Math.Exp(value.G),
				(double) System.Math.Exp(value.B));
		}

		public static ColorF Saturate(ColorF value)
		{
			return new ColorF(
				MathUtility.Saturate(value.R),
				MathUtility.Saturate(value.G),
				MathUtility.Saturate(value.B));
		}

		public override string ToString()
		{
			return string.Format("{0:F10}, {1:F10}, {2:F10}", R, G, B);
		}

		#endregion

		#region Static stuff

		public static ColorF FromRgbColor(Color value)
		{
			return new ColorF(
				value.A / 255.0,
				value.R / 255.0,
				value.G / 255.0,
				value.B / 255.0);
		}

		public static ColorF FromHexRef(string hexRef)
		{
			return FromRgbColor(Color.FromHexRef(hexRef));
		}

		#endregion

		#region Operators

		public static ColorF operator *(ColorF value, double multiplier)
		{
			return new ColorF(
				value.A * multiplier,
				value.R * multiplier,
				value.G * multiplier,
				value.B * multiplier);
		}

		public static ColorF operator -(ColorF value, double valueToSubtract)
		{
			return new ColorF(
				value.R - valueToSubtract,
				value.G - valueToSubtract,
				value.B - valueToSubtract);
		}

		public static ColorF operator +(ColorF value, double valueToAdd)
		{
			return new ColorF(
				value.R + valueToAdd,
				value.G + valueToAdd,
				value.B + valueToAdd);
		}

		public static ColorF operator /(ColorF value, double divider)
		{
			return new ColorF(
				value.R / divider,
				value.G / divider,
				value.B / divider);
		}

		public static ColorF operator +(ColorF left, ColorF right)
		{
			return new ColorF(
				left.A + right.A,
				left.R + right.R,
				left.G + right.G,
				left.B + right.B);
		}

		public static ColorF operator -(ColorF left, ColorF right)
		{
			return new ColorF(
				left.R - right.R,
				left.G - right.G,
				left.B - right.B);
		}

		public static ColorF operator *(ColorF left, ColorF right)
		{
			return new ColorF(
				left.A * right.A,
				left.R * right.R,
				left.G * right.G,
				left.B * right.B);
		}

		public static explicit operator Color(ColorF value)
		{
			return new Color(
				(byte)(MathUtility.Saturate(value.A) * 255.0),
				(byte)(MathUtility.Saturate(value.R) * 255.0),
				(byte)(MathUtility.Saturate(value.G) * 255.0),
				(byte)(MathUtility.Saturate(value.B) * 255.0));
		}

		public static explicit operator Vector3D(ColorF value)
		{
			return new Vector3D(value.R, value.G, value.B);
		}

		#endregion

		public ColorRgbF Rgb
		{
			get { return new ColorRgbF(R, G, B); }
		}

		public double Red
		{
			get { return R; }
			set { R = value; }
		}

		public double Green
		{
			get { return G; }
			set { G = value; }
		}

		public double Blue
		{
			get { return B; }
			set { B = value; }
		}

		public double Alpha
		{
			get { return A; }
			set { A = value; }
		}

		public static ColorF Invert(ColorF value)
		{
			return new ColorF(1 - value.A, 1 - value.R, 1 - value.G, 1 - value.B);
		}
	}
}