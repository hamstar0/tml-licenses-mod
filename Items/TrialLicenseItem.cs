﻿using HamstarHelpers.Helpers.ItemHelpers;
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

			Main.NewText( real_item_name+" is now licensed for "+(LicensesMod.Instance.Config.TrialLicenseDurationInTicks/60)+" seconds.", Color.LimeGreen );

			return true;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Trail License" );
			this.Tooltip.SetDefault( "Select an item with this to temporarily license it"
				+'\n'+"Trial licenses only last 2 minutes" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.rare = 1;
		}
	}
}
