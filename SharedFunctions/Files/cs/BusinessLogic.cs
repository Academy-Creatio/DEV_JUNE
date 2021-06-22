using Common.Logging;
using SharedFunctions.API;
using System;
using Terrasoft.Core.Factories;

namespace SharedFunctions
{
	[DefaultBinding(typeof(IBusinessLogic), Name ="V1")]
	public class BusinessLogic1 : IBusinessLogic
	{
		private readonly ILog _logger;
		public BusinessLogic1()
		{
			_logger = ClassFactory.Get<API.ILogger>(new ConstructorArgument("loggerKey", "GuidedLearningLogger")).GetLogger();
		}

		public double Add(double a, double b)
		{
			_logger.Info($"added {a} and {b}, returned { a+b }");
			return a + b;
		}
		public double Subtract(double a, double b)
		{
			return a - b;
		}
		public double Multiply(double a, double b)
		{
			return a * b;
		}
		public double Divide(double a, double b)
		{
			if (b == 0)
			{
				throw new ArgumentOutOfRangeException("Cannot divide by zero");
			}

			return a / b;
		}
	}	
	
	[DefaultBinding(typeof(IBusinessLogic), Name ="V2")]
	public class BusinessLogic2 : IBusinessLogic
	{
		public double Add(double a, double b)
		{
			return a + b+10;
		}
		public double Subtract(double a, double b)
		{
			return a - b;
		}
		public double Multiply(double a, double b)
		{
			return a * b;
		}
		public double Divide(double a, double b)
		{
			if (b == 0)
			{
				throw new ArgumentOutOfRangeException("Cannot divide by zero");
			}

			return a / b;
		}
	}
}
