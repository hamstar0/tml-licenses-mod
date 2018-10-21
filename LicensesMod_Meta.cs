using HamstarHelpers.Components.Config;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Promises;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Licenses {
	public partial class LicensesMod : Mod {
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

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var config_data = new LicensesConfigData();
			//config_data.SetDefaults();

			LicensesMod.Instance.ConfigJson.SetData( config_data );
			LicensesMod.Instance.ConfigJson.SaveFile();
		}
	}
}
