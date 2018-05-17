using Licenses.Items;
using System;
using Terraria;


namespace Licenses {
	public static partial class LicensesAPI {
		public static LicensesConfigData GetModSettings() {
			return LicensesMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			LicensesMod.Instance.JsonConfig.SaveFile();
		}


		////////////////

		public static void LicenseItemForCurrentPlayer( Item item, bool play_sound ) {
			string item_name = LicenseItem.GetItemName( item );
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.SetItemNameLicense( item_name, play_sound );
		}
	}
}
