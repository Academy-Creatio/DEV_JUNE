using Common.Logging;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;

namespace SharedFunctions
{
	[DefaultBinding(typeof(API.ILogger))]
	public class Logger : API.ILogger
	{
		private readonly ILog _logger;
		public Logger(string loggerKey)
		{
			_logger = LogManager.GetLogger(loggerKey);
		}

		public ILog GetLogger()
		{
			return _logger;
		}
	}

	

	
} 