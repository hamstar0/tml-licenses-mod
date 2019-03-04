using HamstarHelpers.Helpers.ItemHelpers;
using Terraria;


namespace Licenses {
	public static partial class LicensesAPI {
		public static void ResetPlayerModData( Player player ) {    // <- In accordance with Mod Helpers convention
			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.ResetLicenses();
		}
	}
}
