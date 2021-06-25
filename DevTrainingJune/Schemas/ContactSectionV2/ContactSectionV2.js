define("ContactSectionV2", [], function() {
	return {
		entitySchemaName: "Contact",
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		diff: /**SCHEMA_DIFF*/[

			{
				"operation": "insert",
				"name": "PrimaryContactButtonRed",
				"parentName": "CombinedModeActionButtonsCardLeftContainer", //INVISIBLE in section, visible on the page
				"propertyName": "items",
				"values":{
					itemType: this.Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.RED,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					caption: "Section Red Button",
					hint: "Section red button hint",
					click: {"bindTo": "onMyMainButtonClick"},
					tag: "CombinedModeActionButtonsCardLeftContainer_Red"
				}
			},
			{
				"operation": "insert",
				"name": "PrimaryContactButtonGreen",
				"parentName": "ActionButtonsContainer", //visible in section and on a page
				"propertyName": "items",
				"values":{
					itemType: this.Terrasoft.ViewItemType.BUTTON,
					style: Terrasoft.controls.ButtonEnums.style.GREEN,
					classes: {
						"textClass": ["actions-button-margin-right"],
						"wrapperClass": ["actions-button-margin-right"]
					},
					caption: "Section Green Button",
					hint: "Section red button hint",
					click: {"bindTo": "onMyMainButtonClick"},
					tag: "ActionButtonsContainer_Red"
				}
			},

		]/**SCHEMA_DIFF*/,
		messages:{
			
			/**
			 * Subscribed on: ContactPageV2 
			 */
			"SectionActionClicked":{
				mode: this.Terrasoft.MessageMode.PTP,
				direction: this.Terrasoft.MessageDirectionType.PUBLISH
			}
		},
		methods: {
			getSectionActions: function() {
				var actionMenuItems = this.callParent(arguments);
				actionMenuItems.addItem(this.getButtonMenuSeparator());
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action1",
					"Caption": "Section Action One",
					"Click": {"bindTo": "onActionClick"},
				}));
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action2",
					"Caption": "Section Action Two",
					"Click": {"bindTo": "onActionClick"}
				}));
				return actionMenuItems;
			},

			onActionClick: function(tag){
				this.showInformationDialog("Section Action Clicked with tag: "+ tag);
			},

			onMyMainButtonClick: function(){
				var tag = arguments[3]; //identifies button
				var primaryColumnValue = this.$ActiveRow;
				var gridData = this.getGridData();
				var activeRow = gridData.get(primaryColumnValue);

				this.sandbox.publish("SectionActionClicked", null, [this.sandbox.id+"_CardModuleV2"]);
				// this.sandbox.id = SectionModuleV2_ContactSectionV2
				//this.sandbox.publish("SectionActionClicked", null, ["some_random_but_unique_text_this_is_my_tag"]);
			}
		}
	};
});
