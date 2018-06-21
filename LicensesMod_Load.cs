using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.TmlHelpers;
using Nihilism;
using Rewards;
using Rewards.Items;
using Terraria.ModLoader;


namespace Licenses {
    public partial class LicensesMod : Mod {
		public void LoadGameModeOnWorldLoad() {
			NihilismAPI.SuppressAutoSavingOn();
			RewardsAPI.SuppressConfigAutoSavingOn();
			
			this.LoadNihilismFilters();
			this.LoadLicensePacks();

			if( this.Config.RemoveRewardsGrinding ) {
				RewardsAPI.GetModSettings().GrindKillMultiplier = 0f;
			}
			if( this.Config.ForceSpawnWayfarer ) {
				RewardsAPI.SpawnWayfarer( false );
			}

			// Finish loading Nihilism
			TmlLoadHelpers.AddCustomPromise( "LicensesOnEnterWorld", () => {
				NihilismAPI.NihilateCurrentWorld();

				InboxMessages.ReadMessage( "nihilism_init" );

				return false;
			} );
			
			TmlLoadHelpers.TriggerCustomPromise( "LicensesOnGameModeLoad" );
			TmlLoadHelpers.AddWorldUnloadOncePromise( () => {
				TmlLoadHelpers.ClearCustomPromise( "LicensesOnGameModeLoad" );
			} );
		}


		private void LoadNihilismFilters() {
			if( this.Config.OverrideNihilismDefaultFilters ) {
				NihilismAPI.ClearRecipeBlacklist( true );
				NihilismAPI.ClearItemBlacklist( true );
				NihilismAPI.ClearNpcBlacklist( true );
				NihilismAPI.ClearNpcLootBlacklist( true );
			}
			
			NihilismAPI.SetItemBlacklistEntry( "Any Item", true );

			if( !this.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeBlacklistEntry( "Any Item", true );
			}

			if( this.Config.FreeMaterials ) {
				NihilismAPI.SetItemWhitelistEntry( "Any Plain Material", true );
			}
			if( this.Config.FreePlaceables ) {
				NihilismAPI.SetItemWhitelistEntry( "Any Placeable", true );
			}

			foreach( string name in this.Config.FreeStarterItems ) {
				NihilismAPI.SetItemWhitelistEntry( name, true );
			}
		}


		private void LoadLicensePacks() {
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
		}
	}
}
