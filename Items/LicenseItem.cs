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
			Item defaultOfItem = new Item();
			defaultOfItem.SetDefaults( item.type, true );

			float cost = (float)mymod.Config.LicenseCostBase;
			cost += (float)defaultOfItem.rare * mymod.Config.LicenseCostRarityMultiplier;
			
			if( mymod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( defaultOfItem ) ) {
					cost = (float)cost * mymod.Config.LicenseCostArmorMultiplier;
				}
			}
			if( mymod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( defaultOfItem.accessory ) {
					cost = (float)cost * mymod.Config.LicenseCostAccessoryMultiplier;
				}
			}

			return (int)Math.Max( cost, mymod.Config.LicenseCostBase );
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int licenseType = LicensesMod.Instance.ItemType<LicenseItem>();
			int totalLicenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { licenseType } );
			int needed = LicenseItem.ComputeCost( item );
			
			if( totalLicenses < needed ) {
				return false;
			}

			string realItemName = ItemIdentityHelpers.GetQualifiedName( item );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.LicenseItemByName( realItemName, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, licenseType, needed );

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
