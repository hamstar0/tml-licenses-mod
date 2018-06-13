﻿using HamstarHelpers.ItemHelpers;
using Terraria;


namespace Licenses {
	public static partial class LicensesAPI {
		public static LicensesConfigData GetModSettings() {
			return LicensesMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			LicensesMod.Instance.ConfigJson.SaveFile();
		}


		////////////////

		public static void LicenseItemForCurrentPlayer( Item item, bool play_sound ) {
			string item_name = ItemIdentityHelpers.GetQualifiedName( item );
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.SetItemNameLicense( item_name, play_sound );
		}

		public static void ClearLicencesForCurrentPlayer() {
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.LicensedItems.Clear();
		}
	}
}
