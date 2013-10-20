namespace Nexus.NumericalAnalysis
{
	/// <summary>
	/// Solves an ODE using RK4
	/// </summary>
	public class Rk4OdeSolver : OdeSolver
	{
		public Rk4OdeSolver(int dimensions, CalculateDerivatives callback)
			: base(dimensions, callback)
		{
		}

		public override double[] Solve(double[] initial, double x, double h)
		{
			double[] k1 = Callback(initial, x);
			double[] temp = DoEulerStep(initial, k1, h / 2.0);

			double[] k2 = Callback(temp, x + (h / 2.0));
			temp = DoEulerStep(initial, k2, h / 2.0);

			double[] k3 = Callback(temp, x + (h / 2.0));
			temp = DoEulerStep(initial, k3, h);

			double[] k4 = Callback(temp, x + h);

			double[] ret = DoEulerStep(initial, k1, h / 6.0);
			ret = DoEulerStep(ret, k2, h / 3.0);
			ret = DoEulerStep(ret, k3, h / 3.0);
			ret = DoEulerStep(ret, k4, h / 6.0);
			return ret;
		}
	}
}