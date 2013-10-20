namespace Nexus.Graphics.Transforms
{
	public class TranslateTransform : AffineTransform
	{
		public double OffsetX { get; set; }
		public double OffsetY { get; set; }
		public double OffsetZ { get; set; }

		public override Matrix3D Value
		{
			get { return Matrix3D.CreateTranslation(OffsetX, OffsetY, OffsetZ); }
		}
	}
}