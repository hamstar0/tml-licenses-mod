using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.Timers;
using Licenses.Items;
using Microsoft.Xna.Framework;
using Nihilism;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private void RunLicenseCheckForItem( Item item ) {
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
			string realItemName = ItemIdentityHelpers.GetQualifiedName( item );

			bool isTrialed = this.TrialLicensedItems.Contains( realItemName );
			bool isLicensed = this.LicensedItems.Contains( realItemName )
				|| mymod.Config.FreeStarterItems.Contains( realItemName );

			// When the item is NOT licenses, we apply the licensing effect to the item, if we can:
			switch( this.LicenseMode ) {
			case 1:
				this.LicenseMode = 0;

				if( !isLicensed ) {
					if( LicenseItem.AttemptToLicenseItem( this.player, item ) ) {
						Main.NewText( item.Name + " is now usable.", Color.Lime );
					} else {
						int needed = LicenseItem.ComputeCost( item );
						Main.NewText( "Not enough licenses for " + realItemName + ": " + needed + " needed", Color.Red );
					}
				} else {
					Main.NewText( item.Name + " is already licensed.", Color.Yellow );
				}
				break;

			case 2:
				this.LicenseMode = 0;
				
				if( !isLicensed && (!isTrialed || !mymod.Config.TrialLicenseOncePerItem) ) {
					if( TrialLicenseItem.AttemptToTemporaryLicenseItem( this.player, item ) ) {
						Main.NewText( realItemName + " is now licensed for " + ( LicensesMod.Instance.Config.TrialLicenseDurationInTicks / 60 ) + " seconds.", Color.LimeGreen );
					} else {
						int needed = LicenseItem.ComputeCost( item );
						Main.NewText( "Not enough trial licenses for " + realItemName + ": " + needed + " needed", Color.Red );
					}
				} else {
					if( isLicensed ) {
						Main.NewText( item.Name + " is already licensed.", Color.Yellow );
					} else if( isTrialed ) {
						Main.NewText( item.Name + " has already been trialed.", Color.Yellow );
					}
				}

				break;
			}
		}


		////////////////

		internal void TrialLicenseItemByName( string itemName, bool playSound ) {
			var mymod = (LicensesMod)this.mod;

			if( !string.IsNullOrEmpty(this.TrialLicensedItem) ) {
				Main.NewText( this.TrialLicensedItem + " trial cancelled.", Color.Yellow );

				if( !this.LicensedItems.Contains( itemName ) ) {
					NihilismAPI.UnsetItemWhitelistEntry( this.TrialLicensedItem, true );
				}
			}

			this.TrialLicensedItems.Add( itemName );
			this.TrialLicensedItem = itemName;

			NihilismAPI.SetItemWhitelistEntry( itemName, true );

			Timers.UnsetTimer( "LicensesTrialPeriod" );
			Timers.SetTimer( "LicensesTrialPeriod", mymod.Config.TrialLicenseDurationInTicks, () => {
				var myplayer = (LicensesPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "LicensesPlayer" );

				if( !myplayer.LicensedItems.Contains( itemName ) ) {
					Main.NewText( itemName+" trial has expired.", Color.Yellow );
					NihilismAPI.UnsetItemWhitelistEntry( itemName, true );

					myplayer.TrialLicensedItem = "";
				}
				return false;
			} );

			if( playSound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}

		internal void LicenseItemByName( string itemName, bool playSound ) {
			var mymod = (LicensesMod)this.mod;

			this.LicensedItems.Add( itemName );
			
			NihilismAPI.SetItemWhitelistEntry( itemName, true );

			if( !mymod.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeWhitelistEntry( itemName, true );
			}

			if( playSound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}
	}
}
