namespace Terrasoft.Core.Process.Configuration
{

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using SharedFunctions.API;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.UI.WebControls.Controls;

	#region Class: ProcessUserTask_BusinessLogic

	/// <exclude/>
	public partial class ProcessUserTask_BusinessLogic
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var businessLogic = Factories.ClassFactory.Get<IBusinessLogic>("V1");
			var a = Convert.ToDouble(A);
			var b = Convert.ToDouble(B);

			switch (Action)
			{
				case "Add":
					Result = (decimal)(businessLogic.Add(a, b));
					break;
				case "Subtract":
					Result = (decimal)(businessLogic.Subtract(a, b));
					break;
				case "Multiply":
					Result = (decimal)(businessLogic.Multiply(a, b));
					break;
				case "Divide":
					Result = (decimal)(businessLogic.Divide(a, b));
					break;
				default:
					break;
			}
			return true;
		}

		#endregion

		#region Methods: Public

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion

}

