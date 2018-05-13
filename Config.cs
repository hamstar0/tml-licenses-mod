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

		public string ItemBlacklistPattern = "(.*?)";
		public ISet<string> StarterItems = new HashSet<string>();

		public int LicensesPerPack = 3;
		public int LicenseCostInPP = 10;


		////////////////

		public void SetDefaults() {
			this.StarterItems.Add( "Copper Pickaxe" );
			this.StarterItems.Add( "Copper Axe" );
			this.StarterItems.Add( "Copper Shortsword" );
			this.StarterItems.Add( "Wood Sword" );
			this.StarterItems.Add( "Wood Bow" );
			this.StarterItems.Add( "Wood Arrow" );
			this.StarterItems.Add( "Bucket" );

			this.StarterItems.Add( "Wood" );
			this.StarterItems.Add( "Dirt" );
			this.StarterItems.Add( "Stone" );

			this.StarterItems.Add( "Torch" );
			this.StarterItems.Add( "Work Bench" );
			this.StarterItems.Add( "Table" );
			this.StarterItems.Add( "Chair" );
			this.StarterItems.Add( "Furnace" );
			this.StarterItems.Add( "Iron Anvil" );
			this.StarterItems.Add( "Lead Anvil" );
			this.StarterItems.Add( "Bottle" );
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
