using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class WildcardLicenseItem : ModItem {
		public static int ComputeMaxCost() {
			var mymod = LicensesMod.Instance;
			
			int maxRarityCost = mymod.Config.WildcardLicenseCostBase;
			maxRarityCost += ItemAttributeHelpers.HighestVanillaRarity * mymod.Config.WildcardLicenseCostRarityMultiplier;
			return maxRarityCost;
		}


		public static int ComputeTargetRarityOfLicenseStackSize( int stack ) {
			var mymod = LicensesMod.Instance;

			return (stack - mymod.Config.WildcardLicenseCostBase) / mymod.Config.WildcardLicenseCostRarityMultiplier;
		}


		public static int ComputeCost( Item item ) {
			var mymod = LicensesMod.Instance;

			float cost = (float)mymod.Config.WildcardLicenseCostBase;
			cost += (float)item.stack * (float)mymod.Config.WildcardLicenseCostRarityMultiplier;

			if( mymod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( item ) ) {
					cost = (float)cost * mymod.Config.LicenseCostArmorMultiplier;
				}
			}

			if( mymod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( item.accessory ) {
					cost = (float)cost * mymod.Config.LicenseCostAccessoryMultiplier;
				}
			}

			return (int)Math.Max( cost, mymod.Config.WildcardLicenseCostBase );
		}


		////////////////
		
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

			if( targetRarity > ItemAttributeHelpers.HighestVanillaRarity ) {
				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Stack size exceeds highest item tier." ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ ItemAttributeHelpers.JunkRarity ]
				};
			} else {
				string rareStr = ItemAttributeHelpers.RarityLabel[ targetRarity ];
				string rareClrStr = ItemAttributeHelpers.RarityColorText[ targetRarity ];

				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Current item tier: "+rareStr+" ("+rareClrStr+")" ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ targetRarity ]
				};
			}
			
			tooltips.Add( tip );
		}


		////////////////
		
		public override bool CanUseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			int maxRarityCost = WildcardLicenseItem.ComputeMaxCost();
			
			return this.item.stack <= maxRarityCost;
		}

		
		public override bool UseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			string randItemName = this.AttemptToLicenseRandomItem( player );

			if( randItemName == null ) {
				Main.NewText( "No items of the given tier left to license.", Color.Red );
				return false;
			}

			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );
			string msg = randItemName + " licensed";
			Color color = ItemAttributeHelpers.RarityColor[targetRarity];

			PlayerMessages.AddPlayerLabel( player, msg, color, 2 * 60, true );
			Main.NewText( msg, color );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( randItemName + " unlocked" );
			}

			return true;
		}


		////////////////

		public string AttemptToLicenseRandomItem( Player player ) {
			var myplayer = player.GetModPlayer<LicensesPlayer>();
			
			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );

			string grpName = "Any " + ItemAttributeHelpers.RarityColorText[targetRarity] + " Tier";
			ISet<int> tierItems = EntityGroups.ItemGroups[ grpName ];
			ISet<int> equipments = EntityGroups.ItemGroups[ "Any Equipment" ];
			IList<int> tierEquips = new List<int>( tierItems.Intersect( equipments ) );

			int randItemType;
			string randItemName;

			IDictionary<int, string> idsToNames = ItemIdentityHelpers.NamesToIds
				.ToLookup( kp => kp.Value )
				.ToDictionary( g => g.Key, g => g.First().Key );
			
			do {
				int count = tierEquips.Count();
				if( count == 0 ) { return null; }

				randItemType = tierEquips[ Main.rand.Next( count ) ];
				randItemName = idsToNames[ randItemType ];

				tierEquips.Remove( randItemType );
			} while( myplayer.LicensedItems.Contains( randItemName ) );

			var dummyItem = new Item();
			dummyItem.SetDefaults( randItemType, true );

			int cost = WildcardLicenseItem.ComputeCost( dummyItem );

			myplayer.LicenseItemByName( randItemName, true );

			ItemHelpers.ReduceStack( this.item, cost );
			
			return randItemName;
		}
	}
}
