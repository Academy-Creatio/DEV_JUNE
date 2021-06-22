namespace SharedFunctions.API
{
	/// <summary>
	/// Make sure to provide UserConnection in constructor
	/// </summary>
	public interface ISearcher
	{
		/// <summary>
		/// Will return current user name from UserConnection
		/// </summary>
		/// <returns>User name from UserConnection</returns>
		string DoSearch();
	}
}