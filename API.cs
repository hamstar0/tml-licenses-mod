using HamstarHelpers.Helpers.ItemHelpers;
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

		public static void LicenseItemForCurrentPlayer( Item item, bool playSound ) {
			string itemName = ItemIdentityHelpers.GetQualifiedName( item );
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.LicenseItemByName( itemName, playSound );
		}

		public static void ClearLicencesForCurrentPlayer() {
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.LicensedItems.Clear();
		}

		public static void TrialLicenseItemForCurrentPlayer( Item item, bool playSound ) {
			string itemName = ItemIdentityHelpers.GetQualifiedName( item );
			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

			myplayer.TrialLicenseItemByName( itemName, playSound );
		}
	}
}
