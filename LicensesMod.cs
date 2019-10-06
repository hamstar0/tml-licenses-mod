using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace Licenses {
    public partial class LicensesMod : Mod {
		public static LicensesMod Instance { get; private set; }



		////////////////

		public LicensesConfig Config => ModContent.GetInstance<LicensesConfig>();



		////////////////

		public LicensesMod() {
			LicensesMod.Instance = this;
		}

		////////////////

		public override void Load() {
			EntityGroups.Enable();

			LoadHooks.AddWorldLoadEachHook( this.LoadGameModeOnWorldLoad );
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
