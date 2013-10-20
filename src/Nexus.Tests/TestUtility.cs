using NUnit.Framework;

namespace Nexus.Tests
{
	internal static class TestUtility
	{
		public static void AssertAreRoughlyEqual(double expectedValue, double actualValue)
		{
			if (System.Math.Abs(expectedValue - actualValue) > 0.01)
				Assert.Fail("Expected value: " + expectedValue + "; actual value: " + actualValue);
		}
	}
}