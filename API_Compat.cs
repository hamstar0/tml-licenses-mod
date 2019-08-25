using HamstarHelpers.Helpers.TModLoader;
using Terraria;


namespace Licenses {
	public static partial class LicensesAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var myplayer = TmlHelpers.SafelyGetModPlayer<LicensesPlayer>( player );
			myplayer.ResetLicenses();
		}
	}
}
