using NUnit.Framework;

namespace Nexus.Tests
{
	[TestFixture]
	public class MathUtilityTests
	{
		[Test]
		public void CanClampFloatValueInsideRange()
		{
			double result = MathUtility.Clamp(1.0, 0.0, 2.0);
			Assert.AreEqual(1.0, result);
		}

		[Test]
		public void CanClampFloatValueBelowRange()
		{
			double result = MathUtility.Clamp(-3.0, 0.0, 2.0);
			Assert.AreEqual(0.0, result);
		}

		[Test]
		public void CanClampFloatValueAboveRange()
		{
			double result = MathUtility.Clamp(3.0, 0.0, 2.0);
			Assert.AreEqual(2.0, result);
		}

		[Test]
		public void CanClampIntValueInsideRange()
		{
			int result = MathUtility.Clamp(1, 0, 2);
			Assert.AreEqual(1, result);
		}

		[Test]
		public void CanClampIntValueBelowRange()
		{
			int result = MathUtility.Clamp(-3, 0, 2);
			Assert.AreEqual(0, result);
		}

		[Test]
		public void CanClampIntValueAboveRange()
		{
			int result = MathUtility.Clamp(3, 0, 2);
			Assert.AreEqual(2, result);
		}
	}
}