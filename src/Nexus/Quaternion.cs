using System;

namespace Nexus
{
	public struct Quaternion
	{
		public double X;
		public double Y;
		public double Z;
		public double W;

		public Quaternion(double x, double y, double z, double w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Quaternion(Vector3D axis, double angle)
		{
			axis.Normalize();
			double num2 = angle * 0.5;
			double num = MathUtility.Sin(num2);
			double num3 = MathUtility.Cos(num2);
			X = axis.X * num;
			Y = axis.Y * num;
			Z = axis.Z * num;
			W = num3;
		}

		public void Normalize()
		{
			double lengthSq = (X * X) + (Y * Y) + (Z * Z) + (W * W);
			double inverseLength = 1 / MathUtility.Sqrt(lengthSq);
			X *= inverseLength;
			Y *= inverseLength;
			Z *= inverseLength;
			W *= inverseLength;
		}

		#region Properties

		public bool IsIdentity
		{
			get { return X == 0.0 && Y == 0.0 && Z == 0.0 && W == 1.0; }
		}

		/// <summary>
		/// http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm
		/// </summary>
		public double Angle
		{
			get
			{
				if (IsIdentity)
					return 0.0;

				double y = MathUtility.Sqrt((X * X) + (Y * Y) + (Z * Z));
				double x = W;
				if (y > double.MaxValue)
				{
					double num = System.Math.Max(System.Math.Abs(X), System.Math.Max(System.Math.Abs(Y), System.Math.Abs(Z)));
					double num5 = X / num;
					double num4 = Y / num;
					double num3 = Z / num;
					y = MathUtility.Sqrt((num5 * num5) + (num4 * num4) + (num3 * num3));
					x = W / num;
				}
				return MathUtility.Atan2(y, x);
			}
		}

		public Vector3D Axis
		{
			get
			{
				if (IsIdentity)
					return new Vector3D(0.0, 1.0, 0.0);
				Vector3D vector = new Vector3D(X, Y, Z);
				vector.Normalize();
				return vector;
			}
		}

		#endregion

		#region Operators

		public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion quaternion;
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double num4 = quaternion2.X;
			double num3 = quaternion2.Y;
			double num2 = quaternion2.Z;
			double num = quaternion2.W;
			double num12 = (y * num2) - (z * num3);
			double num11 = (z * num4) - (x * num2);
			double num10 = (x * num3) - (y * num4);
			double num9 = ((x * num4) + (y * num3)) + (z * num2);
			quaternion.X = ((x * num) + (num4 * w)) + num12;
			quaternion.Y = ((y * num) + (num3 * w)) + num11;
			quaternion.Z = ((z * num) + (num2 * w)) + num10;
			quaternion.W = (w * num) - num9;
			return quaternion;
		}

		public static Quaternion operator *(Quaternion quaternion1, double scaleFactor)
		{
			Quaternion quaternion;
			quaternion.X = quaternion1.X * scaleFactor;
			quaternion.Y = quaternion1.Y * scaleFactor;
			quaternion.Z = quaternion1.Z * scaleFactor;
			quaternion.W = quaternion1.W * scaleFactor;
			return quaternion;
		}

		public static bool operator ==(Quaternion left, Quaternion right)
		{
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;
		}

		public static bool operator !=(Quaternion left, Quaternion right)
		{
			return !(left == right);
		}

		#endregion

		#region Static stuff

		public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

		public static Quaternion CreateFromYawPitchRoll(double yaw, double pitch, double roll)
		{
			Quaternion quaternion;
			double num9 = roll * 0.5;
			double num6 = MathUtility.Sin(num9);
			double num5 = MathUtility.Cos(num9);
			double num8 = pitch * 0.5;
			double num4 = MathUtility.Sin(num8);
			double num3 = MathUtility.Cos(num8);
			double num7 = yaw * 0.5;
			double num2 = MathUtility.Sin(num7);
			double num = MathUtility.Cos(num7);
			quaternion.X = ((num * num4) * num5) + ((num2 * num3) * num6);
			quaternion.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
			quaternion.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
			quaternion.W = ((num * num3) * num5) + ((num2 * num4) * num6);
			return quaternion;
		}

		public static Quaternion CreateFromAxisAngle(Vector3D axis, double angle)
		{
			Quaternion quaternion;
			double num2 = angle * 0.5;
			double num = MathUtility.Sin(num2);
			double num3 = MathUtility.Cos(num2);
			quaternion.X = axis.X * num;
			quaternion.Y = axis.Y * num;
			quaternion.Z = axis.Z * num;
			quaternion.W = num3;
			return quaternion;
		}

		public static Quaternion CreateFromRotationMatrix(Matrix3D matrix)
		{
			double num8 = (matrix.M11 + matrix.M22) + matrix.M33;
			Quaternion quaternion = new Quaternion();
			if (num8 > 0)
			{
				double num = (double)Math.Sqrt((double)(num8 + 1));
				quaternion.W = num * 0.5;
				num = 0.5 / num;
				quaternion.X = (matrix.M23 - matrix.M32) * num;
				quaternion.Y = (matrix.M31 - matrix.M13) * num;
				quaternion.Z = (matrix.M12 - matrix.M21) * num;
				return quaternion;
			}
			if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
			{
				double num7 = (double)Math.Sqrt((double)(((1 + matrix.M11) - matrix.M22) - matrix.M33));
				double num4 = 0.5 / num7;
				quaternion.X = 0.5 * num7;
				quaternion.Y = (matrix.M12 + matrix.M21) * num4;
				quaternion.Z = (matrix.M13 + matrix.M31) * num4;
				quaternion.W = (matrix.M23 - matrix.M32) * num4;
				return quaternion;
			}
			if (matrix.M22 > matrix.M33)
			{
				double num6 = (double)Math.Sqrt((double)(((1 + matrix.M22) - matrix.M11) - matrix.M33));
				double num3 = 0.5 / num6;
				quaternion.X = (matrix.M21 + matrix.M12) * num3;
				quaternion.Y = 0.5 * num6;
				quaternion.Z = (matrix.M32 + matrix.M23) * num3;
				quaternion.W = (matrix.M31 - matrix.M13) * num3;
				return quaternion;
			}
			double num5 = (double)Math.Sqrt((double)(((1 + matrix.M33) - matrix.M11) - matrix.M22));
			double num2 = 0.5 / num5;
			quaternion.X = (matrix.M31 + matrix.M13) * num2;
			quaternion.Y = (matrix.M32 + matrix.M23) * num2;
			quaternion.Z = 0.5 * num5;
			quaternion.W = (matrix.M12 - matrix.M21) * num2;

			return quaternion;
		}

		#endregion

		public static bool IsNan(Quaternion q)
		{
			return double.IsNaN(q.X) || double.IsNaN(q.Y) || double.IsNaN(q.Z) || double.IsNaN(q.W);
		}
	}
}