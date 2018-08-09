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
			
			int max_rarity_cost = mymod.Config.WildcardLicenseCostBase;
			max_rarity_cost += ItemAttributeHelpers.HighestVanillaRarity * mymod.Config.WildcardLicenseCostRarityMultiplier;
			return max_rarity_cost;
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
			int target_rarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );
			TooltipLine tip;

			if( target_rarity > ItemAttributeHelpers.HighestVanillaRarity ) {
				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Stack size exceeds highest item tier." ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ ItemAttributeHelpers.JunkRarity ]
				};
			} else {
				string rare_str = ItemAttributeHelpers.RarityLabel[ target_rarity ];
				string rare_clr_str = ItemAttributeHelpers.RarityColorText[ target_rarity ];

				tip = new TooltipLine( mymod, "WildcardLicense:Tier", "Current item tier: "+rare_str+" ("+rare_clr_str+")" ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ target_rarity ]
				};
			}
			
			tooltips.Add( tip );
		}


		////////////////
		
		public override bool CanUseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			int max_rarity_cost = WildcardLicenseItem.ComputeMaxCost();
			
			return this.item.stack <= max_rarity_cost;
		}

		
		public override bool UseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			string rand_item_name = this.AttemptToLicenseRandomItem( player );

			if( rand_item_name == null ) {
				Main.NewText( "No items of the given tier left to license.", Color.Red );
				return false;
			}

			int target_rarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );
			string msg = rand_item_name + " licensed";
			Color color = ItemAttributeHelpers.RarityColor[target_rarity];

			PlayerMessages.AddPlayerLabel( player, msg, color, 2 * 60, true );
			Main.NewText( msg, color );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Licenses - WildcardLicenseItem.UseItem - " + rand_item_name + " unlocked" );
			}

			return true;
		}


		////////////////

		public string AttemptToLicenseRandomItem( Player player ) {
			var myplayer = player.GetModPlayer<LicensesPlayer>();
			
			int target_rarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );

			string grp_name = "Any " + ItemAttributeHelpers.RarityColorText[target_rarity] + " Tier";
			ISet<int> tier_items = EntityGroups.ItemGroups[ grp_name ];
			ISet<int> equipments = EntityGroups.ItemGroups[ "Any Equipment" ];
			IList<int> tier_equips = new List<int>( tier_items.Intersect( equipments ) );

			int rand_item_type;
			string rand_item_name;

			IDictionary<int, string> ids_to_names = ItemIdentityHelpers.NamesToIds
				.ToLookup( kp => kp.Value )
				.ToDictionary( g => g.Key, g => g.First().Key );
			
			do {
				int count = tier_equips.Count();
				if( count == 0 ) { return null; }

				rand_item_type = tier_equips[ Main.rand.Next( count ) ];
				rand_item_name = ids_to_names[ rand_item_type ];

				tier_equips.Remove( rand_item_type );
			} while( myplayer.LicensedItems.Contains( rand_item_name ) );

			var dummy_item = new Item();
			dummy_item.SetDefaults( rand_item_type, true );

			int cost = WildcardLicenseItem.ComputeCost( dummy_item );

			myplayer.SetItemNameLicense( rand_item_name, true );

			ItemHelpers.ReduceStack( this.item, cost );
			
			return rand_item_name;
		}
	}
}
