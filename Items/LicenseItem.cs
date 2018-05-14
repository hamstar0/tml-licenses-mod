using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class LicenseItem : ModItem {
		public static int ComputeNeededLicenses( Item item ) {
			return item.rare > 0 ? item.rare : 1;
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int license_type = LicensesMod.Instance.ItemType<LicenseItem>();
			int count = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { license_type } );
			int needed = LicenseItem.ComputeNeededLicenses( item );
			
			if( count < needed ) {
				return false;
			}

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.AddItemLicense( item.Name, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, license_type, needed );

			return true;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "License" );
			this.Tooltip.SetDefault( "Select a license and click on an item to license it" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = 0;
			this.item.rare = 1;
		}
	}
}
