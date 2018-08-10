using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class LicenseItem : ModItem {
		public static int ComputeCost( Item item ) {
			var mymod = LicensesMod.Instance;
			Item default_of_item = new Item();
			default_of_item.SetDefaults( item.type, true );

			float cost = (float)mymod.Config.LicenseCostBase;
			cost += (float)default_of_item.rare * mymod.Config.LicenseCostRarityMultiplier;
			
			if( mymod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( default_of_item ) ) {
					cost = (float)cost * mymod.Config.LicenseCostArmorMultiplier;
				}
			}
			if( mymod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( default_of_item.accessory ) {
					cost = (float)cost * mymod.Config.LicenseCostAccessoryMultiplier;
				}
			}

			return (int)Math.Max( cost, mymod.Config.LicenseCostBase );
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int license_type = LicensesMod.Instance.ItemType<LicenseItem>();
			int total_licenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { license_type } );
			int needed = LicenseItem.ComputeCost( item );
			
			if( total_licenses < needed ) {
				return false;
			}

			string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.LicenseItemByName( real_item_name, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, license_type, needed );

			return true;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "License" );
			this.Tooltip.SetDefault( "Select an item with this to license it" );
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
