namespace Nexus.NumericalAnalysis
{
	/// <summary>
	/// Solves an ODE using RK2
	/// </summary>
	public class Rk2OdeSolver : OdeSolver
	{
		public Rk2OdeSolver(int dimensions, CalculateDerivatives callback)
			: base(dimensions, callback)
		{
		}

		public override double[] Solve(double[] initial, double x, double h)
		{
			// do Euler step with half the step value
			double[] k1 = Callback(initial, x);
			double[] temp = DoEulerStep(initial, k1, h / 2.0);

			// calculate again at midpoint
			double[] k2 = Callback(temp, x + (h / 2.0));

			// use derivatives for complete timestep
			return DoEulerStep(initial, k2, h);
		}
	}
}