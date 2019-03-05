using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.EntityGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	partial class WildcardLicenseItem : ModItem {
		public string AttemptToLicenseRandomItem( Player player, out int savings ) {
			var myplayer = player.GetModPlayer<LicensesPlayer>();
			
			int targetRarity = WildcardLicenseItem.ComputeTargetRarityOfLicenseStackSize( this.item.stack );

			string grpName = "Any " + ItemAttributeHelpers.RarityColorText[targetRarity] + " Tier";
			ISet<int> tierItems = EntityGroups.ItemGroups[ grpName ];
			ISet<int> equipments = EntityGroups.ItemGroups[ "Any Equipment" ];
			IList<int> tierEquips = new List<int>( tierItems.Intersect( equipments ) );

			int randItemType;
			string randItemName;

			IDictionary<int, string> idsToNames = ItemIdentityHelpers.NamesToIds
				.ToLookup( kv => kv.Value )
				.ToDictionary( g => g.Key, g => g.First().Key );
			
			do {
				int count = tierEquips.Count();
				if( count == 0 ) {
					savings = 0;
					return null;
				}

				randItemType = tierEquips[ Main.rand.Next( count ) ];
				randItemName = idsToNames[ randItemType ];

				tierEquips.Remove( randItemType );
			} while( myplayer.LicensedItems.Contains( randItemName ) );

			var dummyItem = new Item();
			dummyItem.SetDefaults( randItemType, true );

			int cost = WildcardLicenseItem.ComputeCost( dummyItem, out savings );
			Item selectedItem = player.inventory[ PlayerItemHelpers.VanillaInventorySelectedSlot ];
			int selectedItemStack = selectedItem?.stack ?? 0;
			
			myplayer.LicenseItemByName( randItemName, true );
			
			ItemHelpers.ReduceStack( this.item, cost );
			
			// Failsafes:
			if( selectedItem.type == this.item.type ) {
				int newStackSize = ( selectedItemStack >= cost ) ? ( selectedItemStack - cost ) : 0;

				selectedItem.stack = newStackSize;
				Main.mouseItem.stack = newStackSize;

				if( selectedItem.stack == 0 ) { selectedItem.active = false; }
				if( Main.mouseItem.stack == 0 ) { Main.mouseItem.active = false; }
			}
			
			return randItemName;
		}
	}
}
