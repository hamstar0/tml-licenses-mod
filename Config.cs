using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;


namespace Licenses {
	public class LicensesConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 2, 0, 1 );
		public readonly static string ConfigFileName = "Licenses Config.json";


		////////////////

		public string VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		//public bool DebugModeCheat = false;

		public bool ResetWayfarerShop = true;

		public bool OverrideNihilismDefaultFilters = true;

		public ISet<string> FreeStarterItems = new HashSet<string>();

		public int LicensePackCostInPP = 10;
		public int LicensesPerPack = 6;
		public int WildcardLicensePackCostInPP = 20;
		public int WildcardLicensesPerPack = 20;

		public int LicenseCostBase = 1;
		public float LicenseCostRarityMultiplier = 1;
		public float LicenseCostArmorMultiplier = 1f / 3f;
		public float LicenseCostAccessoryMultiplier = 1f;

		public int WildcardLicenseCostBase = 1;
		public int WildcardLicenseCostRarityMultiplier = 1;

		public bool FreeMaterials = true;
		public bool FreePlaceables = true;
		public bool FreeRecipes = true;

		public int NewPlayerStarterLicenses = 8;

		public bool ForceSpawnWayfarer = true;
		public bool RemoveRewardsGrinding = true;



		////////////////

		public void SetDefaults() {
			this.FreeStarterItems.Add( "Any Wood Equipment" );
			this.FreeStarterItems.Add( "Any Copper Or Tin Equipment" );

			this.FreeStarterItems.Add( "License" );
			this.FreeStarterItems.Add( "Wildcard License" );
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

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
