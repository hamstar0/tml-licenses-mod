using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.Utilities.EntityGroups;
using HamstarHelpers.Utilities.Messages;
using Nihilism;
using Rewards;
using Rewards.Items;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Licenses {
    public class LicensesMod : Mod {
		public static LicensesMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-licenses-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + LicensesConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !LicensesMod.Instance.ConfigJson.LoadFile() ) {
				LicensesMod.Instance.ConfigJson.SaveFile();
			}
		}


		////////////////

		public JsonConfig<LicensesConfigData> ConfigJson { get; private set; }
		public LicensesConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public LicensesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<LicensesConfigData>( LicensesConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new LicensesConfigData() );
		}

		////////////////

		public override void Load() {
			LicensesMod.Instance = this;

			this.LoadConfig();

			EntityGroups.Enable();

			TmlLoadHelpers.AddWorldLoadEachPromise( this.LoadGameMode );
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Licenses config " + LicensesConfigData.ConfigVersion.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Licenses updated to " + LicensesConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}

			this.Config.UpdateForSettings();
		}

		public override void Unload() {
			LicensesMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return LicensesAPI.Call( call_type, new_args );
		}


		////////////////

		public void LoadGameMode() {
			NihilismAPI.SuppressAutoSavingOn();
			RewardsAPI.SuppressAutoSavingOn();
			
			NihilismAPI.SetItemFilter( true, true );

			if( this.Config.OverrideNihilismDefaultFilters ) {
				NihilismAPI.SetRecipesFilter( !this.Config.FreeRecipes, true );
				NihilismAPI.SetNpcFilter( false, true );
				NihilismAPI.SetNpcLootFilter( false, true );
				NihilismAPI.ClearItemWhitelist( true );
				NihilismAPI.ClearRecipeWhitelist( true );
				NihilismAPI.ClearNpcWhitelist( true );
				NihilismAPI.ClearNpcLootWhitelist( true );
			}

			foreach( string name in this.Config.FreeStarterItems ) {
				NihilismAPI.SetItemWhitelistEntry( name, true );
			}
			NihilismAPI.NihilateCurrentWorld();

			this.CreateLicensePacks();

			if( this.Config.ForceSpawnWayfarer ) {
				RewardsAPI.SpawnWayfarer( false );
			}

			InboxMessages.ReadMessage( "nihilism_init" );
		}


		private void CreateLicensePacks() {
			var lic_def = new ShopPackItemDefinition {
				Name = "License",
				Stack = this.Config.LicensesPerPack,
				CrimsonWorldOnly = null
			};
			var wild_lic_def = new ShopPackItemDefinition {
				Name = "Wildcard License",
				Stack = this.Config.WildcardLicensesPerPack,
				CrimsonWorldOnly = null
			};

			var def1 = new ShopPackDefinition {
				Name = "Standard License Pack",
				Price = this.Config.LicensePackCostInPP,
				Items = new ShopPackItemDefinition[] { lic_def }
			};
			var def2 = new ShopPackDefinition {
				Name = "Wildcard License Pack",
				Price = this.Config.WildcardLicensePackCostInPP,
				Items = new ShopPackItemDefinition[] { wild_lic_def }
			};
			
			if( this.Config.ResetWayfarerShop ) {
				RewardsAPI.ShopClear();
			}

			if( this.Config.LicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def1 );
			}
			if( this.Config.WildcardLicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def2);
			}

			TmlLoadHelpers.TriggerCustomPromise( "LicensesPreEnterWorld" );
			TmlLoadHelpers.AddWorldUnloadOncePromise( () => {
				TmlLoadHelpers.ClearCustomPromise( "LicensesPreEnterWorld" );
			} );
		}
	}
}
