using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
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
			if( !LicensesMod.Instance.JsonConfig.LoadFile() ) {
				LicensesMod.Instance.JsonConfig.SaveFile();
			}
		}


		////////////////

		public JsonConfig<LicensesConfigData> JsonConfig { get; private set; }
		public LicensesConfigData Config { get { return this.JsonConfig.Data; } }


		////////////////

		public LicensesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.JsonConfig = new JsonConfig<LicensesConfigData>( LicensesConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new LicensesConfigData() );
		}

		////////////////

		public override void Load() {
			LicensesMod.Instance = this;

			this.LoadConfig();

			TmlLoadHelpers.AddWorldLoadPromise( this.LoadGameMode );
		}

		private void LoadConfig() {
			if( !this.JsonConfig.LoadFile() ) {
				this.Config.SetDefaults();
				this.JsonConfig.SaveFile();
				ErrorLogger.Log( "Licenses config " + LicensesConfigData.ConfigVersion.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Licenses updated to " + LicensesConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
			}
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
			if( !TmlLoadHelpers.IsWorldLoaded() ) { return; }
			
			NihilismAPI.SuppressAutoSavingOn();
			RewardsAPI.SuppressAutoSavingOn();
			
			NihilismAPI.SetItemsBlacklistPattern( this.Config.ItemBlacklistPattern, true );

			if( this.Config.FullNihilismBlacklistReset ) {
				NihilismAPI.SetRecipesBlacklistPattern( "$.", true );
				NihilismAPI.SetNpcBlacklistPattern( "$.", true );
				NihilismAPI.SetNpcLootBlacklistPattern( "$.", true );
			}

			foreach( string name in this.Config.FreeStarterItems ) {
				NihilismAPI.SetItemsWhitelistEntry( name, true );
			}
			NihilismAPI.NihilateCurrentWorld();

			var item_def = new ShopPackItemDefinition {
				Name = "License",
				Stack = this.Config.LicensesPerPack,
				CrimsonWorldOnly = null
			};
			var def = new ShopPackDefinition {
				Name = "Item License Pack",
				Price = this.Config.LicenseCostInPP,
				Items = new ShopPackItemDefinition[] { item_def }
			};

			if( this.Config.ResetWayfarerShop ) {
				RewardsAPI.ShopClear();
			}

			RewardsAPI.ShopAddPack( def );

			if( this.Config.ForceSpawnWayfarer ) {
				RewardsAPI.SpawnWayfarer( false );
			}

			InboxMessages.ReadMessage( "nihilism_init" );
		}
	}
}
