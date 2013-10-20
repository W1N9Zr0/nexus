namespace Nexus.Graphics.Transforms
{
	public class ScaleTransform3D : AffineTransform
	{
		public double ScaleX
		{
			get;
			set;
		}

		public double ScaleY
		{
			get;
			set;
		}

		public double ScaleZ
		{
			get;
			set;
		}

		public override Matrix3D Value
		{
			get { return Matrix3D.CreateScale(this.ScaleX, this.ScaleY, this.ScaleZ); }
		}

		public ScaleTransform3D()
		{
			ScaleX = ScaleY = ScaleZ = 1;
		}
	}
}