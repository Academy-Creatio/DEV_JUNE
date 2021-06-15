using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Core;
using Terrasoft.Web.Common;

namespace DevTrainingJune.WebServices
{
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class FirstServiceConf : BaseService
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
			//http://k_krylov:7070/0/rest/FirstServiceConf/PostName
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
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, 
			BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
		public Person GetName(string name)
		{
			//http://k_krylov:7070/0/rest/FirstServiceConf/GetName
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


	[DataContract]
	public class Person
	{
		[DataMember(Name = "age")]
		public int Age { get; set; }
		
		[DataMember(Name ="firstName")]
		public string FirstName { get; set; }

		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }
	}

}
