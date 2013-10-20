namespace Nexus.Objects3D
{
	public struct RayDifferential3D
	{
		public const double EPSILON = 1e-3;

		public Point3D Origin;
		public Vector3D Direction;

		public double MinT;
		public double MaxT;
		public double Time;

		public bool HasDifferentials;
		public Ray3D RayX, RayY;

		public RayDifferential3D(Point3D origin, Vector3D direction, double start = EPSILON, double end = double.MaxValue, double time = 0.0)
		{
			this.Origin = origin;
			this.Direction = direction;
			MinT = start;
			MaxT = end;
			Time = time;

			HasDifferentials = false;

			RayX = new Ray3D();
			RayY = new Ray3D();
		}

		public Point3D Evaluate(double t)
		{
			return Origin + Direction * t;
		}

		public override string ToString()
		{
			return string.Format("{{Origin:{0} Direction:{1}}}", this.Origin, this.Direction);
		}
	}
}