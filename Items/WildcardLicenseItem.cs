﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Messages.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


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
				if( targetRarity > ItemRarityAttributeHelpers.HighestVanillaRarity ) {
					tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Stack size exceeds highest item tier." ) {
						overrideColor = ItemRarityAttributeHelpers.RarityColor[ItemRarityAttributeHelpers.JunkRarity]
					};
				} else {
					string rareStr = ItemRarityAttributeHelpers.RarityLabel[targetRarity];
					string rareClrStr = ItemRarityAttributeHelpers.RarityColorText[targetRarity];

					tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Current item tier: " + rareStr + " (" + rareClrStr + ")" ) {
						overrideColor = ItemRarityAttributeHelpers.RarityColor[targetRarity]
					};
				}
			} else {
				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "No applicable item tier." ) {
					overrideColor = ItemRarityAttributeHelpers.RarityColor[ItemRarityAttributeHelpers.JunkRarity]
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
			ItemDefinition randItemDef = this.AttemptToLicenseRandomItem( player, out savings );

			if( randItemDef == null ) {
				Main.NewText( "No items of the given tier left to license.", Color.Red );
				return false;
			}

			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( oldStack );
			Color color = ItemRarityAttributeHelpers.RarityColor[ targetRarity ];

			string randItemName = ItemAttributeHelpers.GetQualifiedName( randItemDef.Type );
			string msg = randItemName + " licensed";

			if( savings > 0 ) {
				msg += " - "+savings+" discounted";
			}

			PlayerMessages.AddPlayerLabel( player, msg, color, 2 * 60, true );
			Main.NewText( msg, color );

			if( LicensesMod.Config.DebugModeInfo ) {
				LogHelpers.Alert( randItemName + " unlocked" );
			}

			return true;
		}
	}
}
