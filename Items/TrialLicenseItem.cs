using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class TrialLicenseItem : ModItem {
		public static bool AttemptToTemporaryLicenseItem( Player player, Item item ) {
			int trial_license_type = LicensesMod.Instance.ItemType<TrialLicenseItem>();
			int total_licenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { trial_license_type } );
			int needed = LicenseItem.ComputeCost( item );
			
			if( total_licenses < needed ) {
				return false;
			}

			string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.TrialLicenseItemByName( real_item_name, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, trial_license_type, needed );

			return true;
		}


		////////////////

		public override void SetStaticDefaults() {
			var mymod = (LicensesMod)this.mod;

			this.DisplayName.SetDefault( "Trail License" );
			this.Tooltip.SetDefault( "Select an item with this to temporarily license it"
				+'\n'+"Trial licenses only last "+(mymod.Config.TrialLicenseDurationInTicks/60)+" seconds"
				+'\n'+"Only 1 item may be trialed at a time" );
		}

		public override void SetDefaults() {
			var mymod = (LicensesMod)this.mod;

			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = mymod.Config.TrialLicenseCost;
			this.item.rare = 1;
		}
	}
}
