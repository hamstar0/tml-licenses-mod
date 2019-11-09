using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Licenses.Items;
using Nihilism;
using Rewards;
using Rewards.Configs;
using Rewards.Items;
using System.Collections.Generic;
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

		public void LoadGameMode() {
			if( LicensesMod.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Loading game mode..." );
			}

			NihilismAPI.InstancedFiltersOn();
			NihilismAPI.OnSyncOrWorldLoad( ( isSync ) => {
				if( isSync ) { return; }
				this.LoadNihilismFilters();
				NihilismAPI.NihilateCurrentWorld( true );
			}, 0f );

			LoadHooks.AddWorldLoadEachHook( () => {
				if( LicensesMod.Config.RemoveRewardsGrinding ) {
					RewardsPointsConfig rewConfig = ModContent.GetInstance<RewardsPointsConfig>();
					rewConfig.GrindKillMultiplier = 0f;
				}
				if( LicensesMod.Config.ForceSpawnWayfarer ) {
					RewardsAPI.SpawnWayfarer( false );
				}
				
				this.LoadLicensePacks();
			} );

			CustomLoadHooks.TriggerHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );

			LoadHooks.AddWorldUnloadEachHook( () => {
				CustomLoadHooks.ClearHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );
				LoadHooks.AddWorldLoadOnceHook( () => {
					CustomLoadHooks.TriggerHook( LicensesMod.GameModeLoadValidator, LicensesMod.MyValidatorKey );	// Whee!
				} );
			} );

			if( LicensesMod.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Finished loading game mode" );
			}
		}


		private void LoadNihilismFilters() {
			if( LicensesMod.Config.OverrideNihilismDefaultFilters ) {
				NihilismAPI.ClearFiltersForCurrentWorld( true );
			}
			
			NihilismAPI.SetItemBlacklistGroupEntry( "Any Item", true );

			if( !LicensesMod.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeBlacklistGroupEntry( "Any Item", true );
			}

			if( LicensesMod.Config.FreeMaterials ) {
				NihilismAPI.SetItemWhitelistGroupEntry( "Any Plain Material", true );
			}
			if( LicensesMod.Config.FreePlaceables ) {
				NihilismAPI.SetItemWhitelistGroupEntry( "Any Placeable", true );
			}

			foreach( ItemDefinition itemDef in LicensesMod.Config.FreeStarterItems ) {
				NihilismAPI.SetItemWhitelistEntry( itemDef, true );
			}
		}


		private void LoadLicensePacks() {
			var licDef = new ShopPackItemDefinition(
				new ItemDefinition( ModContent.ItemType<LicenseItem>() ),
				LicensesMod.Config.LicensesPerPack,
				null
			);
			var wildLicDef = new ShopPackItemDefinition(
				new ItemDefinition( ModContent.ItemType<WildcardLicenseItem>() ),
				LicensesMod.Config.WildcardLicensesPerPack,
				null
			);

			var def1 = new ShopPackDefinition(
				null,
				"Standard License Pack",
				LicensesMod.Config.LicensePackCostInPP,
				new List<ShopPackItemDefinition> { licDef }
			);
			var def2 = new ShopPackDefinition(
				null,
				"Wildcard License Pack",
				LicensesMod.Config.WildcardLicensePackCostInPP,
				new List<ShopPackItemDefinition> { wildLicDef }
			);
			
			if( LicensesMod.Config.ResetWayfarerShop ) {
				RewardsAPI.ShopClear();
			}

			if( LicensesMod.Config.LicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def1 );
			}
			if( LicensesMod.Config.WildcardLicensesPerPack > 0 ) {
				RewardsAPI.ShopAddPack( def2);
			}
		}
	}
}
