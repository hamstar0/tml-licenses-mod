﻿using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class LicenseItem : ModItem {
		public static int ComputeNeededLicenses( Item item ) {
			var mymod = LicensesMod.Instance;
			Item default_of_item = new Item();
			default_of_item.SetDefaults( item.type, true );

			float cost = (float)mymod.Config.LicenseCostBase + ((float)default_of_item.rare * mymod.Config.LicenseCostRarityMultiplier);
			
			if( mymod.Config.ArmorLicenseCostMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( default_of_item ) ) {
					cost = (float)cost * mymod.Config.ArmorLicenseCostMultiplier;
				}
			}
			if( mymod.Config.AccessoryLicenseCostMultiplier != 1f ) {
				if( default_of_item.accessory ) {
					cost = (float)cost * mymod.Config.AccessoryLicenseCostMultiplier;
				}
			}

			return (int)Math.Max( cost, 1f );
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int license_type = LicensesMod.Instance.ItemType<LicenseItem>();
			int total_licenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { license_type } );
			int needed = LicenseItem.ComputeNeededLicenses( item );
			
			if( total_licenses < needed ) {
				return false;
			}

			string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.SetItemNameLicense( real_item_name, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, license_type, needed );

			return true;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "License" );
			this.Tooltip.SetDefault( "Click on an item with this to license it" );
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