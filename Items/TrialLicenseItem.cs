using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class TrialLicenseItem : ModItem {
		public static bool AttemptToTemporaryLicenseItem( Player player, Item item ) {
			int trialLicenseType = LicensesMod.Instance.ItemType<TrialLicenseItem>();
			int totalLicenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { trialLicenseType } );
			int needed = LicenseItem.ComputeCost( item );
			
			if( totalLicenses < needed ) {
				return false;
			}

			string realTtemName = ItemIdentityHelpers.GetQualifiedName( item );

			var myplayer = player.GetModPlayer<LicensesPlayer>();
			myplayer.TrialLicenseItemByName( realTtemName, true );

			PlayerItemHelpers.RemoveInventoryItemQuantity( player, trialLicenseType, needed );

			return true;
		}



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (LicensesMod)this.mod;
			string oneTimeMsg = mymod.Config.TrialLicenseOncePerItem ?
				+'\n' + "Items cannot be trialed more than once"
				: "";

			this.DisplayName.SetDefault( "Trail License" );
			this.Tooltip.SetDefault( "Select an item with this to temporarily license it"
				+'\n'+"Trial licenses only last "+(mymod.Config.TrialLicenseDurationInTicks/60)+" seconds"
				+'\n'+"Only 1 item may be trialed at a time"
				+oneTimeMsg );
		}

		public override void SetDefaults() {
			var mymod = (LicensesMod)this.mod;

			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = mymod.Config.TrialLicenseCost;
			this.item.rare = 1;
		}


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var mymod = LicensesMod.Instance;

			for( int i = 0; i < ItemAttributeHelpers.HighestVanillaRarity; i++ ) {
				float cost = (float)mymod.Config.LicenseCostBase;
				cost += (float)i * mymod.Config.LicenseCostRarityMultiplier;

				string str = cost + " trial licenses needed for " + ItemAttributeHelpers.RarityColorText[i] + " items";

				var tip = new TooltipLine( this.mod, "TrialLicense:RarityTip_" + i, str );
				tip.overrideColor = ItemAttributeHelpers.RarityColor[i];

				tooltips.Add( tip );
			}
		}
	}
}
