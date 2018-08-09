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



		////////////////

		public JsonConfig<LicensesConfigData> ConfigJson { get; private set; }
		public LicensesConfigData Config { get { return this.ConfigJson.Data; } }



		////////////////

		public LicensesMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<LicensesConfigData>( LicensesConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new LicensesConfigData() );
		}

		////////////////

		public override void Load() {
			LicensesMod.Instance = this;

			this.LoadConfig();

			EntityGroups.Enable();

			Promises.AddWorldLoadEachPromise( this.LoadGameModeOnWorldLoad );
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Licenses config " + LicensesConfigData.ConfigVersion.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Licenses updated to " + LicensesConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
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
	}
}
