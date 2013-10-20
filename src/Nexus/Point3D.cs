using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Nexus.Design;
using Nexus.Util;

namespace Nexus
{
	[TypeConverter(typeof(Point3DConverter))]
	[StructLayout(LayoutKind.Sequential)]
	public struct Point3D
	{
		public double X;
		public double Y;
		public double Z;

		public Point3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Point3D(Point2D xy, double z)
			: this(xy.X, xy.Y, z)
		{
			
		}

		#region Properties

		public static Point3D Zero
		{
			get { return new Point3D(0, 0, 0); }
		}

		public static int SizeInBytes
		{
			get { return sizeof(double) * 3; }
		}

		public Point2D Xy
		{
			get { return new Point2D(X, Y); }
		}

		#endregion

		#region Indexer

		public double this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return X;
					case 1 :
						return Y;
					case 2 :
						return Z;
					default :
						throw new ArgumentOutOfRangeException("index");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						X = value;
						break;
					case 1 :
						Y = value;
						break;
					case 2 :
						Z = value;
						break;
					default :
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		#endregion

		#region Methods

		internal string ConvertToString(string format, IFormatProvider provider)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
			return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}", new object[] { numericListSeparator, this.X, this.Y, this.Z });
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Point3D)
				flag = (this == (Point3D) obj);
			return flag;
		}

		public override int GetHashCode()
		{
			return ((X.GetHashCode() + Y.GetHashCode()) + Z.GetHashCode());
		}

		public override string ToString()
		{
			return string.Format("{{X:{0} Y:{1} Z:{2}}}", X, Y, Z);
		}

		public static Point3D Parse(string source)
		{
			IFormatProvider cultureInfo = CultureInfo.InvariantCulture;
			TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
			string str = helper.NextTokenRequired();
			Point3D pointd = new Point3D(Convert.ToSingle(str, cultureInfo), Convert.ToSingle(helper.NextTokenRequired(), cultureInfo), Convert.ToSingle(helper.NextTokenRequired(), cultureInfo));
			helper.LastTokenRequired();
			return pointd;
		}

		public static Point3D Transform(Point3D position, Matrix3D matrix)
		{
			Point3D vector;
			double num3 = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
			double num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
			double num = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
			vector.X = num3;
			vector.Y = num2;
			vector.Z = num;
			return vector;
		}

		public Point4D ToHomogeneousPoint3D()
		{
			return new Point4D(this, 1.0);
		}

		public static double Distance(Point3D p1, Point3D p2)
		{
			return (p1 - p2).Length();
		}

		public static double DistanceSquared(Point3D p1, Point3D p2)
		{
			return (p1 - p2).LengthSquared();
		}

		#endregion

		#region Operators

		public static Point3D operator +(Point3D value1, Point3D value2)
		{
			Point3D vector;
			vector.X = value1.X + value2.X;
			vector.Y = value1.Y + value2.Y;
			vector.Z = value1.Z + value2.Z;
			return vector;
		}

		public static Point3D operator+(Point3D point, Vector3D vector)
		{
			return new Point3D(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
		}

		public static Vector3D operator -(Point3D point1, Point3D point2)
		{
			return new Vector3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
		}

		public static Point3D operator -(Point3D point, Vector3D vector)
		{
			return new Point3D(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
		}

		public static Point3D operator *(Point3D value, double scaleFactor)
		{
			Point3D vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			vector.Z = value.Z * scaleFactor;
			return vector;
		}

		public static Point3D operator *(Point3D value, Vector3D scaleFactors)
		{
			Point3D vector;
			vector.X = value.X * scaleFactors.X;
			vector.Y = value.Y * scaleFactors.Y;
			vector.Z = value.Z * scaleFactors.Z;
			return vector;
		}

		public static Point3D operator *(double scaleFactor, Point3D value)
		{
			Point3D vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			vector.Z = value.Z * scaleFactor;
			return vector;
		}

		public static Point3D operator /(Point3D value, double divider)
		{
			Point3D vector;
			double num = 1 / divider;
			vector.X = value.X * num;
			vector.Y = value.Y * num;
			vector.Z = value.Z * num;
			return vector;
		}

		public static bool operator ==(Point3D value1, Point3D value2)
		{
			return (((value1.X == value2.X) && (value1.Y == value2.Y)) && (value1.Z == value2.Z));
		}

		public static bool operator !=(Point3D value1, Point3D value2)
		{
			if ((value1.X == value2.X) && (value1.Y == value2.Y))
				return (value1.Z != value2.Z);
			return true;
		}

		public static explicit operator Vector3D(Point3D point)
		{
			return new Vector3D(point.X, point.Y, point.Z);
		}

		#endregion

		#region Static methods

		public static void Subtract(ref Point3D value1, ref Point3D value2, out Vector3D result)
		{
			result = new Vector3D(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
		}

		#endregion
	}
}