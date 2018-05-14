using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Items {
	class LicenseItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "License" );
			this.Tooltip.SetDefault( "Right-click and select an item to license it for use" );
		}

		public override void SetDefaults() {
			this.item.maxStack = 1;
			this.item.width = 16;
			this.item.height = 16;
			this.item.value = 0;
			this.item.rare = 1;
		}


		public static void AttemptToLicenseItem( Item item ) {
			int count = ItemFinderHelpers.CountTotalOfEach( Main.LocalPlayer.inventory, new HashSet<int> { item.type } );

			if( count < item.rare ) {
				Main.NewText( "Not enough licenses for " + item.Name + ": " + item.rare + " needed", Color.Red );
				return;
			}

			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();
			myplayer.LicenseItem( item.Name );

			PlayerItemHelpers.RemoveInventoryItemQuantity( Main.LocalPlayer, item.type, item.rare );
		}
	}
}
