using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;
using Terraria.ModLoader.Config;


namespace Licenses {
	public static partial class LicensesAPI {
		public static void TrialLicenseItemForCurrentPlayer( Item item, bool playSound ) {
			var itemDef = new ItemDefinition( item.type );
			var myplayer = (LicensesPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, LicensesMod.Instance, "LicensesPlayer" );

			myplayer.TrialLicenseItemByName( itemDef, playSound );
		}

		public static void LicenseItemForCurrentPlayer( Item item, bool playSound ) {
			var itemDef = new ItemDefinition( item.type );
			var myplayer = (LicensesPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, LicensesMod.Instance, "LicensesPlayer" );

			myplayer.LicenseItemByName( itemDef, playSound );
		}
	}
}
