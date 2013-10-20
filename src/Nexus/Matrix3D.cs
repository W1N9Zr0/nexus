using System;
using System.Runtime.InteropServices;

namespace Nexus
{
	/// <summary>
	/// A structure representing a homogeneous (4x4) 3D matrix.
	/// </summary>
	/// <remarks>
	/// Apollo uses a right-handed coordinate system, which means the viewer is looking along the  
	/// negative z-axis, with the y-axis pointing up and the x-axis to the right.
	/// </remarks>
	/// <seealso cref="http://wiki.oriontransfer.org/blog:2009:05:09:the_matrix"/>
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix3D : IEquatable<Matrix3D>
	{
		private readonly static Matrix3D _identity;

		public double M11;
		public double M12;
		public double M13;
		public double M14;
		public double M21;
		public double M22;
		public double M23;
		public double M24;
		public double M31;
		public double M32;
		public double M33;
		public double M34;
		public double M41;
		public double M42;
		public double M43;
		public double M44;

		#region Constructors

		static Matrix3D()
		{
			_identity = CreateIdentity();
		}

		public Matrix3D(
			double m11, double m12, double m13, double m14,
			double m21, double m22, double m23, double m24,
			double m31, double m32, double m33, double m34,
			double m41, double m42, double m43, double m44)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = m14;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = m24;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = m34;
			M41 = m41;
			M42 = m42;
			M43 = m43;
			M44 = m44;
		}

		#endregion

		#region Properties

		public static Matrix3D Identity
		{
			get { return _identity; }
		}

		private bool IsDistinguishedIdentity
		{
			get
			{
				return M11 == 1.0 && M12 == 0.0 && M13 == 0.0 && M14 == 0.0
				       && M21 == 0.0 && M22 == 1.0 && M23 == 0.0 && M24 == 0.0
				       && M31 == 0.0 && M32 == 0.0 && M33 == 1.0 && M34 == 0.0
				       && M41 == 0.0 && M42 == 0.0 && M43 == 0.0 && M44 == 1.0;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this Matrix3D structure is affine.
		/// </summary>
		/// <remarks>
		/// Transforms that incorporate translation as well as scaling, rotation, and shear are known as affine transforms.
		/// When using affine transformations, the homogeneous component of a coordinate vector (normally called w) 
		/// will never be altered. One can therefore safely assume that it is always 1 and ignore it.
		/// However, this is not true when using perspective projections.
		/// </remarks>
		public bool IsAffine
		{
			get { return (IsDistinguishedIdentity || (M14 == 0.0 && M24 == 0.0 && M34 == 0.0 && M44 == 1.0)); }
		}

		public bool HasInverse
		{
			get { return !MathUtility.IsZero(this.Determinant); }
		}

		public double Determinant
		{
			get
			{
				if (IsDistinguishedIdentity)
					return 1.0;

				if (this.IsAffine)
					return this.GetNormalizedAffineDeterminant();

				double num6 = (this.M13 * this.M24) - (this.M23 * this.M14);
				double num5 = (this.M13 * this.M34) - (this.M33 * this.M14);
				double num4 = (this.M13 * this.M44) - (this.M43 * this.M14);
				double num3 = (this.M23 * this.M34) - (this.M33 * this.M24);
				double num2 = (this.M23 * this.M44) - (this.M43 * this.M24);
				double num = (this.M33 * this.M44) - (this.M43 * this.M34);
				double num10 = ((this.M22 * num5) - (this.M32 * num6)) - (this.M12 * num3);
				double num9 = ((this.M12 * num2) - (this.M22 * num4)) + (this.M42 * num6);
				double num8 = ((this.M32 * num4) - (this.M42 * num5)) - (this.M12 * num);
				double num7 = ((this.M22 * num) - (this.M32 * num2)) + (this.M42 * num3);
				return ((((this.M41 * num10) + (this.M31 * num9)) + (this.M21 * num8)) + (this.M11 * num7));
			}
		}

		public bool SwapsHandedness
		{
			get
			{
				double det = ((M11 *
					(M22 * M33 -
					 M23 * M32)) -
				 (M12 *
					(M21 * M33 -
					 M23 * M31)) +
				 (M13 *
					(M21 * M32 -
					 M22 * M31)));
				return det < 0.0;
			}
		}

		public bool HasScale
		{
			get
			{
#if false
				double det = Math.Abs(M11 * (M22 * M33 - M23 * M32)) -
					(M12 * (M21 * M33 - M23 * M31)) +
					(M13 * (M21 * M32 - M22 * M31));
				return (det < 0.999 || det > 1.001);
#endif
				return false;
			}
		}

		public Vector3D Translation
		{
			get
			{
				Vector3D vector;
				vector.X = M41;
				vector.Y = M42;
				vector.Z = M43;
				return vector;
			}
			set
			{
				M41 = value.X;
				M42 = value.Y;
				M43 = value.Z;
			}
		}

		public Vector3D Forward
		{
			get { return new Vector3D(-M31, -M32, -M33); }
		}

		#endregion

		#region Indexer

		public double this[int column, int row]
		{
			get
			{
				return this[(row * 4) + column];
			}
			set
			{
				this[(row * 4) + column] = value;
			}
		}

		public double this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.M11;

					case 1:
						return this.M12;

					case 2:
						return this.M13;

					case 3:
						return this.M14;

					case 4:
						return this.M21;

					case 5:
						return this.M22;

					case 6:
						return this.M23;

					case 7:
						return this.M24;

					case 8:
						return this.M31;

					case 9:
						return this.M32;

					case 10:
						return this.M33;

					case 11:
						return this.M34;

					case 12:
						return this.M41;

					case 13:
						return this.M42;

					case 14:
						return this.M43;

					case 15:
						return this.M44;
				}
				throw new IndexOutOfRangeException("Invalid matrix index!");
			}
			set
			{
				switch (index)
				{
					case 0:
						this.M11 = value;
						break;

					case 1:
						this.M12 = value;
						break;

					case 2:
						this.M13 = value;
						break;

					case 3:
						this.M14 = value;
						break;

					case 4:
						this.M21 = value;
						break;

					case 5:
						this.M22 = value;
						break;

					case 6:
						this.M23 = value;
						break;

					case 7:
						this.M24 = value;
						break;

					case 8:
						this.M31 = value;
						break;

					case 9:
						this.M32 = value;
						break;

					case 10:
						this.M33 = value;
						break;

					case 11:
						this.M34 = value;
						break;

					case 12:
						this.M41 = value;
						break;

					case 13:
						this.M42 = value;
						break;

					case 14:
						this.M43 = value;
						break;

					case 15:
						this.M44 = value;
						break;

					default:
						throw new IndexOutOfRangeException("Invalid matrix index.");
				}
			}
		}

		#endregion

		#region Methods

		public bool Equals(Matrix3D other)
		{
			return ((((((this.M11 == other.M11) && (this.M22 == other.M22)) && ((this.M33 == other.M33) && (this.M44 == other.M44))) && (((this.M12 == other.M12) && (this.M13 == other.M13)) && ((this.M14 == other.M14) && (this.M21 == other.M21)))) && ((((this.M23 == other.M23) && (this.M24 == other.M24)) && ((this.M31 == other.M31) && (this.M32 == other.M32))) && (((this.M34 == other.M34) && (this.M41 == other.M41)) && (this.M42 == other.M42)))) && (this.M43 == other.M43));
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Matrix3D)
				flag = Equals((Matrix3D) obj);
			return flag;
		}

		public override int GetHashCode()
		{
			return (((((((((((((((this.M11.GetHashCode() + this.M12.GetHashCode()) + this.M13.GetHashCode()) + this.M14.GetHashCode()) + this.M21.GetHashCode()) + this.M22.GetHashCode()) + this.M23.GetHashCode()) + this.M24.GetHashCode()) + this.M31.GetHashCode()) + this.M32.GetHashCode()) + this.M33.GetHashCode()) + this.M34.GetHashCode()) + this.M41.GetHashCode()) + this.M42.GetHashCode()) + this.M43.GetHashCode()) + this.M44.GetHashCode());
		}

		public void Invert()
		{
			if (!InvertCore())
				throw new InvalidOperationException("Matrix is not invertible");
		}

		public void Transform(Point4D[] points)
		{
			if (points != null)
				for (int i = 0; i < points.Length; i++)
					MultiplyPoint(ref points[i]);
		}

		public Point4D Transform(Point4D point)
		{
			MultiplyPoint(ref point);
			return point;
		}

		public void Transform(Point3D[] points)
		{
			if (points != null)
				for (int i = 0; i < points.Length; i++)
					MultiplyPoint(ref points[i]);
		}

		public Point3D Transform(Point3D point)
		{
			this.MultiplyPoint(ref point);
			return point;
		}

		public void Transform(Vector3D[] vectors)
		{
			if (vectors != null)
				for (int i = 0; i < vectors.Length; i++)
					MultiplyVector(ref vectors[i]);
		}

		public Vector3D Transform(Vector3D vector)
		{
			MultiplyVector(ref vector);
			return vector;
		}

		private void MultiplyPoint(ref Point4D point)
		{
			if (!this.IsDistinguishedIdentity)
			{
				double x = point.X;
				double y = point.Y;
				double z = point.Z;
				double w = point.W;
				point.X = (((x * this.M11) + (y * this.M21)) + (z * this.M31)) + (w * this.M41);
				point.Y = (((x * this.M12) + (y * this.M22)) + (z * this.M32)) + (w * this.M42);
				point.Z = (((x * this.M13) + (y * this.M23)) + (z * this.M33)) + (w * this.M43);
				point.W = (((x * this.M14) + (y * this.M24)) + (z * this.M34)) + (w * this.M44);
			}
		}

		private void MultiplyPoint(ref Point3D point)
		{
			if (!IsDistinguishedIdentity)
			{
				double x = point.X;
				double y = point.Y;
				double z = point.Z;
				point.X = (((x * M11) + (y * M21)) + (z * M31)) + M41;
				point.Y = (((x * M12) + (y * M22)) + (z * M32)) + M42;
				point.Z = (((x * M13) + (y * M23)) + (z * M33)) + M43;
				if (!IsAffine)
				{
					double w = (((x * M14) + (y * M24)) + (z * M34)) + M44;
					point.X /= w;
					point.Y /= w;
					point.Z /= w;
				}
			}
		}

		private void MultiplyVector(ref Vector3D vector)
		{
			if (!this.IsDistinguishedIdentity)
			{
				double x = vector.X;
				double y = vector.Y;
				double z = vector.Z;
				vector.X = ((x * this.M11) + (y * this.M21)) + (z * this.M31);
				vector.Y = ((x * this.M12) + (y * this.M22)) + (z * this.M32);
				vector.Z = ((x * this.M13) + (y * this.M23)) + (z * this.M33);
			}
		}

		public override string ToString()
		{
			return ("{ " + string.Format("{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", M11, M12, M13, M14)
			        + string.Format("{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", M21, M22, M23, M24)
			        + string.Format("{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", M31, M32, M33, M34)
			        + string.Format("{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", M41, M42, M43, M44) + "}");
		}

		private static Matrix3D CreateIdentity()
		{
			return new Matrix3D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
		}

		#region Static Create* methods

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// In orthographic projection, all the lines of projection are perpendicular to the eventual drawing surface.
		/// </summary>
		/// <seealso cref="http://www.codeguru.com/cpp/misc/misc/math/article.php/c10123__2/"/>
		/// <param name="width">Width in pixels of the view volume.</param>
		/// <param name="height">Height in pixels of the view volume.</param>
		/// <param name="zNearPlane">Minimum z-value of the view volume.</param>
		/// <param name="zFarPlane">Maximum z-value of the view volume.</param>
		/// <returns>The created projection matrix with normalized device coordinates in the range  (�1, �1, 0) to (1, 1, 1).</returns>
		public static Matrix3D CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane)
		{
			/*Matrix3D matrix1 = CreateScale(2 / width, 2 / height, 1 / (zNearPlane - zFarPlane));
			Matrix3D matrix2 = CreateTranslation(0, 0, zNearPlane);
			return matrix1 * matrix2;*/

			Matrix3D matrix;
			matrix.M11 = 2 / width;
			matrix.M12 = matrix.M13 = matrix.M14 = 0;
			matrix.M22 = 2 / height;
			matrix.M21 = matrix.M23 = matrix.M24 = 0;
			matrix.M33 = 1 / (zNearPlane - zFarPlane);
			matrix.M31 = matrix.M32 = matrix.M34 = 0;
			matrix.M41 = matrix.M42 = 0;
			matrix.M43 = zNearPlane / (zNearPlane - zFarPlane);
			matrix.M44 = 1;
			return matrix;
		}

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// In orthographic projection, all the lines of projection are perpendicular to the eventual drawing surface.
		/// You can almost always use <see cref="CreateOrthographic"/> instead of <see cref="CreateOrthographicOffCenter"/>,
		/// unless you're doing something strange with your projection.
		/// </summary>
		/// <seealso cref="http://www.codeguru.com/cpp/misc/misc/math/article.php/c10123__2/"/>
		/// <param name="left">Minimum x-value of the view volume.</param>
		/// <param name="right">Maximum x-value of the view volume.</param>
		/// <param name="bottom">Minimum y-value of the view volume.</param>
		/// <param name="top">Maximum y-value of the view volume.</param>
		/// <param name="zNearPlane">Minimum z-value of the view volume.</param>
		/// <param name="zFarPlane">Maximum z-value of the view volume.</param>
		/// <returns>The created projection matrix with normalized device coordinates in the range  (�1, �1, 0) to (1, 1, 1).</returns>
		public static Matrix3D CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
		{
			/*return new Matrix3D(
				2 / (right - left), 0, 0, 0,
				0, 2 / (top - bottom), 0, 0,
				0, 0, 1 / (zNearPlane - zFarPlane), 0,
				(left + right) / (left - right), (bottom + top) / (bottom - top), zNearPlane / (zNearPlane - zFarPlane), 1);*/

			Matrix3D matrix;
			matrix.M11 = 2 / (right - left);
			matrix.M12 = matrix.M13 = matrix.M14 = 0;
			matrix.M22 = 2 / (top - bottom);
			matrix.M21 = matrix.M23 = matrix.M24 = 0;
			matrix.M33 = 1 / (zNearPlane - zFarPlane);
			matrix.M31 = matrix.M32 = matrix.M34 = 0;
			matrix.M41 = (left + right) / (left - right);
			matrix.M42 = (top + bottom) / (bottom - top);
			matrix.M43 = zNearPlane / (zNearPlane - zFarPlane);
			matrix.M44 = 1;
			return matrix;
		}

		/// <summary>
		/// Builds a perspective projection matrix based on a field of view and the near and far plane distances.
		/// Geometrically speaking, the difference between this method and <see cref="CreateOrthographic" /> or 
		/// <see cref="CreateOrthographicOffCenter" /> is that in perspective projection, 
		/// the view volume is a frustum�that is, a truncated pyramid�rather than an axis-aligned box.
		/// This transformation combines a perspective distortion with a depth (z) transformation. The perspective
		/// assumes the eye is at the origin, looking down the +z axis. The matrix transforms
		/// <paramref name="nearPlaneDistance"/> to +0, and <paramref name="farPlaneDistance"/> to +1.
		/// </summary>
		/// <seealso cref="http://www.codeguru.com/cpp/misc/misc/math/article.php/c10123__3/"/>
		/// <param name="fieldOfView">Field of view in radians.</param>
		/// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
		/// <param name="nearPlaneDistance">Distance to the near view plane.</param>
		/// <param name="farPlaneDistance">Distance to the far view plane.</param>
		/// <returns>The created projection matrix with normalized device coordinates in the range  (�1, �1, 0) to (1, 1, 1).</returns>
		public static Matrix3D CreatePerspectiveFieldOfView(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
		{
			// Validate arguments.
			if ((fieldOfView <= 0) || (fieldOfView >= 3.141593))
				throw new ArgumentOutOfRangeException("fieldOfView");
			if (nearPlaneDistance <= 0)
				throw new ArgumentOutOfRangeException("nearPlaneDistance");
			if (farPlaneDistance <= 0)
				throw new ArgumentOutOfRangeException("farPlaneDistance");
			if (nearPlaneDistance >= farPlaneDistance)
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "Far plane distance must be larger than near plane distance.");

			double t = MathUtility.Tan(fieldOfView / 2.0);
			double nearMinusFar = nearPlaneDistance - farPlaneDistance;

			return new Matrix3D(
				1.0 / t / aspectRatio, 0, 0, 0,
				0, 1.0 / t, 0, 0,
				0, 0, farPlaneDistance / nearMinusFar, -1,
				0, 0, (nearPlaneDistance * farPlaneDistance) / nearMinusFar, 0);
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of points around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis.</param>
		/// <returns>The created rotation matrix.</returns>
		public static Matrix3D CreateRotationX(double radians)
		{
			double s = MathUtility.Sin(radians);
			double c = MathUtility.Cos(radians);

			return new Matrix3D(
				1, 0, 0, 0,
				0, c, s, 0,
				0, -s, c, 0,
				0, 0, 0, 1);
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of points around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis.</param>
		/// <returns>The created rotation matrix.</returns>
		public static Matrix3D CreateRotationY(double radians)
		{
			double s = MathUtility.Sin(radians);
			double c = MathUtility.Cos(radians);

			return new Matrix3D(
				c, 0, -s, 0,
				0, 1, 0, 0,
				s, 0, c, 0,
				0, 0, 0, 1);
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of points around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis.</param>
		/// <returns>The created rotation matrix.</returns>
		public static Matrix3D CreateRotationZ(double radians)
		{
			double s = MathUtility.Sin(radians);
			double c = MathUtility.Cos(radians);

			return new Matrix3D(
				c, s, 0, 0,
				-s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}

		/// <summary>
		/// Creates a scaling matrix.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param>
		/// <param name="yScale">Value to scale by on the y-axis.</param>
		/// <param name="zScale">Value to scale by on the z-axis.</param>
		/// <returns>The created scaling matrix.</returns>
		public static Matrix3D CreateScale(double xScale, double yScale, double zScale)
		{
			return new Matrix3D(
				xScale, 0, 0, 0,
				0, yScale, 0, 0,
				0, 0, zScale, 0,
				0, 0, 0, 1);
		}

		/// <summary>
		/// Creates a scaling matrix.
		/// </summary>
		/// <param name="scale">Amount to scale by.</param>
		/// <returns>The created scaling matrix.</returns>
		public static Matrix3D CreateScale(double scale)
		{
			return CreateScale(scale, scale, scale);
		}

		public static Matrix3D CreateScale(Vector3D scales)
		{
			return CreateScale(scales.X, scales.Y, scales.Z);
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="xPosition">Value to translate by on the x-axis.</param>
		/// <param name="yPosition">Value to translate by on the y-axis.</param>
		/// <param name="zPosition">Value to translate by on the z-axis.</param>
		/// <returns>The created translation matrix.</returns>
		public static Matrix3D CreateTranslation(double xPosition, double yPosition, double zPosition)
		{
			return new Matrix3D(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				xPosition, yPosition, zPosition, 1);
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="point">Amounts to translate by on the x, y and z axes.</param>
		/// <returns>The created translation matrix.</returns>
		public static Matrix3D CreateTranslation(Point3D point)
		{
			return CreateTranslation(point.X, point.Y, point.Z);
		}

		public static Matrix3D CreateFromAxisAngle(Vector3D axis, double angle)
		{
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double num2 = (double)System.Math.Sin((double)angle);
			double num = (double)System.Math.Cos((double)angle);
			double num11 = x * x;
			double num10 = y * y;
			double num9 = z * z;
			double num8 = x * y;
			double num7 = x * z;
			double num6 = y * z;
			Matrix3D matrix = new Matrix3D(
					num11 + (num * (1 - num11)),
					(num8 - (num * num8)) + (num2 * z),
					(num7 - (num * num7)) - (num2 * y),
					0,
					(num8 - (num * num8)) - (num2 * z),
					num10 + (num * (1 - num10)),
					(num6 - (num * num6)) + (num2 * x),
					0,
					(num7 - (num * num7)) + (num2 * y),
					(num6 - (num * num6)) - (num2 * x),
					num9 + (num * (1 - num9)),
					0,
					0,
					0,
					0,
					1);
			return matrix;
		}

		public static Matrix3D CreateFromQuaternion(Quaternion quaternion)
		{
			Matrix3D matrix = new Matrix3D();
			double num9 = quaternion.X * quaternion.X;
			double num8 = quaternion.Y * quaternion.Y;
			double num7 = quaternion.Z * quaternion.Z;
			double num6 = quaternion.X * quaternion.Y;
			double num5 = quaternion.Z * quaternion.W;
			double num4 = quaternion.Z * quaternion.X;
			double num3 = quaternion.Y * quaternion.W;
			double num2 = quaternion.Y * quaternion.Z;
			double num = quaternion.X * quaternion.W;
			matrix.M11 = 1 - (2 * (num8 + num7));
			matrix.M12 = 2 * (num6 + num5);
			matrix.M13 = 2 * (num4 - num3);
			matrix.M14 = 0;
			matrix.M21 = 2 * (num6 - num5);
			matrix.M22 = 1 - (2 * (num7 + num9));
			matrix.M23 = 2 * (num2 + num);
			matrix.M24 = 0;
			matrix.M31 = 2 * (num4 + num3);
			matrix.M32 = 2 * (num2 - num);
			matrix.M33 = 1 - (2 * (num8 + num9));
			matrix.M34 = 0;
			matrix.M41 = 0;
			matrix.M42 = 0;
			matrix.M43 = 0;
			matrix.M44 = 1;
			return matrix;
		}

		public static Matrix3D CreateLookAt(Point3D cameraPosition, Vector3D lookDirection, Vector3D cameraUpVector)
		{
			Vector3D position = (Vector3D)cameraPosition;

			Vector3D vector = Vector3D.Normalize(-lookDirection);
			Vector3D vector2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector));
			Vector3D vector3 = Vector3D.Cross(vector, vector2);

			Matrix3D matrix = new Matrix3D(
					vector2.X,
					vector3.X,
					vector.X,
					0,
					vector2.Y,
					vector3.Y,
					vector.Y,
					0,
					vector2.Z,
					vector3.Z,
					vector.Z,
					0,
					-Vector3D.Dot(vector2, position),
					-Vector3D.Dot(vector3, position),
					-Vector3D.Dot(vector, position),
					1);
			return matrix;
		}

		#endregion

		public static Matrix3D Invert(Matrix3D matrix)
		{
			double num5 = matrix.M11;
			double num4 = matrix.M12;
			double num3 = matrix.M13;
			double num2 = matrix.M14;
			double num9 = matrix.M21;
			double num8 = matrix.M22;
			double num7 = matrix.M23;
			double num6 = matrix.M24;
			double num17 = matrix.M31;
			double num16 = matrix.M32;
			double num15 = matrix.M33;
			double num14 = matrix.M34;
			double num13 = matrix.M41;
			double num12 = matrix.M42;
			double num11 = matrix.M43;
			double num10 = matrix.M44;
			double num23 = (num15 * num10) - (num14 * num11);
			double num22 = (num16 * num10) - (num14 * num12);
			double num21 = (num16 * num11) - (num15 * num12);
			double num20 = (num17 * num10) - (num14 * num13);
			double num19 = (num17 * num11) - (num15 * num13);
			double num18 = (num17 * num12) - (num16 * num13);
			double num39 = ((num8 * num23) - (num7 * num22)) + (num6 * num21);
			double num38 = -(((num9 * num23) - (num7 * num20)) + (num6 * num19));
			double num37 = ((num9 * num22) - (num8 * num20)) + (num6 * num18);
			double num36 = -(((num9 * num21) - (num8 * num19)) + (num7 * num18));
			double num = 1 / ((((num5 * num39) + (num4 * num38)) + (num3 * num37)) + (num2 * num36));

			double num35 = (num7 * num10) - (num6 * num11);
			double num34 = (num8 * num10) - (num6 * num12);
			double num33 = (num8 * num11) - (num7 * num12);
			double num32 = (num9 * num10) - (num6 * num13);
			double num31 = (num9 * num11) - (num7 * num13);
			double num30 = (num9 * num12) - (num8 * num13);

			double num29 = (num7 * num14) - (num6 * num15);
			double num28 = (num8 * num14) - (num6 * num16);
			double num27 = (num8 * num15) - (num7 * num16);
			double num26 = (num9 * num14) - (num6 * num17);
			double num25 = (num9 * num15) - (num7 * num17);
			double num24 = (num9 * num16) - (num8 * num17);

			Matrix3D matrix2 = new Matrix3D(num39 * num,
					-(((num4 * num23) - (num3 * num22)) + (num2 * num21)) * num,
					(((num4 * num35) - (num3 * num34)) + (num2 * num33)) * num,
					-(((num4 * num29) - (num3 * num28)) + (num2 * num27)) * num,
					num38 * num,
					(((num5 * num23) - (num3 * num20)) + (num2 * num19)) * num,
					-(((num5 * num35) - (num3 * num32)) + (num2 * num31)) * num,
					(((num5 * num29) - (num3 * num26)) + (num2 * num25)) * num,
					num37 * num,
					-(((num5 * num22) - (num4 * num20)) + (num2 * num18)) * num,
					(((num5 * num34) - (num4 * num32)) + (num2 * num30)) * num,
					-(((num5 * num28) - (num4 * num26)) + (num2 * num24)) * num,
					num36 * num,
					(((num5 * num21) - (num4 * num19)) + (num3 * num18)) * num,
					-(((num5 * num33) - (num4 * num31)) + (num3 * num30)) * num,
					(((num5 * num27) - (num4 * num25)) + (num3 * num24)) * num);
			return matrix2;
		}

		public static Matrix3D Transpose(Matrix3D matrix)
		{
			Matrix3D matrix2 = new Matrix3D();
			matrix2.M11 = matrix.M11;
			matrix2.M12 = matrix.M21;
			matrix2.M13 = matrix.M31;
			matrix2.M14 = matrix.M41;
			matrix2.M21 = matrix.M12;
			matrix2.M22 = matrix.M22;
			matrix2.M23 = matrix.M32;
			matrix2.M24 = matrix.M42;
			matrix2.M31 = matrix.M13;
			matrix2.M32 = matrix.M23;
			matrix2.M33 = matrix.M33;
			matrix2.M34 = matrix.M43;
			matrix2.M41 = matrix.M14;
			matrix2.M42 = matrix.M24;
			matrix2.M43 = matrix.M34;
			matrix2.M44 = matrix.M44;
			return matrix2;
		}

		private double GetNormalizedAffineDeterminant()
		{
			double num3 = (this.M12 * this.M23) - (this.M22 * this.M13);
			double num2 = (this.M32 * this.M13) - (this.M12 * this.M33);
			double num = (this.M22 * this.M33) - (this.M32 * this.M23);
			return (((this.M31 * num3) + (this.M21 * num2)) + (this.M11 * num));
		}

		private bool InvertCore()
		{
			if (!IsDistinguishedIdentity)
			{
				if (IsAffine)
					return NormalizedAffineInvert();

				double num7 = (this.M13 * this.M24) - (this.M23 * this.M14);
				double num6 = (this.M13 * this.M34) - (this.M33 * this.M14);
				double num5 = (this.M13 * this.M44) - (this.M43 * this.M14);
				double num4 = (this.M23 * this.M34) - (this.M33 * this.M24);
				double num3 = (this.M23 * this.M44) - (this.M43 * this.M24);
				double num2 = (this.M33 * this.M44) - (this.M43 * this.M34);
				double num12 = ((this.M22 * num6) - (this.M32 * num7)) - (this.M12 * num4);
				double num11 = ((this.M12 * num3) - (this.M22 * num5)) + (this.M42 * num7);
				double num10 = ((this.M32 * num5) - (this.M42 * num6)) - (this.M12 * num2);
				double num9 = ((this.M22 * num2) - (this.M32 * num3)) + (this.M42 * num4);
				double num8 = (((this.M41 * num12) + (this.M31 * num11)) + (this.M21 * num10)) + (this.M11 * num9);

				if (MathUtility.IsZero(num8))
					return false;

				double num24 = ((this.M11 * num4) - (this.M21 * num6)) + (this.M31 * num7);
				double num23 = ((this.M21 * num5) - (this.M41 * num7)) - (this.M11 * num3);
				double num22 = ((this.M11 * num2) - (this.M31 * num5)) + (this.M41 * num6);
				double num21 = ((this.M31 * num3) - (this.M41 * num4)) - (this.M21 * num2);
				num7 = (this.M11 * this.M22) - (this.M21 * this.M12);
				num6 = (this.M11 * this.M32) - (this.M31 * this.M12);
				num5 = (this.M11 * this.M42) - (this.M41 * this.M12);
				num4 = (this.M21 * this.M32) - (this.M31 * this.M22);
				num3 = (this.M21 * this.M42) - (this.M41 * this.M22);
				num2 = (this.M31 * this.M42) - (this.M41 * this.M32);
				double num20 = ((this.M13 * num4) - (this.M23 * num6)) + (this.M33 * num7);
				double num19 = ((this.M23 * num5) - (this.M43 * num7)) - (this.M13 * num3);
				double num18 = ((this.M13 * num2) - (this.M33 * num5)) + (this.M43 * num6);
				double num17 = ((this.M33 * num3) - (this.M43 * num4)) - (this.M23 * num2);
				double num16 = ((this.M24 * num6) - (this.M34 * num7)) - (this.M14 * num4);
				double num15 = ((this.M14 * num3) - (this.M24 * num5)) + (this.M44 * num7);
				double num14 = ((this.M34 * num5) - (this.M44 * num6)) - (this.M14 * num2);
				double num13 = ((this.M24 * num2) - (this.M34 * num3)) + (this.M44 * num4);
				double num = 1.0 / num8;
				this.M11 = num9 * num;
				this.M12 = num10 * num;
				this.M13 = num11 * num;
				this.M14 = num12 * num;
				this.M21 = num21 * num;
				this.M22 = num22 * num;
				this.M23 = num23 * num;
				this.M24 = num24 * num;
				this.M31 = num13 * num;
				this.M32 = num14 * num;
				this.M33 = num15 * num;
				this.M34 = num16 * num;
				this.M41 = num17 * num;
				this.M42 = num18 * num;
				this.M43 = num19 * num;
				this.M44 = num20 * num;
			}
			return true;
		}

		private bool NormalizedAffineInvert()
		{
			double num11 = (this.M12 * this.M23) - (this.M22 * this.M13);
			double num10 = (this.M32 * this.M13) - (this.M12 * this.M33);
			double num9 = (this.M22 * this.M33) - (this.M32 * this.M23);
			double num8 = ((this.M31 * num11) + (this.M21 * num10)) + (this.M11 * num9);
			if (MathUtility.IsZero(num8))
				return false;

			double num20 = (this.M21 * this.M13) - (this.M11 * this.M23);
			double num19 = (this.M11 * this.M33) - (this.M31 * this.M13);
			double num18 = (this.M31 * this.M23) - (this.M21 * this.M33);
			double num7 = (this.M11 * this.M22) - (this.M21 * this.M12);
			double num6 = (this.M11 * this.M32) - (this.M31 * this.M12);
			double num5 = (this.M11 * this.M42) - (this.M41 * this.M12);
			double num4 = (this.M21 * this.M32) - (this.M31 * this.M22);
			double num3 = (this.M21 * this.M42) - (this.M41 * this.M22);
			double num2 = (this.M31 * this.M42) - (this.M41 * this.M32);
			double num17 = ((this.M23 * num5) - (this.M43 * num7)) - (this.M13 * num3);
			double num16 = ((this.M13 * num2) - (this.M33 * num5)) + (this.M43 * num6);
			double num15 = ((this.M33 * num3) - (this.M43 * num4)) - (this.M23 * num2);
			double num14 = num7;
			double num13 = -num6;
			double num12 = num4;
			double num = 1.0 / num8;
			this.M11 = num9 * num;
			this.M12 = num10 * num;
			this.M13 = num11 * num;
			this.M21 = num18 * num;
			this.M22 = num19 * num;
			this.M23 = num20 * num;
			this.M31 = num12 * num;
			this.M32 = num13 * num;
			this.M33 = num14 * num;
			this.M41 = num15 * num;
			this.M42 = num16 * num;
			this.M43 = num17 * num;
			return true;
		}

		// Thanks to MonoGame (https://github.com/mono/MonoGame/blob/develop/MonoGame.Framework/Matrix.cs)
		public bool Decompose(out Vector3D scale, out Quaternion rotation, out Vector3D translation)
		{
			translation.X = this.M41;
			translation.Y = this.M42;
			translation.Z = this.M43;

			double xs = (Math.Sign(M11 * M12 * M13 * M14) < 0) ? -1 : 1;
			double ys = (Math.Sign(M21 * M22 * M23 * M24) < 0) ? -1 : 1;
			double zs = (Math.Sign(M31 * M32 * M33 * M34) < 0) ? -1 : 1;

			scale.X = xs * (double)Math.Sqrt(this.M11 * this.M11 + this.M12 * this.M12 + this.M13 * this.M13);
			scale.Y = ys * (double)Math.Sqrt(this.M21 * this.M21 + this.M22 * this.M22 + this.M23 * this.M23);
			scale.Z = zs * (double)Math.Sqrt(this.M31 * this.M31 + this.M32 * this.M32 + this.M33 * this.M33);

			if (scale.X == 0.0 || scale.Y == 0.0 || scale.Z == 0.0)
			{
				rotation = Quaternion.Identity;
				return false;
			}

			Matrix3D m1 = new Matrix3D(this.M11 / scale.X, M12 / scale.X, M13 / scale.X, 0,
								   this.M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, 0,
								   this.M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, 0,
								   0, 0, 0, 1);

			rotation = Quaternion.CreateFromRotationMatrix(m1);
			return true;
		}

		#endregion

		#region Operators
		
		public static Matrix3D operator *(Matrix3D matrix1, Matrix3D matrix2)
		{
			if (matrix1.IsDistinguishedIdentity)
				return matrix2;

			if (matrix2.IsDistinguishedIdentity)
				return matrix1;

			return new Matrix3D(
				(((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41),
				(((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42),
				(((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43),
				(((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44),
				(((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41),
				(((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42),
				(((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43),
				(((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44),
				(((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41),
				(((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42),
				(((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43),
				(((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44),
				(((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41),
				(((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42),
				(((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43),
				(((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44));
		}

		public static Matrix3D operator *(Matrix3D matrix, double scaleFactor)
		{
			return new Matrix3D(
				matrix.M11 * scaleFactor,
				matrix.M12 * scaleFactor,
				matrix.M13 * scaleFactor,
				matrix.M14 * scaleFactor,
				matrix.M21 * scaleFactor,
				matrix.M22 * scaleFactor,
				matrix.M23 * scaleFactor,
				matrix.M24 * scaleFactor,
				matrix.M31 * scaleFactor,
				matrix.M32 * scaleFactor,
				matrix.M33 * scaleFactor,
				matrix.M34 * scaleFactor,
				matrix.M41 * scaleFactor,
				matrix.M42 * scaleFactor,
				matrix.M43 * scaleFactor,
				matrix.M44 * scaleFactor);
		}

		public static Matrix3D operator +(Matrix3D matrix1, Matrix3D matrix2)
		{
			return new Matrix3D(
				matrix1.M11 + matrix2.M11,
				matrix1.M12 + matrix2.M12,
				matrix1.M13 + matrix2.M13,
				matrix1.M14 + matrix2.M14,
				matrix1.M21 + matrix2.M21,
				matrix1.M22 + matrix2.M22,
				matrix1.M23 + matrix2.M23,
				matrix1.M24 + matrix2.M24,
				matrix1.M31 + matrix2.M31,
				matrix1.M32 + matrix2.M32,
				matrix1.M33 + matrix2.M33,
				matrix1.M34 + matrix2.M34,
				matrix1.M41 + matrix2.M41,
				matrix1.M42 + matrix2.M42,
				matrix1.M43 + matrix2.M43,
				matrix1.M44 + matrix2.M44);
		}

		public static bool operator ==(Matrix3D matrix1, Matrix3D matrix2)
		{
			return matrix1.Equals(matrix2);
		}

		public static bool operator !=(Matrix3D matrix1, Matrix3D matrix2)
		{
			return !matrix1.Equals(matrix2);
		}

		#endregion

		public static Matrix3D CreateFromYawPitchRoll(double yaw, double pitch, double roll)
		{
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
			return CreateFromQuaternion(quaternion);
		}
	}
}