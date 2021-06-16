using System;
using System.Linq;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

namespace WorkshopWorkingWithData.Files.DataOperations
{
	internal sealed class DeletingData
	{
		readonly UserConnection UserConnection;
		public DeletingData(UserConnection userConnection)
		{
			UserConnection = userConnection;
		}

		internal string DeleteContact(Guid ContactId)
		{
			const string tableName = "Contact";

			var names = new string[] { "John", "Bob", "Sam" };
			Delete delete= new Delete(UserConnection)
				.From(tableName)
				.Where("Name").In(Column.Parameters(names.ToList()))as Delete;
				//.Where("Id").IsEqual(Column.Parameter(ContactId)) as Delete;
			return (delete.Execute() == 3) ? "Deleted" : "Failed";
		}

		internal string DeleteContactEsq(Guid ContactId)
		{
			const string tableName = "Contact";
			EntitySchema contactSchema = UserConnection.EntitySchemaManager.GetInstanceByName(tableName);
			Entity contact = contactSchema.CreateEntity(UserConnection);
			contact.FetchFromDB(ContactId);

			if (contact.Delete())
				return "Deleted";
			return "Failed";
		}
	}
}
