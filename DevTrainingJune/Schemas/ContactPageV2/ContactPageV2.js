define("ContactPageV2", ["ContactPageV2Resources", "ProcessModuleUtilities"], 
function(resources, ProcessModuleUtilities) {
	return {
		entitySchemaName: "Contact",
		attributes: {
			"Account": {
				lookupListConfig: {
					columns: ["Web", "Owner", "Owner.Email"]
				}
			},
			"EventAttribute": {
				"dependencies": [
					{
						"columns": ["Name"],
						"methodName": "onNameChanged"
					},
					{
						"columns": ["Email"],
						"methodName": "onEmailChanged"
					}
				]
			},
		},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			/**
			 * Initializes the initial values of the model.
			 * @inheritdoc Terrasoft./**
			 * @inheritdoc Terrasoft.BasePageV2#init
			 * @override
			 */
			init: function() {
				this.callParent(arguments);
			},

			/**
			 * @inheritdoc Terrasoft.BasePageV2#onEntityInitialized
			 * @override
			 */
			 onEntityInitialized: function() {
				this.callParent(arguments);
			},

			/**
			 * 
			 */
			 onNameChanged: function(a, colChanged){
				var newValue = this.get(colChanged);
				this.showInformationDialog("Column " +colChanged +" has changed, new value is "+ newValue);
			},
			onEmailChanged: function(a, colChanged){
				var newValue = this.get(colChanged);
				var domain = newValue.split("@")[1];
				var web = this.$Account.Web;
				var result = web.includes(domain);

				if(result === false){
					
					var yB = this.Terrasoft.MessageBoxButtons.YES;
					yB.style = "GREEN";
	
					var nB = this.Terrasoft.MessageBoxButtons.NO;
					nB.style = "RED";
					this.showConfirmationDialog(
						"Are you sure this is the correct email, domain don't match",
						function(returnCode) {
							if (returnCode === this.Terrasoft.MessageBoxButtons.NO.returnCode) {
								console.log("NO Selected");
							}else{
								console.log("YES Selected");
							}
						},
						[
							yB.returnCode,
							nB.returnCode
						],
						null
					);
				}

				
			},

			/**
			 * @inheritdoc Terrasoft.BasePageV2#getActions
			 * @overridden
			 */
			getActions: function() {
				var actionMenuItems = this.callParent(arguments);
				actionMenuItems.addItem(this.getButtonMenuSeparator());
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action1",
					"Caption": this.get("Resources.Strings.ActionOneCaption"),
					"Click": {"bindTo": "onActionClick"},
					ImageConfig: this.get("Resources.Images.CreatioSquare"),
				}));
				actionMenuItems.addItem(this.getButtonMenuItem({
					"Tag": "action2",
					"Caption": this.get("Resources.Strings.ActionTwoCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Items": this.addSubItems()
				}));
				return actionMenuItems;
			},

			onActionClick: function(tag){
				//https://academy.creatio.com/docs/developer/integrations_and_api/processengineservice/processengineservice?_ga=2.3506422.772349606.1622122733-2062963899.1599201789#case-1934
				var args = {
					sysProcessName: "Process_SharedFunctionsUserTask",
					parameters: {
						inA: this.$Name.length,
						inB: this.$Account.Web.length
					}
				}
				ProcessModuleUtilities.executeProcess(args);
			},

			addSubItems: function(){
				var collection = this.Ext.create("Terrasoft.BaseViewModelCollection");
				collection.addItem(this.getButtonMenuItem({
					"Caption": this.get("Resources.Strings.SubActionOneCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Tag": "sub1"
				}));
				collection.addItem(this.getButtonMenuItem({
					"Caption": this.get("Resources.Strings.SubActionTwoCaption"),
					"Click": {"bindTo": "onActionClick"},
					"Tag": "sub2"
				}));
				return collection;
			},

		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "FullNameKirill",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 3,
						"layoutName": "ContactGeneralInfoBlock"
					},
					"bindTo": "Name"
				},
				"parentName": "ContactGeneralInfoBlock",
				"propertyName": "items",
				"index": 6
			}
		]/**SCHEMA_DIFF*/
	};
});
