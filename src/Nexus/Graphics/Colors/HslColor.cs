namespace Nexus.Graphics.Colors
{
	/// <summary>
	/// http://www.student.kuleuven.be/~m0216922/CG/color.html#RGB_to_HSL_
	/// </summary>
	public struct HslColor
	{
		#region Constructor

		public HslColor(double h, double s, double l)
			: this()
		{
			H = h;
			S = s;
			L = l;
		}

		#endregion

		#region Properties

		public double H { get; set; }
		public double S { get; set; }
		public double L { get; set; }

		#endregion

		#region Methods

		public Color ToRgbColor()
		{
			double r = 0, g = 0, b = 0;

			if (S == 0.0)
			{
				// If saturation is 0, the colour is a shade of grey.
				r = g = b = L;
			}
			else
			{
				// If saturation is higher than 0, more calculations are needed again. Red, green
				// and blue are calculated with the following formulas.
				//Set the temporary values      
				double temp2 = (L < 0.5) ? L * (1 + S) : (L + S) - (L * S);
				double temp1 = 2.0 * L - temp2;
				double tempr = H + 1.0 / 3.0;
				if (tempr > 1) tempr--;
				double tempg = H;
				double tempb = H - 1.0 / 3.0;
				if (tempb < 0) tempb++;

				// Red
				if (tempr < 1.0 / 6.0) r = temp1 + (temp2 - temp1) * 6.0 * tempr;
				else if (tempr < 0.5) r = temp2;
				else if (tempr < 2.0 / 3.0) r = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - tempr) * 6.0;
				else r = temp1;

				// Green
				if (tempg < 1.0 / 6.0) g = temp1 + (temp2 - temp1) * 6.0 * tempg;
				else if (tempg < 0.5) g = temp2;
				else if (tempg < 2.0 / 3.0) g = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - tempg) * 6.0;
				else g = temp1;

				// Blue
				if (tempb < 1.0 / 6.0) b = temp1 + (temp2 - temp1) * 6.0 * tempb;
				else if (tempb < 0.5) b = temp2;
				else if (tempb < 2.0 / 3.0) b = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - tempb) * 6.0;
				else b = temp1;
			}

			return new Color((byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
		}

		#endregion

		#region Static stuff

		public static HslColor FromRgbColor(Color rgb)
		{
			return FromRgbColorF(ColorF.FromRgbColor(rgb));
		}

		public static HslColor FromRgbColorF(ColorF rgb)
		{
			double h, s, l;

			// These two variables are needed because the Lightness is defined as
			// (minColour + maxColour) / 2
			double minColour = System.Math.Min(System.Math.Min(rgb.R, rgb.G), rgb.B);
			double maxColour = System.Math.Max(System.Math.Max(rgb.R, rgb.G), rgb.B);

			// If minColour equals maxColour, we know that R = G = B and thus the colour
			// is a shade of grey. This is a trivial case, hue can be set to anything,
			// saturation has to be to 0 because only then it's a shade of grey, and lightness
			// is set to R = G = B, the shade of the grey.
			if (minColour == maxColour)
			{
				// R = G = B to it's a shade of grey
				h = 0.0; // it doesn't matter what value it has
				s = 0.0;
				l = rgb.R; // doesn't matter if you pick r, b, or b
			}
			else
			{
				// If minColour is not equal to maxColour, we have a real colour instead of
				// a shade of grey, so more calculations are needed:
				// - Lightness (l) is now set to its definition of (minColour + maxColour) / 2
				// - Saturation (s) is then calculated with a different formula depending if light
				//   is in the first half or the second half. This is because the HSL model can be
				//   represented as a double cone, the first cone has a black tip and corresponds
				//   to the first half of lightness values, the second cone has a white tip and
				//   contains the second half of lightness values.
				// - Hue (h) is calculated with a different formula depending on which of the 3
				//   colour components is the dominating one, and then normalised to a number
				//   between 0 and 1.
				l = (minColour + maxColour) / 2.0;

				double delta = maxColour - minColour;
				if (l < 0.5)
					s = delta / (maxColour + minColour);
				else
					s = delta / (2.0 - delta);

				if (rgb.R == maxColour)
					h = (rgb.G - rgb.B) / delta;
				else if (rgb.G == maxColour)
					h = 2.0 + (rgb.B - rgb.R) / delta;
				else
					h = 4.0 + (rgb.R - rgb.G) / delta;
				h /= 6.0; // to bring it to a number between 0 and 1.
				if (h < 0)
					++h;
			}

			return new HslColor(h, s, l);
		}

		#endregion
	}
}