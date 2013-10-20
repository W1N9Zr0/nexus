﻿namespace Nexus.NumericalAnalysis
{
	/// <summary>
	/// Solves an ODE using Euler integration
	/// </summary>
	public class EulerOdeSolver : OdeSolver
	{
		public EulerOdeSolver(int dimensions, CalculateDerivatives callback)
			: base(dimensions, callback)
		{
		}

		public override double[] Solve(double[] initial, double x, double h)
		{
			// final = initial + derived * h
			double[] derivs = Callback(initial, x);
			return DoEulerStep(initial, derivs, h);
		}
	}
}