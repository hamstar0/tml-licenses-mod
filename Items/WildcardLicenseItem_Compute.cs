using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	partial class WildcardLicenseItem : ModItem {
		public static int ComputeMaxCost() {
			int maxRarityCost = LicensesMod.Config.WildcardLicenseCostBase;
			maxRarityCost += ItemRarityAttributeHelpers.HighestVanillaRarity * LicensesMod.Config.WildcardLicenseCostRarityMultiplier;
			return maxRarityCost;
		}


		public static int ComputeTargetRarityOfLicenseStackSize( int stack ) {
			int rarity = (stack - LicensesMod.Config.WildcardLicenseCostBase) / LicensesMod.Config.WildcardLicenseCostRarityMultiplier;

			return Math.Max( -1, rarity );
		}


		public static int ComputeCost( Item item, out int savings ) {
			int rarity = Math.Max( 0, item.rare );

			float totalSavings = 0;
			float cost = (float)LicensesMod.Config.WildcardLicenseCostBase;
			cost += (float)item.stack * (float)rarity * (float)LicensesMod.Config.WildcardLicenseCostRarityMultiplier;

			if( LicensesMod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( item ) ) {
					float armorCost = (float)cost * LicensesMod.Config.LicenseCostArmorMultiplier;

					totalSavings += cost - armorCost;
					cost = armorCost;
				}
			}

			if( LicensesMod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( item.accessory ) {
					float accCost = (float)cost * LicensesMod.Config.LicenseCostAccessoryMultiplier;

					totalSavings += cost - accCost;
					cost = accCost;
				}
			}

			savings = (int)totalSavings;
			return (int)Math.Max( cost, LicensesMod.Config.WildcardLicenseCostBase );
		}
	}
}
