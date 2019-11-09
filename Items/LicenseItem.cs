using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Licenses.Items {
	class LicenseItem : ModItem {
		public static int ComputeCost( Item item ) {
			Item defaultOfItem = new Item();
			defaultOfItem.SetDefaults( item.type, true );

			int rarity = Math.Max( 0, defaultOfItem.rare );

			float cost = (float)LicensesMod.Config.LicenseCostBase;
			cost += (float)rarity * LicensesMod.Config.LicenseCostRarityMultiplier;

			if( LicensesMod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( defaultOfItem ) ) {
					cost = (float)cost * LicensesMod.Config.LicenseCostArmorMultiplier;
				}
			}
			if( LicensesMod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( defaultOfItem.accessory ) {
					cost = (float)cost * LicensesMod.Config.LicenseCostAccessoryMultiplier;
				}
			}

			return (int)Math.Max( cost, LicensesMod.Config.LicenseCostBase );
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int licenseType = ModContent.ItemType<LicenseItem>();
			int totalLicenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { licenseType } );
			int needed = LicenseItem.ComputeCost( item );

			if( totalLicenses < needed ) {
				return false;
			}

			var realItemDef = new ItemDefinition( item.type );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.LicenseItemByDefinition( realItemDef, true );

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


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			for( int i=0; i<ItemRarityAttributeHelpers.HighestVanillaRarity; i++ ) {
				float cost = (float)LicensesMod.Config.LicenseCostBase;
				cost += (float)i * LicensesMod.Config.LicenseCostRarityMultiplier;

				string str = cost + " licenses needed for " + ItemRarityAttributeHelpers.RarityColorText[i] + " items";

				var tip = new TooltipLine( this.mod, "License:RarityTip_"+i, str );
				tip.overrideColor = ItemRarityAttributeHelpers.RarityColor[i];

				tooltips.Add( tip );
			}
		}
	}
}
