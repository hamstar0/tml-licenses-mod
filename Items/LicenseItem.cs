﻿using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class LotteryLicenseItem : ModItem {
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
			int license_type = LicensesMod.Instance.ItemType<LotteryLicenseItem>();
			int total_licenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { license_type } );
			int needed = LotteryLicenseItem.ComputeNeededLicenses( item );
			
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
			this.DisplayName.SetDefault( "Lottery License" );
			this.Tooltip.SetDefault( "Unlocks a random item of the indicated tier (rarity)" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = 0;
			this.item.rare = 1;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			TooltipLine tip1, tip2;

			tip1 = new TooltipLine( this.mod, "LotteryLicenseItem:Tip1", "Adjust stack size (right-click) to select rarity" );
			
			if( this.item.stack > ItemAttributeHelpers.HighestVanillaRarity ) {
				tip2 = new TooltipLine( this.mod, "LotteryLicenseItem:Tip2", "No rarity indicated." );
				tip2.overrideColor = ItemAttributeHelpers.RarityColor[ -1 ];
			} else {
				string rare_str = ItemAttributeHelpers.RarityLabel[ item.rare ];
				string rare_clr_str = ItemAttributeHelpers.RarityColorText[ item.rare ];

				tip2 = new TooltipLine( this.mod, "LotteryLicenseItem:Tip2", "Unlocks a random "+rare_str+" ("+rare_clr_str+") item" );
				tip2.overrideColor = ItemAttributeHelpers.RarityColor[ item.rare ];
			}

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}
	}
}
