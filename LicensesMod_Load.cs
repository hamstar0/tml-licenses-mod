using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Messages.Inbox;
using Licenses.Items;
using Nihilism;
using Rewards;
using Rewards.Configs;
using Rewards.Items;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Licenses {
    public partial class LicensesMod : Mod {
		private readonly static object MyValidatorKey;
		public readonly static CustomLoadHookValidator<object> GameModeLoadValidator;


		////////////////

		static LicensesMod() {
			LicensesMod.MyValidatorKey = new object();
			LicensesMod.GameModeLoadValidator = new CustomLoadHookValidator<object>( LicensesMod.MyValidatorKey );
		}


		////////////////

		public void LoadGameModeOnWorldLoad() {
			if( this.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Loading game mode..." );
			}

			NihilismAPI.InstancedFiltersOn();	//.SuppressAutoSavingOn();
			RewardsAPI.SuppressConfigAutoSavingOn();
			
			this.LoadNihilismFilters();
			this.LoadLicensePacks();

			if( this.Config.RemoveRewardsGrinding ) {
				Mod rewMod = ModLoader.GetMod( "Rewards" );
				RewardsPointsConfig rewConfig = rewMod.GetConfig<RewardsPointsConfig>();
				rewConfig.GrindKillMultiplier = 0f;
			}
			if( this.Config.ForceSpawnWayfarer ) {
				RewardsAPI.SpawnWayfarer( false );
			}

			// Finish loading Nihilism
			CustomLoadHooks.AddHook( LicensesPlayer.EnterWorldValidator, ( _ ) => {
				if( this.Config.DebugModeInfo ) {
					LogHelpers.Alert( "Loading Nihilism for game mode..." );
				}

				this.LoadNihilismFilters();
				NihilismAPI.NihilateCurrentWorld();

				string __;
				InboxMessages.ReadMessage( "nihilism_init", out __ );

				return false;
			} );

			CustomLoadHooks.TriggerHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );

			LoadHooks.AddWorldUnloadOnceHook( () => {
				CustomLoadHooks.ClearHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );
				LoadHooks.AddWorldLoadOnceHook( () => {
					CustomLoadHooks.TriggerHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );	// Whee!
				} );
			} );

			if( this.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Finished loading game mode" );
			}
		}


		private void LoadNihilismFilters() {
			if( this.Config.OverrideNihilismDefaultFilters ) {
				NihilismAPI.ClearFiltersForCurrentWorld();
			}
			
			NihilismAPI.SetItemBlacklistGroupEntry( "Any Item", true );

			if( !this.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeBlacklistGroupEntry( "Any Item", true );
			}

			if( this.Config.FreeMaterials ) {
				NihilismAPI.SetItemWhitelistGroupEntry( "Any Plain Material", true );
			}
			if( this.Config.FreePlaceables ) {
				NihilismAPI.SetItemWhitelistGroupEntry( "Any Placeable", true );
			}

			foreach( ItemDefinition itemDef in this.Config.FreeStarterItems ) {
				NihilismAPI.SetItemWhitelistEntry( itemDef, true );
			}
		}


		private void LoadLicensePacks() {
			var licDef = new ShopPackItemDefinition(
				new ItemDefinition( this.ItemType<LicenseItem>() ),
				this.Config.LicensesPerPack,
				null
			);
			var wildLicDef = new ShopPackItemDefinition(
				new ItemDefinition( this.ItemType<WildcardLicenseItem>() ),
				this.Config.WildcardLicensesPerPack,
				null
			);

			var def1 = new ShopPackDefinition(
				null,
				"Standard License Pack",
				this.Config.LicensePackCostInPP,
				new ShopPackItemDefinition[] { licDef }
			);
			var def2 = new ShopPackDefinition(
				null,
				"Wildcard License Pack",
				this.Config.WildcardLicensePackCostInPP,
				new ShopPackItemDefinition[] { wildLicDef }
			);
			
			if( this.Config.ResetWayfarerShop ) {
				RewardsAPI.ShopClear();
			}

			if( this.Config.LicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def1 );
			}
			if( this.Config.WildcardLicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def2);
			}
		}
	}
}
