using SharedFunctions.API;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Common.Logging;

namespace SharedFunctions
{
	[DefaultBinding(typeof(ISearcher))]
	public class Searcher : ISearcher
	{
		private readonly UserConnection _userConnection;
		private readonly ILog _logger;

		public Searcher(UserConnection userConnection)
		{
			_userConnection = userConnection;
			_logger  = ClassFactory.Get<API.ILogger>(new ConstructorArgument("loggerKey", "GuidedLearningLogger")).GetLogger();
		}

		public string DoSearch()
		{
			_logger.Info("My Log message through factory");
			_logger.Warn("My Log message through factory");
			_logger.Error("My Log message through factory");
			return _userConnection.CurrentUser.Name;
		}
	}
}
