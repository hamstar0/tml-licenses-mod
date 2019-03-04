using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Licenses {
	public partial class LicensesMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-licenses-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + LicensesConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reload configs outside of single player." );
			}
			if( !LicensesMod.Instance.ConfigJson.LoadFile() ) {
				LicensesMod.Instance.ConfigJson.SaveFile();
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reset to default configs outside of single player." );
			}

			var configData = new LicensesConfigData();
			configData.SetDefaults();

			LicensesMod.Instance.ConfigJson.SetData( configData );
			LicensesMod.Instance.ConfigJson.SaveFile();
		}
	}
}
