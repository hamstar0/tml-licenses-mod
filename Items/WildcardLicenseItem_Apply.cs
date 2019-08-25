using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.EntityGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Licenses.Items {
	partial class WildcardLicenseItem : ModItem {
		public ItemDefinition AttemptToLicenseRandomItem( Player player, out int savings ) {
			var myplayer = player.GetModPlayer<LicensesPlayer>();
			
			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );

			string grpName = "Any " + ItemRarityAttributeHelpers.RarityColorText[targetRarity] + " Tier";
			IReadOnlySet<int> tierItems, equipments;

			EntityGroups.TryGetItemGroup( grpName, out tierItems );
			EntityGroups.TryGetItemGroup( "Any Equipment", out equipments );

			IList<int> tierEquips = new List<int>( tierItems.Intersect( equipments ) );

			int randItemType;
			ItemDefinition randItemDef;

			IDictionary<int, string> idsToNames = ItemAttributeHelpers.DisplayNamesToIds
				.ToLookup( kv => kv.Value )
				.ToDictionary( g => g.Key, g => g.First().Key );
			
			do {
				int count = tierEquips.Count();
				if( count == 0 ) {
					savings = 0;
					return null;
				}

				randItemType = tierEquips[ Main.rand.Next( count ) ];
				randItemDef = new ItemDefinition( randItemType );

				tierEquips.Remove( randItemType );
			} while( myplayer.LicensedItems.Contains( randItemDef ) );

			var dummyItem = new Item();
			dummyItem.SetDefaults( randItemType, true );

			int cost = WildcardLicenseItem.ComputeCost( dummyItem, out savings );
			Item selectedItem = player.inventory[ PlayerItemHelpers.VanillaInventorySelectedSlot ];
			int selectedItemStack = selectedItem?.stack ?? 0;
			
			myplayer.LicenseItemByName( randItemDef, true );
			
			ItemHelpers.ReduceStack( this.item, cost );
			
			// Failsafes:
			if( selectedItem.type == this.item.type ) {
				int newStackSize = ( selectedItemStack >= cost ) ? ( selectedItemStack - cost ) : 0;

				selectedItem.stack = newStackSize;
				Main.mouseItem.stack = newStackSize;

				if( selectedItem.stack == 0 ) { selectedItem.active = false; }
				if( Main.mouseItem.stack == 0 ) { Main.mouseItem.active = false; }
			}
			
			return randItemDef;
		}
	}
}
