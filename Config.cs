using HamstarHelpers.Utilities.Config;
using System;
using System.Collections.Generic;


namespace Licenses {
	public class LicensesConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 0, 0 );
		public readonly static string ConfigFileName = "Licenses Config.json";


		////////////////

		public string VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;

		public bool ClearWayfarer = true;

		public string ItemBlacklistPattern = "(.*?)";
		public ISet<string> FreeStarterItems = new HashSet<string>();

		public int LicensesPerPack = 3;
		public int LicenseCostInPP = 10;


		////////////////

		public void SetDefaults() {
			this.FreeStarterItems.Add( "Copper Pickaxe" );
			this.FreeStarterItems.Add( "Copper Axe" );
			this.FreeStarterItems.Add( "Copper Shortsword" );
			this.FreeStarterItems.Add( "Wood Sword" );
			this.FreeStarterItems.Add( "Wood Bow" );
			this.FreeStarterItems.Add( "Wood Arrow" );
			this.FreeStarterItems.Add( "Bucket" );

			this.FreeStarterItems.Add( "Wood" );
			this.FreeStarterItems.Add( "Dirt" );
			this.FreeStarterItems.Add( "Stone" );

			this.FreeStarterItems.Add( "Torch" );
			this.FreeStarterItems.Add( "Work Bench" );
			this.FreeStarterItems.Add( "Table" );
			this.FreeStarterItems.Add( "Chair" );
			this.FreeStarterItems.Add( "Furnace" );
			this.FreeStarterItems.Add( "Iron Anvil" );
			this.FreeStarterItems.Add( "Lead Anvil" );
			this.FreeStarterItems.Add( "Bottle" );
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

			this.VersionSinceUpdate = LicensesConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
