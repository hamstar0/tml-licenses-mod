using HamstarHelpers.TmlHelpers;
using Licenses.Items;
using Microsoft.Xna.Framework;
using Nihilism;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Licenses {
	class LicensesPlayer : ModPlayer {
		private readonly ISet<string> Licenses = new HashSet<string>();

		public bool LicenseMode = false;


		////////////////

		public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (LicensesMod)this.mod;

			this.Licenses.Clear();

			if( tag.ContainsKey("license_count") ) {
				int count = tag.GetInt( "license_count" );
				string[] licenses = new string[count];

				for( int i=0; i<count; i++ ) {
					string item_name = tag.GetString( "license_" + i );

					licenses[i] = item_name;
				}

				TmlLoadHelpers.AddWorldLoadPromise( () => {
					foreach( string item_name in licenses ) {
						self.AddItemLicense( item_name, false );
					}
				} );
			} else {
				// Preload starter item licenses
				TmlLoadHelpers.AddWorldLoadPromise( () => {
					foreach( string item_name in mymod.Config.FreeStarterItems ) {
						self.AddItemLicense( item_name, false );
					}
				} );
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				{ "license_count", this.Licenses.Count }
			};

			int i = 0;
			foreach( string name in this.Licenses ) {
				tags["license_" + i++] = name;
			}

			return tags;
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items ) {
			var mymod = (LicensesMod)this.mod;
			if( mymod.Config.NewPlayerStarterLicenses == 0 ) { return; }

			Item licenses = new Item();
			licenses.SetDefaults( mymod.ItemType<LicenseItem>(), true );
			licenses.stack = mymod.Config.NewPlayerStarterLicenses;
			
			items.Add( licenses );
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item item = Main.mouseItem;

			if( item != null && !item.IsAir ) {
				if( item.type == this.mod.ItemType<LicenseItem>() ) {
					this.LicenseMode = true;
				} else {
					if( this.LicenseMode ) {
						if( !this.Licenses.Contains( item.Name ) ) {
							if( LicenseItem.AttemptToLicenseItem( this.player, item ) ) {
								Main.NewText( item.Name + " is now usable.", Color.Lime );
							} else {
								Main.NewText( "Not enough licenses for " + item.Name + ": " + LicenseItem.ComputeNeededLicenses(item) + " needed", Color.Red );
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

		public void AddItemLicense( string item_name, bool play_sound ) {
			this.Licenses.Add( item_name );
			
			NihilismAPI.SetItemsWhitelistEntry( item_name, true );

			if( play_sound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}
	}
}
