using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
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

		public bool LicenseMode = false;


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
			if( player.whoAmI != this.player.whoAmI ) { return; }

			if( Main.netMode == 0 ) {
				this.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				this.OnEnterWorldForClient();
			}
		}


		////////////////

		public override void PostUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item item = Main.mouseItem;

			if( item != null && !item.IsAir ) {
				string real_item_name = ItemIdentityHelpers.GetQualifiedName( item );
				
				if( item.type == this.mod.ItemType<LicenseItem>() ) {
					this.LicenseMode = true;
				} else {
					if( this.LicenseMode ) {
						if( !this.LicensedItems.Contains( real_item_name ) ) {
							if( LicenseItem.AttemptToLicenseItem( this.player, item ) ) {
								Main.NewText( item.Name + " is now usable.", Color.Lime );
							} else {
								int needed = LicenseItem.ComputeCost( item );
								Main.NewText( "Not enough licenses for " + item.Name + ": " + needed + " needed", Color.Red );
							}
						} else {
							Main.NewText( item.Name + " is already licensed.", Color.Yellow );
						}

						this.LicenseMode = false;
					}
				}
			} else {
				this.LicenseMode = false;
			}
		}


		////////////////

		internal void SetItemNameLicense( string item_name, bool play_sound ) {
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
