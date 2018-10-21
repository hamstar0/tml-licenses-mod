using HamstarHelpers.Components.Config;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Promises;
using System;
using Terraria.ModLoader;


namespace Licenses {
    public partial class LicensesMod : Mod {
		public JsonConfig<LicensesConfigData> ConfigJson { get; private set; }
		public LicensesConfigData Config { get { return this.ConfigJson.Data; } }



		////////////////

		public LicensesMod() {
			this.ConfigJson = new JsonConfig<LicensesConfigData>(
				LicensesConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new LicensesConfigData()
			);
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
				ErrorLogger.Log( "Licenses config " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Licenses updated to " + this.Version.ToString() );
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
