using System;
using System.IO;
using System.Windows.Media.Imaging;
using Nexus.Graphics.Colors;

namespace Nexus.Graphics
{
	public static class ColorSurfaceLoader
	{
		public static ColorSurface LoadFromFile(string uri)
		{
			BitmapImage bitmapImage = new BitmapImage(new Uri(uri));
			WriteableBitmapWrapper writeableBitmap = new WriteableBitmapWrapper(new WriteableBitmap(bitmapImage));

			ColorSurface surface = new ColorSurface(writeableBitmap.Width, writeableBitmap.Height, 1);
			PopulateSurface(surface, writeableBitmap);
			return surface;
		}

		public static void PopulateFromStream(ColorSurface surface, Stream stream)
		{
			BitmapFrame bitmapImage = BitmapFrame.Create(stream);
			PopulateSurface(surface, new WriteableBitmapWrapper(new WriteableBitmap(bitmapImage)));
		}

		private static void PopulateSurface(ColorSurface surface, WriteableBitmapWrapper writeableBitmap)
		{
			for (int y = 0; y < surface.Height; ++y)
				for (int x = 0; x < surface.Width; ++x)
				{
					var c = writeableBitmap.GetPixel(x, y);
					surface[x, y, 0] = (ColorF) c;
				}
		}
	}
}