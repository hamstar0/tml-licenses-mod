using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	partial class WildcardLicenseItem : ModItem {
		public static int ComputeMaxCost() {
			var mymod = LicensesMod.Instance;
			
			int maxRarityCost = mymod.Config.WildcardLicenseCostBase;
			maxRarityCost += ItemRarityAttributeHelpers.HighestVanillaRarity * mymod.Config.WildcardLicenseCostRarityMultiplier;
			return maxRarityCost;
		}


		public static int ComputeTargetRarityOfLicenseStackSize( int stack ) {
			var mymod = LicensesMod.Instance;
			int rarity = (stack - mymod.Config.WildcardLicenseCostBase) / mymod.Config.WildcardLicenseCostRarityMultiplier;

			return Math.Max( -1, rarity );
		}


		public static int ComputeCost( Item item, out int savings ) {
			var mymod = LicensesMod.Instance;
			int rarity = Math.Max( 0, item.rare );

			float totalSavings = 0;
			float cost = (float)mymod.Config.WildcardLicenseCostBase;
			cost += (float)item.stack * (float)rarity * (float)mymod.Config.WildcardLicenseCostRarityMultiplier;

			if( mymod.Config.LicenseCostArmorMultiplier != 1f ) {
				if( ItemAttributeHelpers.IsArmor( item ) ) {
					float armorCost = (float)cost * mymod.Config.LicenseCostArmorMultiplier;

					totalSavings += cost - armorCost;
					cost = armorCost;
				}
			}

			if( mymod.Config.LicenseCostAccessoryMultiplier != 1f ) {
				if( item.accessory ) {
					float accCost = (float)cost * mymod.Config.LicenseCostAccessoryMultiplier;

					totalSavings += cost - accCost;
					cost = accCost;
				}
			}

			savings = (int)totalSavings;
			return (int)Math.Max( cost, mymod.Config.WildcardLicenseCostBase );
		}
	}
}
