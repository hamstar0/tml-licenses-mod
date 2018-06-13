using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses.Items {
	class WildcardLicenseItem : ModItem {
		public static int ComputeNeededLotteryLicenses( Item item ) {
			var mymod = LicensesMod.Instance;
			Item default_of_item = new Item();
			default_of_item.SetDefaults( item.type, true );

			float cost = (float)mymod.Config.WildcardLicenseCostBase;
			cost += (float)default_of_item.rare * (float)mymod.Config.WildcardLicenseCostRarityMultiplier;

			if( mymod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( default_of_item ) ) {
					cost = (float)cost * mymod.Config.LicenseCostArmorMultiplier;
				}
			}

			if( mymod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( default_of_item.accessory ) {
					cost = (float)cost * mymod.Config.LicenseCostAccessoryMultiplier;
				}
			}

			return (int)Math.Max( cost, 1f );
		}


		public static bool AttemptToLicenseItem( Player player, Item item ) {
			int license_type = LicensesMod.Instance.ItemType<WildcardLicenseItem>();
			int total_licenses = ItemFinderHelpers.CountTotalOfEach( player.inventory, new HashSet<int> { license_type } );
			int needed = WildcardLicenseItem.ComputeNeededLotteryLicenses( item );
			
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
			this.Tooltip.SetDefault( "Use to license a random item of the given tier" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = 0;
			this.item.rare = 1;
			this.item.consumable = true;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.useAnimation = 30;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var mymod = (LicensesMod)this.mod;
			TooltipLine tip1, tip2;

			int max_rarity_cost = mymod.Config.WildcardLicenseCostBase;
			max_rarity_cost += ItemAttributeHelpers.HighestVanillaRarity * mymod.Config.WildcardLicenseCostRarityMultiplier;

			tip1 = new TooltipLine( mymod, "LotteryLicense:Tip1", "Adjust stack quantity (right-click item) to select tier (rarity)" );

			if( this.item.stack > max_rarity_cost ) {
				tip2 = new TooltipLine( mymod, "LotteryLicense:Tip2", "Stack size exceeds highest item tier." ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ItemAttributeHelpers.JunkRarity]
				};
			} else {
				string rare_str = ItemAttributeHelpers.RarityLabel[ this.item.stack ];
				string rare_clr_str = ItemAttributeHelpers.RarityColorText[ this.item.stack ];

				tip2 = new TooltipLine( mymod, "LotteryLicense:Tip2", "Current item tier: "+rare_str+" ("+rare_clr_str+")" ) {
					overrideColor = ItemAttributeHelpers.RarityColor[ this.item.stack ]
				};
			}

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}


		////////////////
		
		public override bool CanUseItem( Player player ) {
			var mymod = (LicensesMod)this.mod;
			int max_rarity_cost = mymod.Config.WildcardLicenseCostBase;
			max_rarity_cost += ItemAttributeHelpers.HighestVanillaRarity * mymod.Config.WildcardLicenseCostRarityMultiplier;
			
			return this.item.stack <= max_rarity_cost;
		}


		public override bool UseItem( Player player ) {
Main.NewText("derp?");
			return false;
		}
	}
}
