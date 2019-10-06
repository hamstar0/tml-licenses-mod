using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Timers;
using Licenses.Items;
using Microsoft.Xna.Framework;
using Nihilism;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private void RunLicenseCheckForItem( Item item ) {
			if( item == null || item.IsAir ) {
				this.LicenseMode = 0;
				return;
			}
			
			// If the item is a type of License, we engage the respective mode:
			if( item.type == ModContent.ItemType<LicenseItem>() ) {
				this.LicenseMode = 1;
				return;
			}
			if( item.type == ModContent.ItemType<TrialLicenseItem>() ) {
				this.LicenseMode = 2;
				return;
			}

			var mymod = (LicensesMod)this.mod;
			string realItemName = ItemAttributeHelpers.GetQualifiedName( item );
			ItemDefinition itemDef = new ItemDefinition( item.type );

			bool isTrialed = this.TrialLicensedItems.Contains( itemDef );
			bool isLicensed = this.LicensedItems.Contains( itemDef )
				|| mymod.Config.FreeStarterItems.Contains( itemDef );

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

		internal void TrialLicenseItemByDefinition( ItemDefinition itemDef, bool playSound ) {
			var mymod = (LicensesMod)this.mod;

			if( this.TrialLicensedItem != null ) {
				string itemName = ItemAttributeHelpers.GetQualifiedName( this.TrialLicensedItem.Type );

				Main.NewText( itemName + " trial cancelled.", Color.Yellow );

				if( !this.LicensedItems.Contains( itemDef ) ) {
					NihilismAPI.UnsetItemWhitelistEntry( this.TrialLicensedItem, true );
				}
			}

			this.TrialLicensedItems.Add( itemDef );
			this.TrialLicensedItem = itemDef;

			NihilismAPI.SetItemWhitelistEntry( itemDef, true );

			Timers.UnsetTimer( "LicensesTrialPeriod" );
			Timers.SetTimer( "LicensesTrialPeriod", mymod.Config.TrialLicenseDurationInTicks, () => {
				var myplayer = (LicensesPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "LicensesPlayer" );

				if( !myplayer.LicensedItems.Contains( itemDef ) ) {
					string itemName = ItemAttributeHelpers.GetQualifiedName( itemDef.Type );

					Main.NewText( itemName + " trial has expired.", Color.Yellow );
					NihilismAPI.UnsetItemWhitelistEntry( itemDef, true );

					myplayer.TrialLicensedItem = null;
				}
				return false;
			} );

			if( playSound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}

		internal void LicenseItemByDefinition( ItemDefinition itemDef, bool playSound ) {
			var mymod = (LicensesMod)this.mod;

			this.LicensedItems.Add( itemDef );
			
			NihilismAPI.SetItemWhitelistEntry( itemDef, true );

			if( !mymod.Config.FreeRecipes ) {
				NihilismAPI.SetRecipeWhitelistEntry( itemDef, true );
			}

			if( playSound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}
	}
}
