using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Services.Timers;
using Licenses.Items;
using Microsoft.Xna.Framework;
using Nihilism;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private void RunLicenseModeForItem( Item item ) {
			if( item == null || item.IsAir ) {
				this.LicenseMode = 0;
				return;
			}
			
			// If the item is a type of License, we engage the respective mode:
			if( item.type == this.mod.ItemType<LicenseItem>() ) {
				this.LicenseMode = 1;
				return;
			}
			if( item.type == this.mod.ItemType<TrialLicenseItem>() ) {
				this.LicenseMode = 2;
				return;
			}

			var mymod = (LicensesMod)this.mod;
			string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );

			bool is_licensed = this.LicensedItems.Contains( real_item_name )
				|| mymod.Config.FreeStarterItems.Contains( real_item_name );

			// When the item is NOT licenses, we apply the licensing effect to the item, if we can:
			switch( this.LicenseMode ) {
			case 1:
				this.LicenseMode = 0;


				if( !is_licensed ) {
					if( LicenseItem.AttemptToLicenseItem( this.player, item ) ) {
						Main.NewText( item.Name + " is now usable.", Color.Lime );
					} else {
						int needed = LicenseItem.ComputeCost( item );
						Main.NewText( "Not enough licenses for " + real_item_name + ": " + needed + " needed", Color.Red );
					}
				} else {
					Main.NewText( item.Name + " is already licensed.", Color.Yellow );
				}
				break;

			case 2:
				this.LicenseMode = 0;
				
				if( !is_licensed ) {
					if( TrialLicenseItem.AttemptToTemporaryLicenseItem( this.player, item ) ) {
						Main.NewText( real_item_name + " is now licensed for " + ( LicensesMod.Instance.Config.TrialLicenseDurationInTicks / 60 ) + " seconds.", Color.LimeGreen );
					} else {
						int needed = LicenseItem.ComputeCost( item );
						Main.NewText( "Not enough trial licenses for " + real_item_name + ": " + needed + " needed", Color.Red );
					}
				} else {
					Main.NewText( item.Name + " is already licensed.", Color.Yellow );
				}

				break;
			}
		}


		////////////////

		internal void TrialLicenseItemByName( string item_name, bool play_sound ) {
			var mymod = (LicensesMod)this.mod;

			if( !string.IsNullOrEmpty(this.TrialLicensedItem) ) {
				Main.NewText( this.TrialLicensedItem + " trial cancelled.", Color.Yellow );
				NihilismAPI.UnsetItemWhitelistEntry( this.TrialLicensedItem, true );
			}

			this.TrialLicensedItem = item_name;

			NihilismAPI.SetItemWhitelistEntry( item_name, true );

			Timers.UnsetTimer( "LicensesTrialPeriod" );
			Timers.SetTimer( "LicensesTrialPeriod", mymod.Config.TrialLicenseDurationInTicks, () => {
				var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();

				if( !myplayer.LicensedItems.Contains( item_name ) ) {
					Main.NewText( item_name+" trial has expired.", Color.Yellow );
					NihilismAPI.UnsetItemWhitelistEntry( item_name, true );

					myplayer.TrialLicensedItem = "";
				}
				return false;
			} );

			if( play_sound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}

		internal void LicenseItemByName( string item_name, bool play_sound ) {
			var mymod = (LicensesMod)this.mod;

			this.LicensedItems.Add( item_name );
			
			NihilismAPI.SetItemWhitelistEntry( item_name, true );

			if( !mymod.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeWhitelistEntry( item_name, true );
			}

			if( play_sound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}
	}
}
