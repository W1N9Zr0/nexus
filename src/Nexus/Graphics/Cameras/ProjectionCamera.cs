namespace Nexus.Graphics.Cameras
{
	/// <summary>
	/// An abstract base class for perspective and orthographic projection cameras. 
	/// </summary>
	public abstract class ProjectionCamera : Camera
	{
		private Vector3D _lookDirection;

		protected ProjectionCamera()
		{
			NearPlaneDistance = 0.125;
			FarPlaneDistance = 10000.0;
			Position = Point3D.Zero;
			_lookDirection = Vector3D.Forward;
			UpDirection = Vector3D.Up;
		}

		/// <summary>
		/// Gets or sets a value that specifies the distance from the camera of the camera's far clip plane.
		/// </summary>
		public double FarPlaneDistance
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a Vector3D which defines the direction in which the camera is looking in world coordinates.
		/// </summary>
		public Vector3D LookDirection
		{
			get { return _lookDirection; }
			set { _lookDirection = Vector3D.Normalize(value); }
		}

		/// <summary>
		/// Gets or sets a value that specifies the distance from the camera of the camera's near clip plane.
		/// </summary>
		public double NearPlaneDistance
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the position of the camera in world coordinates.
		/// </summary>
		public Point3D Position
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a Vector3D which defines the upward direction of the camera.
		/// </summary>
		public Vector3D UpDirection
		{
			get;
			set;
		}

		public override Matrix3D GetViewMatrix()
		{
			return Matrix3D.CreateLookAt(Position, LookDirection, UpDirection);
		}
	}
}