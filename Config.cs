using HamstarHelpers.Utilities.Config;
using System;
using System.Collections.Generic;


namespace Licenses {
	public class LicensesConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 2, 0 );
		public readonly static string ConfigFileName = "Licenses Config.json";


		////////////////

		public string VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		//public bool DebugModeCheat = false;

		public bool ResetWayfarerShop = true;
		public bool FullNihilismBlacklistReset = true;
		
		public ISet<string> FreeStarterItems = new HashSet<string>();

		public int LicensesPerPack = 5;
		public int LicenseCostInPP = 3;

		public int ItemLicenseCostBase = 1;
		public bool ItemLicenseCostIncreasesWithRarity = true;
		public float ArmorLicenseCostMultiplier = 1f / 3f;
		public float AccessoryLicenseCostMultiplier = 1f;
		public bool FreeMaterials = true;
		public bool FreePlaceables = true;
		public bool FreeRecipes = true;

		public int NewPlayerStarterLicenses = 8;

		public bool ForceSpawnWayfarer = true;



		////////////////

		public void SetDefaults() {
			this.FreeStarterItems.Add( "Any Wood Equipment" );
			this.FreeStarterItems.Add( "Any Copper Or Tin Equipment" );

			this.FreeStarterItems.Add( "License" );
			this.FreeStarterItems.Add( "Wayfarer's Pack" );

			this.FreeStarterItems.Add( "Copper Coin" );
			this.FreeStarterItems.Add( "Silver Coin" );
			this.FreeStarterItems.Add( "Gold Coin" );
			this.FreeStarterItems.Add( "Platinum Coin" );

			this.FreeStarterItems.Add( "Blowpipe" );
			this.FreeStarterItems.Add( "Seed" );
			this.FreeStarterItems.Add( "Rope" );
			this.FreeStarterItems.Add( "Rope Coil" );
			this.FreeStarterItems.Add( "Empty Bucket" );
			this.FreeStarterItems.Add( "Water Bucket" );
			this.FreeStarterItems.Add( "Lava Bucket" );
			this.FreeStarterItems.Add( "Honey Bucket" );
			this.FreeStarterItems.Add( "Bug Net" );
			this.FreeStarterItems.Add( "Glowstick" );
			this.FreeStarterItems.Add( "Sticky Glowstick" );
			this.FreeStarterItems.Add( "Bouncy Glowstick" );

			this.FreeStarterItems.Add( "Lesser Healing Potion" );
			this.FreeStarterItems.Add( "Lesser Mana Potion" );

			this.FreeStarterItems.Add( "Gel" );
			this.FreeStarterItems.Add( "Mushroom" );
			this.FreeStarterItems.Add( "Fallen Star" );
			this.FreeStarterItems.Add( "Acorn" );

			this.FreeStarterItems.Add( "Wood" );
			this.FreeStarterItems.Add( "Rich Mahogany" );
			this.FreeStarterItems.Add( "Boreal Wood" );
			this.FreeStarterItems.Add( "Palm Wood" );

			this.FreeStarterItems.Add( "Dirt Block" );
			this.FreeStarterItems.Add( "Stone Block" );
			this.FreeStarterItems.Add( "Clay Block" );
			this.FreeStarterItems.Add( "Mud Block" );
			this.FreeStarterItems.Add( "Sand Block" );
			this.FreeStarterItems.Add( "Silt Block" );
			this.FreeStarterItems.Add( "Snow Block" );
			this.FreeStarterItems.Add( "Ice Block" );

			this.FreeStarterItems.Add( "Wood Platform" );

			this.FreeStarterItems.Add( "Wood Wall" );
			this.FreeStarterItems.Add( "Dirt Wall" );
			this.FreeStarterItems.Add( "Stone Wall" );

			this.FreeStarterItems.Add( "Torch" );
			this.FreeStarterItems.Add( "Campfire" );
			this.FreeStarterItems.Add( "Work Bench" );
			this.FreeStarterItems.Add( "Wooden Table" );
			this.FreeStarterItems.Add( "Wooden Chair" );
			this.FreeStarterItems.Add( "Wooden Door" );
			this.FreeStarterItems.Add( "Furnace" );
			this.FreeStarterItems.Add( "Iron Anvil" );
			this.FreeStarterItems.Add( "Lead Anvil" );
			this.FreeStarterItems.Add( "Bottle" );
			this.FreeStarterItems.Add( "Chest" );
			this.FreeStarterItems.Add( "Barrel" );
			this.FreeStarterItems.Add( "Wooden Door" );
			this.FreeStarterItems.Add( "Minecart Track" );
			
			this.FreeStarterItems.Add( "Sunglasses" );
		}
		
		
		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new LicensesConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= LicensesConfigData.ConfigVersion ) {
				return false;
			}
			
			if( vers_since < new Version( 1, 0, 0 ) ) {
				new_config.SetDefaults();
			}
			if( vers_since < new Version(1, 0, 1) ) {
				if( this.LicenseCostInPP == 5 ) {
					this.LicenseCostInPP = new_config.LicenseCostInPP;
				}
				if( this.NewPlayerStarterLicenses == 3 ) {
					this.NewPlayerStarterLicenses = new_config.NewPlayerStarterLicenses;
				}
			}
			if( vers_since < new Version( 1, 0, 3 ) ) {
				this.FreeStarterItems = new_config.FreeStarterItems;	// Sorry!
				if( this.LicensesPerPack == 3 ) {
					this.LicensesPerPack = new_config.LicensesPerPack;
				}
			}
			if( vers_since < new Version( 1, 2, 0 ) ) {
				new_config.SetDefaults();
			}

			this.VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

			return true;
		}


		public void UpdateForSettings() {
			if( this.FreeMaterials ) {
				this.FreeStarterItems.Add( "Any Plain Material" );
			} else {
				this.FreeStarterItems.Remove( "Any Plain Material" );
			}

			if( this.FreePlaceables ) {
				this.FreeStarterItems.Add( "Any Placeable" );
			} else {
				this.FreeStarterItems.Remove( "Any Placeable" );
			}
		}
	}
}
