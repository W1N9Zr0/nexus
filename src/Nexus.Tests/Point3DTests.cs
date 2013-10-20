using NUnit.Framework;

namespace Nexus.Tests
{
	[TestFixture]
	public class Point3DTests
	{
		[Test]
		public void CanConstructPoint3D()
		{
			const double value = 3.0;
			Point3D point = new Point3D(value, value, value);
			Assert.AreEqual(value, point.X);
			Assert.AreEqual(value, point.Y);
			Assert.AreEqual(value, point.Z);
		}
	}
}