using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Licenses.Items {
	class TrialLicenseItem : ModItem {
		public static bool AttemptToTemporaryLicenseItem( Player player, Item item ) {
			int trialLicenseType = ModContent.ItemType<TrialLicenseItem>();
			int totalLicenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { trialLicenseType } );
			int needed = LicenseItem.ComputeCost( item );
			
			if( totalLicenses < needed ) {
				return false;
			}

			var realItemDef = new ItemDefinition( item.type );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.TrialLicenseItemByDefinition( realItemDef, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, trialLicenseType, needed );

			return true;
		}



		////////////////

		public override void SetStaticDefaults() {
			string oneTimeMsg = LicensesMod.Config.TrialLicenseOncePerItem ?
				+'\n' + "Items cannot be trialed more than once"
				: "";

			this.DisplayName.SetDefault( "Trail License" );
			this.Tooltip.SetDefault( "Select an item with this to temporarily license it"
				+'\n'+"Trial licenses only last "+( LicensesMod.Config.TrialLicenseDurationInTicks/60)+" seconds"
				+'\n'+"Only 1 item may be trialed at a time"
				+oneTimeMsg );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = LicensesMod.Config.TrialLicenseCoinCost;
			this.item.rare = 1;
		}


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			for( int i = 0; i < ItemRarityAttributeHelpers.HighestVanillaRarity; i++ ) {
				float cost = (float)LicensesMod.Config.LicenseCostBase;
				cost += (float)i * LicensesMod.Config.LicenseCostRarityMultiplier;

				string str = cost + " trial licenses needed for " + ItemRarityAttributeHelpers.RarityColorText[i] + " items";

				var tip = new TooltipLine( this.mod, "TrialLicense:RarityTip_" + i, str );
				tip.overrideColor = ItemRarityAttributeHelpers.RarityColor[i];

				tooltips.Add( tip );
			}
		}
	}
}
