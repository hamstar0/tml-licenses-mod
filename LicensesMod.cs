using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Promises;
using System;
using Terraria.ModLoader;


namespace Licenses {
    public partial class LicensesMod : Mod {
		public static LicensesMod Instance { get; private set; }



		////////////////

		public JsonConfig<LicensesConfigData> ConfigJson { get; private set; }
		public LicensesConfigData Config => this.ConfigJson.Data;



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

			if( this.Config.CanUpdateVersion() ) {
				this.Config.UpdateToLatestVersion();
				ErrorLogger.Log( "Licenses updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			LicensesMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof(LicensesAPI), args );
		}
	}
}
