using Common.Logging;

namespace SharedFunctions.API
{
	/// <summary>
	/// Provides Common.Logging standard features
	/// <para>
	/// <b>MAKE SURE TO PASS UserConnection as the first argument to the constructor</b>
	/// </para>
	/// </summary>
	/// <example>
	/// <code>
	/// ClassFactory.Get<API.ILogger>(new ConstructorArgument("loggerKey", "GuidedLearningLogger"));
	/// </code>
	/// </example>
	public interface ILogger
	{
		/// <summary>
		/// Gets and instance of a a logger;
		/// </summary>
		/// <returns>Instance of a logger</returns>
		/// <remarks>
		/// Use constructor to specify loggerKey
		/// </remarks>
		ILog GetLogger();
	}
}
