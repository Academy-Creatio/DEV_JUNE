using System.Collections.Generic;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;

namespace Events.Files.cs
{
	/// <summary>
	/// Listener for 'EntityName' entity events.
	/// </summary>
	/// <see href="https://academy.creatio.com/docs/developer/back-end_development/entity_event_layer/entity_event_layer"/>
	/// <seealso cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener" />
	[EntityEventListener(SchemaName = "Contact")]
	class ContactEventListener : BaseEntityEventListener
	{
		#region Methods : Public : OnSave

		private string[] columnsToMonitor = new string[] { "Name", "Email" };
		private Entity entity = default;
		
		private void Entity_Validating(object sender, EntityValidationEventArgs e)
		{
			IsChangeInteresting(columnsToMonitor, entity.GetChangedColumnValues());
			IsNameValid();

		}

		public override void OnSaving(object sender, EntityBeforeEventArgs e)
		{
			base.OnSaving(sender, e);
			entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
			entity.Validating += Entity_Validating;

			//If validation is enabled then use  Entity_Validating to provide proper feedback
			if (!e.IsValidationEnabled)
			{
				if (IsNameValid())
				{
					e.IsCanceled = true;
				}
			}
		}
		public override void OnSaved(object sender, EntityAfterEventArgs e)
		{
			base.OnSaved(sender, e);
			entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
			IsChangeInteresting(columnsToMonitor, e.ModifiedColumnValues);
		}
		#endregion

		#region Methods : Public : OnInsert
		public override void OnInserting(object sender, EntityBeforeEventArgs e)
		{
			base.OnInserting(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnInserted(object sender, EntityAfterEventArgs e)
		{
			base.OnInserted(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		#endregion

		#region Methods : Public : OnUpdate
		public override void OnUpdating(object sender, EntityBeforeEventArgs e)
		{
			base.OnUpdating(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnUpdated(object sender, EntityAfterEventArgs e)
		{
			base.OnUpdated(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		#endregion

		#region Methods : Public : OnDelete
		public override void OnDeleting(object sender, EntityBeforeEventArgs e)
		{
			base.OnDeleting(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		public override void OnDeleted(object sender, EntityAfterEventArgs e)
		{
			base.OnDeleted(sender, e);
			Entity entity = (Entity)sender;
			UserConnection userConnection = entity.UserConnection;
		}
		#endregion


		/// <summary>
		/// Check if columns changed are interesting
		/// </summary>
		/// <param name="interestingColunms">Columns under monitoring</param>
		/// <param name="changedColumns">collection of changed columns</param>
		/// <returns></returns>
		private bool IsChangeInteresting(string[] interestingColunms, IEnumerable<EntityColumnValue> changedColumns)
		{
			for (int i = 0; i < interestingColunms.Length; i++)
			{
				foreach (var column in changedColumns.ToList())
				{
					if (column.Name == interestingColunms[i])
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Name validation method
		/// </summary>
		/// <returns>Validated name value</returns>
		private bool IsNameValid()
		{
			string columnName = "Name";
			EntitySchemaColumn esc = entity.Schema.Columns.FindByName(columnName);
			string newValue = entity.GetTypedColumnValue<string>(columnName);
			string oldValue = entity.GetTypedOldColumnValue<string>(columnName);

			if (newValue != oldValue)
			{
				AddValidationMessage(esc, "Name cannot change");
				return false;
			}
			return true;
		}


		/// <summary>
		/// Adds validation message to collection of validation messages
		/// </summary>
		/// <param name="esc">EntitySchemaColumn that triggered validation message</param>
		/// <param name="messageText">Text visible to a user.</param>
		private void AddValidationMessage(EntitySchemaColumn esc, string messageText)
		{
			EntityValidationMessage evm = new EntityValidationMessage
			{
				MassageType = MessageType.Error,
				Text = messageText,
				Column = esc
			};
			entity.ValidationMessages.Add(evm);
		}
	}
}
