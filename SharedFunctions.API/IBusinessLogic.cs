namespace SharedFunctions.API
{

	/// <summary>
	/// Contract for BusinessLogic Implementation
	/// </summary>
	public interface IBusinessLogic
	{
		/// <summary>
		/// Adds two doubles and returns the result
		/// </summary>
		/// <param name="a">First number</param>
		/// <param name="b">Second Number</param>
		/// <returns>Sum of two number (a+b)</returns>
		double Add(double a, double b);

		/// <summary>
		/// Divides a by b
		/// </summary>
		/// <param name="a">First number</param>
		/// <param name="b">Second Number</param>
		/// <returns>a over b (a/b)</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="b"/> is zero</exception>
		double Divide(double a, double b);

		/// <summary>
		/// Multiplies a by b
		/// </summary>
		/// <param name="a">First number</param>
		/// <param name="b">Second Number</param>
		/// <returns>The product of a and b (a*b)</returns>
		double Multiply(double a, double b);


		/// <summary>
		/// Subtracts b from a
		/// </summary>
		/// <param name="a">First number</param>
		/// <param name="b">Second Number</param>
		/// <returns>The difference between a and b (a-b)</returns>
		double Subtract(double a, double b);
	}
}