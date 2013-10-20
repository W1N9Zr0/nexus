using System.Globalization;
using Nexus.Objects2D;

namespace Nexus.Graphics
{
	public struct Viewport3D
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public double MinDepth { get; set; }

		public double MaxDepth { get; set; }

		public Viewport3D(int x, int y, int width, int height, double minDepth, double maxDepth)
			: this()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			MinDepth = minDepth;
			MaxDepth = maxDepth;
		}

		public Viewport3D(int x, int y, int width, int height)
			: this()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			MinDepth = 0;
			MaxDepth = 1;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3} MinDepth:{4} MaxDepth:{5}}}", new object[] { X, Y, Width, Height, MinDepth, MaxDepth });
		}

		private static bool WithinEpsilon(double a, double b)
		{
			double num = a - b;
			return ((-1.401298E-45 <= num) && (num <= double.Epsilon));
		}

		/// <summary>
		/// Projects a scene point onto the screen.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="projection"></param>
		/// <param name="view"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public Point3D Project(Point3D source, Matrix3D projection, Matrix3D view, Matrix3D world)
		{
			Matrix3D matrix = world * view * projection;
			Point3D vector = Point3D.Transform(source, matrix);
			double a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
			if (!WithinEpsilon(a, 1))
				vector = vector / a;
			vector.X = (((vector.X + 1) * 0.5) * Width) + X;
			vector.Y = (((-vector.Y + 1) * 0.5) * Height) + Y;
			vector.Z = (vector.Z * (MaxDepth - MinDepth)) + MinDepth;
			return vector;
		}

		/// <summary>
		/// Projects a screen point into the scene.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="projection"></param>
		/// <param name="view"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public Point3D Unproject(Point3D source, Matrix3D projection, Matrix3D view, Matrix3D world)
		{
			var position = new Point3D();
			Matrix3D matrix = Matrix3D.Invert(world * view * projection);
			position.X = (((source.X - X) / Width) * 2) - 1;
			position.Y = -((((source.Y - Y) / Height) * 2) - 1);
			position.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);
			position = Point3D.Transform(position, matrix);
			double a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
			if (!WithinEpsilon(a, 1))
				position = position / a;
			return position;
		}

		public double AspectRatio
		{
			get
			{
				if (Height != 0 && Width != 0)
					return Width / (double)Height;
				return 0;
			}
		}

		public static explicit operator Box2D(Viewport3D value)
		{
			return new Box2D(new IntPoint2D(value.X, value.Y),
				new IntPoint2D(value.X + value.Width, value.Y + value.Height));
		}
	}
}