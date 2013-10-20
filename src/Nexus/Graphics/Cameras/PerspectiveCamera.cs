using Nexus.Objects2D;
using Nexus.Objects3D;

namespace Nexus.Graphics.Cameras
{
	/// <summary>
	/// Represents a perspective projection camera. 
	/// </summary>
	/// <remarks>
	/// PerspectiveCamera specifies a projection of a 3-D model to a 2-D visual surface. This projection includes perspective foreshortening. 
	/// In other words, the PerspectiveCamera describes a frustrum whose sides converge toward a point on the horizon. Objects closer to 
	/// the camera appear larger, and objects farther from the camera appear smaller.
	/// </remarks>
	public class PerspectiveCamera : ProjectionCamera
	{
		public PerspectiveCamera()
		{
			FieldOfView = MathUtility.PI_OVER_4;
		}

		/// <summary>
		/// Gets or sets a value that represents the camera's horizontal field of view in radians. 
		/// </summary>
		public double FieldOfView { get; set; }

		public override Matrix3D GetProjectionMatrix(double aspectRatio)
		{
			return Matrix3D.CreatePerspectiveFieldOfView(FieldOfView,
				aspectRatio, NearPlaneDistance, FarPlaneDistance);
		}

		public static PerspectiveCamera CreateFromBounds(AxisAlignedBox3D bounds, Viewport3D viewport,
			double fieldOfView, double yaw = 0.0, double pitch = 0.0, double zoom = 1.0)
		{
			// Calculate initial guess at camera settings.
			Matrix3D transform = Matrix3D.CreateFromYawPitchRoll(yaw, pitch, 0);
			Vector3D cameraDirection = Vector3D.Normalize(transform.Transform(Vector3D.Forward));
			PerspectiveCamera initialGuess = new PerspectiveCamera
			{
				FieldOfView = fieldOfView,
				NearPlaneDistance = 1.0,
				FarPlaneDistance = bounds.Size.Length() * 10,
				Position = bounds.Center - cameraDirection * bounds.Size.Length() * 2,
				LookDirection = cameraDirection,
				UpDirection = Vector3D.Up
			};

			Matrix3D projection = initialGuess.GetProjectionMatrix(viewport.AspectRatio);
			Matrix3D view = initialGuess.GetViewMatrix();

			// Project bounding box corners onto screen, and calculate screen bounds.
			double closestZ = double.MaxValue;
			Box2D? screenBounds = null;
			Point3D[] corners = bounds.GetCorners();
			foreach (Point3D corner in corners)
			{
				Point3D screenPoint = viewport.Project(corner,
					projection, view, Matrix3D.Identity);

				if (screenPoint.Z < closestZ)
					closestZ = screenPoint.Z;

				IntPoint2D intScreenPoint = new IntPoint2D((int) screenPoint.X, (int) screenPoint.Y);
				if (screenBounds == null)
					screenBounds = new Box2D(intScreenPoint, intScreenPoint);
				else
				{
					Box2D value = screenBounds.Value;
					value.Expand(intScreenPoint);
					screenBounds = value;
				}
			}

			// Now project back from screen bounds into scene, setting Z to the minimum bounding box Z value.
			IntPoint2D minScreen = screenBounds.Value.Min;
			IntPoint2D maxScreen = screenBounds.Value.Max;
			Point3D min = viewport.Unproject(new Point3D(minScreen.X, minScreen.Y, closestZ),
				projection, view, Matrix3D.Identity);
			Point3D max = viewport.Unproject(new Point3D(maxScreen.X, maxScreen.Y, closestZ),
				projection, view, Matrix3D.Identity);

			// Use these new values to calculate the distance the camera should be from the AABB centre.
			Vector3D size = Vector3D.Abs(max - min);
			double radius = size.Length();
			double dist = radius / (2 * MathUtility.Tan(fieldOfView * viewport.AspectRatio / 2));

			Point3D closestBoundsCenter = (min + (max - min) / 2);
			Point3D position = closestBoundsCenter - cameraDirection * dist * (1 / zoom);

			return new PerspectiveCamera
			{
				FieldOfView = fieldOfView,
				NearPlaneDistance = 1.0,
				FarPlaneDistance = dist * 10,
				Position = position,
				LookDirection = cameraDirection,
				UpDirection = Vector3D.Up
			};
		}
	}
}