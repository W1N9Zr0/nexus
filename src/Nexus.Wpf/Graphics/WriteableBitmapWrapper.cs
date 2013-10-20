using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = Nexus.Graphics.Colors.Color;

namespace Nexus.Graphics
{
	/// <summary>
	/// With thanks to http://writeablebitmapex.codeplex.com/
	/// </summary>
	public class WriteableBitmapWrapper
	{
		#region Fields

		private const double PreMultiplyFactor = 1 / 255;
		private const int SizeOfARGB = 4;

		private readonly WriteableBitmap _inner;
		private readonly int _width;
		private readonly int[] _pixels;

		#endregion

		#region Properties

		public WriteableBitmap InnerBitmap
		{
			get { return _inner; }
		}

		public int Width
		{
			get { return _width; }
		}

		public int Height
		{
			get { return _inner.PixelHeight; }
		}

		#endregion

		public WriteableBitmapWrapper(WriteableBitmap source)
		{
			_inner = source;
			_width = source.PixelWidth;

			int width = _inner.PixelWidth;
			int height = _inner.PixelHeight;
			_pixels = new int[width * height];
			_inner.CopyPixels(_pixels, width * 4, 0);
		}

		private int[] GetPixels()
		{
			return _pixels;
		}

		public WriteableBitmapWrapper(int width, int height)
		{
			_inner = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
			_pixels = new int[width * height];
			_inner.CopyPixels(_pixels, width * 4, 0);
		}

		#region Clear

		/// <summary>
		/// Fills the whole WriteableBitmap with a color.
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="color">The color used for filling.</param>
		public void Clear(Color color)
		{
			double ai = color.A * PreMultiplyFactor;
			int col = (color.A << 24) | ((byte)(color.R * ai) << 16) | ((byte)(color.G * ai) << 8) | (byte)(color.B * ai);
			int[] pixels = GetPixels();
			int w = _inner.PixelWidth;
			int h = _inner.PixelHeight;
			int len = w * SizeOfARGB;

			// Fill first line
			for (int x = 0; x < w; x++)
			{
				pixels[x] = col;
			}

			// Copy first line
			for (int y = 1; y < h; y++)
			{
				Buffer.BlockCopy(pixels, 0, pixels, y * len, len);
			}
		}

		/// <summary>
		/// Fills the whole WriteableBitmap with an empty color (0).
		/// </summary>
		public void Clear()
		{
			int[] pixels = GetPixels();
			Array.Clear(pixels, 0, pixels.Length);
		}

		#endregion

		#region GetPixel

		/// <summary>
		/// Gets the color of the pixel at the x, y coordinate as a Color struct.
		/// </summary>
		/// <param name="bmp">The WriteableBitmap.</param>
		/// <param name="x">The x coordinate of the pixel.</param>
		/// <param name="y">The y coordinate of the pixel.</param>
		/// <returns>The color of the pixel at x, y as a Color struct.</returns>
		public Color GetPixel(int x, int y)
		{
			int c = GetPixels()[y * _width + x];
			byte a = (byte)(c >> 24);
			//double ai = a / PreMultiplyFactor;
			double ai = 1;
			return Color.FromArgb(a, (byte)((c >> 16) * ai), (byte)((c >> 8) * ai), (byte)(c * ai));
		}

		#endregion

		#region SetPixel

		/// <summary>
		/// Sets the color of the pixel.
		/// </summary>
		/// <param name="x">The x coordinate (row).</param>
		/// <param name="y">The y coordinate (column).</param>
		/// <param name="color">The color.</param>
		public void SetPixel(int x, int y, Color color)
		{
			double ai = color.A * PreMultiplyFactor;
			GetPixels()[y * _width + x] = (color.A << 24) | ((byte)(color.R * ai) << 16) | ((byte)(color.G * ai) << 8) | (byte)(color.B * ai);
		}

		#endregion

		#region Invalidate

		public void Invalidate()
		{
			_inner.WritePixels(
				new Int32Rect(0, 0, _inner.PixelWidth, _inner.PixelHeight),
				_pixels, _inner.BackBufferStride, 0, 0);
		}

		#endregion
	}
}