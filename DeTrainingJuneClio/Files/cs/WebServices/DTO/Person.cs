using System.Runtime.Serialization;

namespace DeTrainingJuneClio.WebServices.DTO
{
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
