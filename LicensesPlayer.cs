using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Services.Timers;
using Licenses.Items;
using Microsoft.Xna.Framework;
using Nihilism;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly ISet<string> PendingLoadLicenses = new HashSet<string>();

		public ISet<string> LicensedItems { get; private set; }

		public int LicenseMode = 0;



		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.LicensedItems = new HashSet<string>();
		}


		////////////////
		
		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnEnterWorldForServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 0 ) {
				this.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnEnterWorldForClient();
			}
		}


		////////////////

		public override void PostUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }
			
			this.RunLicenseModeForItem( Main.mouseItem );
		}


		////////////////

		private void RunLicenseModeForItem( Item item ) {
			if( item == null || item.IsAir ) {
				this.LicenseMode = 0;
				return;
			}

			// If the item is License, we engage the mode:
			if( item.type == this.mod.ItemType<LicenseItem>() ) {
				this.LicenseMode = 1;
				return;
			}
			if( item.type == this.mod.ItemType<TrialLicenseItem>() ) {
				this.LicenseMode = 2;
				return;
			}

			string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );

			// When the item is NOT licenses, we apply the licensing effect to the item, if we can:
			switch( this.LicenseMode ) {
			case 1:
				this.LicenseMode = 0;

				if( !this.LicensedItems.Contains( real_item_name ) ) {
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
				
				if( !this.LicensedItems.Contains( real_item_name ) ) {
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

			NihilismAPI.SetItemWhitelistEntry( item_name, true );

			Timers.SetTimer( "LicensesTrialPeriod", mymod.Config.TrialLicenseDurationInTicks, () => {
				if( !this.LicensedItems.Contains( item_name ) ) {
					NihilismAPI.UnsetItemWhitelistEntry( item_name, true );
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
