using DeTrainingJuneClio.WebServices.DTO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Core;
using Terrasoft.Web.Common;

namespace DeTrainingJuneClio.WebServices
{
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class FirstService : BaseService
	{
		#region Properties
		private SystemUserConnection _systemUserConnection;
		private SystemUserConnection SystemUserConnection
		{
			get
			{
				return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
			}
		}
		#endregion

		#region Methods : REST
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public Person PostName(Person person)
		{
			//http://k_krylov:7070/0/rest/FirstService/PostName
			UserConnection userConnection = UserConnection ?? SystemUserConnection;
			return new Person
			{
				LastName = person.LastName,
				FirstName = person.FirstName,
				Age = person.Age+10,
				Email = person.Email
			};
		}

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, UriTemplate ="GetNameUri/{name}",
			BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
		public Person GetName(string name)
		{
			//http://k_krylov:7070/0/rest/FirstService/GetNameUri/{name}
			UserConnection userConnection = UserConnection ?? SystemUserConnection;
			return new Person
			{
				LastName = "Krylov",
				FirstName = name,
				Age = 40,
				Email = "k.krylov@creatio.com"
			};
		}

		#endregion

		#region Methods : Private

		#endregion
	}

}
