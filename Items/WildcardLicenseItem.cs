using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	partial class WildcardLicenseItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Wildcard License" );
			this.Tooltip.SetDefault( "Use to license a random item of the given tier" + '\n' +
				"Adjust stack quantity (right-click item) to select tier (rarity)" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = 0;
			this.item.rare = 1;
			//this.item.consumable = true;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var mymod = (LicensesMod)this.mod;
			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );
			TooltipLine tip;

			if( targetRarity >= 0 ) {
				if( targetRarity > ItemAttributeHelpers.HighestVanillaRarity ) {
					tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Stack size exceeds highest item tier." ) {
						overrideColor = ItemAttributeHelpers.RarityColor[ItemAttributeHelpers.JunkRarity]
					};
				} else {
					string rareStr = ItemAttributeHelpers.RarityLabel[targetRarity];
					string rareClrStr = ItemAttributeHelpers.RarityColorText[targetRarity];

					tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Current item tier: " + rareStr + " (" + rareClrStr + ")" ) {
						overrideColor = ItemAttributeHelpers.RarityColor[targetRarity]
					};
				}
			} else {
				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "No applicable item tier." ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ItemAttributeHelpers.JunkRarity]
				};
			}
			
			tooltips.Add( tip );
		}


		////////////////
		
		public override bool CanUseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			int maxRarityCost = WildcardLicenseItem.ComputeMaxCost();

			if( this.item.stack > maxRarityCost ) {
				Main.NewText( "Invalid stack size. Stack size must correspond to a valid item ranking value (rarity).", Color.Yellow );
			}
			
			return this.item.stack <= maxRarityCost;
		}

		
		public override bool UseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			int savings;
			int oldStack = this.item.stack;
			string randItemName = this.AttemptToLicenseRandomItem( player, out savings );

			if( randItemName == null ) {
				Main.NewText( "No items of the given tier left to license.", Color.Red );
				return false;
			}

			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( oldStack );
			Color color = ItemAttributeHelpers.RarityColor[ targetRarity ];

			string msg = randItemName + " licensed";

			if( savings > 0 ) {
				msg += " - "+savings+" discounted";
			}

			PlayerMessages.AddPlayerLabel( player, msg, color, 2 * 60, true );
			Main.NewText( msg, color );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( randItemName + " unlocked" );
			}

			return true;
		}
	}
}
