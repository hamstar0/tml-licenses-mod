using Licenses.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Rewards.Items;
using HamstarHelpers.Classes.UI.ModConfig;


namespace Licenses {
	class MyFloatInputElement : FloatInputElement { }




	public class LicensesConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( false )]
		public bool DebugModeInfo = false;
		//public bool DebugModeCheat = false;


		[DefaultValue( true )]
		public bool ResetWayfarerShop = true;


		[DefaultValue( true )]
		public bool OverrideNihilismDefaultFilters = true;


		public HashSet<string> FreeStarterItemGroups = new HashSet<string>();
		public HashSet<ItemDefinition> FreeStarterItems = new HashSet<ItemDefinition>();


		[Range( 0, 10000 )]
		[DefaultValue( 10 )]
		public int LicensePackCostInPP = 10;

		[Range( 0, 999 )]
		[DefaultValue( 6 )]
		public int LicensesPerPack = 6;

		[Range( 0, 10000 )]
		[DefaultValue( 10 )]
		public int WildcardLicensePackCostInPP = 10;

		[Range( 0, 999 )]
		[DefaultValue( 18 )]
		public int WildcardLicensesPerPack = 18;


		[Label( "Licenses needed to unlock an item (base amount)" )]
		[Range( 0, 10000 )]
		[DefaultValue( 1 )]
		public int LicenseCostBase = 1;

		[Label( "Multiplier of added license per item rarity grade" )]
		[Range( 0f, 100f )]
		[DefaultValue( 1f / 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float LicenseCostRarityMultiplier = 1f / 2f;

		[Label( "Multiplier of licenses needed for armor" )]
		[Range( 0f, 100f )]
		[DefaultValue( 1f / 3f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float LicenseCostArmorMultiplier = 1f / 3f;

		[Label( "Multiplier of licenses needed for accessories" )]
		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float LicenseCostAccessoryMultiplier = 1f;


		[Label( "Wildcards needed to unlock an item (base amt)" )]
		[DefaultValue( 1 )]
		public int WildcardLicenseCostBase = 1;

		[Label( "Multiplier of added wildcards per item rarity grade" )]
		[Range( 0, 100 )]
		[DefaultValue( 1 )]
		public int WildcardLicenseCostRarityMultiplier = 1;


		[Label( "Material items cost no licenses?" )]
		[DefaultValue( true )]
		public bool FreeMaterials = true;

		[Label( "Placeable items cost no licenses?" )]
		[DefaultValue( true )]
		public bool FreePlaceables = true;

		[Label( "Recipes cost no licenses?" )]
		[DefaultValue( true )]
		public bool FreeRecipes = true;


		[Range( 0, 10000 )]
		[DefaultValue( 10 )]
		public int NewPlayerStarterLicenses = 10;


		[DefaultValue( true )]
		public bool ForceSpawnWayfarer = true;

		[DefaultValue( true )]
		public bool RemoveRewardsGrinding = true;


		[Range( 1, 60 * 60 * 60 * 24 )]
		[DefaultValue( 5 * 60 * 60 )]
		public int TrialLicenseDurationInTicks = 5 * 60 * 60;   // 5 minutes

		[Range( 0, 100000000 )]
		[DefaultValue( 100 * 100 )]
		public int TrialLicenseCoinCost = 100 * 100;    // 1 gold

		[DefaultValue( true )]
		public bool TrialLicenseOncePerItem = true;


		////

		[Header("OBSOLETE SETTINGS BELOW")]
		[Range( 0, 100000000 )]
		public int TrialLicenseCost = 100 * 100;



		////////////////

		public LicensesConfig() {
			this.FreeStarterItemGroups = new HashSet<string> {
				"Any Wood Equipment",
				"Any Cactus Equipment",
				"Any Copper Or Tin Equipment",
				"Any Vanilla Alchemy Ingredient",

				"Any Vanity",
				"Any Dye",
			};

			this.FreeStarterItems = new HashSet<ItemDefinition> {
				new ItemDefinition( ModContent.ItemType<LicenseItem>() ),
				new ItemDefinition( ModContent.ItemType<WildcardLicenseItem>() ),
				new ItemDefinition( ModContent.ItemType<TrialLicenseItem>() ),
				new ItemDefinition( ModContent.ItemType<ShopPackItem>() ),

				new ItemDefinition( ItemID.CopperCoin ),
				new ItemDefinition( ItemID.SilverCoin ),
				new ItemDefinition( ItemID.GoldCoin ),
				new ItemDefinition( ItemID.PlatinumCoin ),

				new ItemDefinition( ItemID.Snowball ),
				new ItemDefinition( ItemID.Blowpipe ),
				new ItemDefinition( ItemID.Seed ),
				new ItemDefinition( ItemID.Rope ),
				new ItemDefinition( ItemID.RopeCoil ),
				new ItemDefinition( ItemID.EmptyBucket ),
				new ItemDefinition( ItemID.WaterBucket ),
				new ItemDefinition( ItemID.LavaBucket ),
				new ItemDefinition( ItemID.HoneyBucket ),
				new ItemDefinition( ItemID.BugNet ),
				new ItemDefinition( ItemID.Glowstick ),
				new ItemDefinition( ItemID.StickyGlowstick ),
				new ItemDefinition( ItemID.BouncyGlowstick ),

				new ItemDefinition( ItemID.LesserHealingPotion ),
				new ItemDefinition( ItemID.LesserManaPotion ),

				new ItemDefinition( ItemID.Gel ),
				new ItemDefinition( ItemID.Mushroom ),
				new ItemDefinition( ItemID.FallenStar ),
				new ItemDefinition( ItemID.Acorn ),

				new ItemDefinition( ItemID.Wood ),
				new ItemDefinition( ItemID.RichMahogany ),
				new ItemDefinition( ItemID.BorealWood ),
				new ItemDefinition( ItemID.PalmWood ),

				new ItemDefinition( ItemID.DirtBlock ),
				new ItemDefinition( ItemID.StoneBlock ),
				new ItemDefinition( ItemID.ClayBlock ),
				new ItemDefinition( ItemID.MudBlock ),
				new ItemDefinition( ItemID.SandBlock ),
				new ItemDefinition( ItemID.SiltBlock ),
				new ItemDefinition( ItemID.SnowBlock ),
				new ItemDefinition( ItemID.IceBlock ),

				new ItemDefinition( ItemID.WoodPlatform ),

				new ItemDefinition( ItemID.WoodWall ),
				new ItemDefinition( ItemID.DirtWall ),
				new ItemDefinition( ItemID.StoneWall ),

				new ItemDefinition( ItemID.Torch ),
				new ItemDefinition( ItemID.Campfire ),
				new ItemDefinition( ItemID.WorkBench ),
				new ItemDefinition( ItemID.WoodenTable ),
				new ItemDefinition( ItemID.WoodenChair ),
				new ItemDefinition( ItemID.WoodenDoor ),
				new ItemDefinition( ItemID.Furnace ),
				new ItemDefinition( ItemID.IronAnvil ),
				new ItemDefinition( ItemID.LeadAnvil ),
				new ItemDefinition( ItemID.Bottle ),
				new ItemDefinition( ItemID.Chest ),
				new ItemDefinition( ItemID.Barrel ),
				new ItemDefinition( ItemID.WoodenDoor ),
				new ItemDefinition( ItemID.MinecartTrack ),

				new ItemDefinition( ItemID.Sunglasses ),
			};
		}

		public override ModConfig Clone() {
			var clone = (LicensesConfig)base.Clone();

			clone.FreeStarterItemGroups = new HashSet<string>( this.FreeStarterItemGroups );
			clone.FreeStarterItems = new HashSet<ItemDefinition>( this.FreeStarterItems );

			return clone;
		}
	}
}
