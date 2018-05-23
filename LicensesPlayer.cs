using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
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
		public override bool CloneNewInstances { get { return false; } }

		private readonly ISet<string> LoadLicenses = new HashSet<string>();
		public ISet<string> LicensedItems { get; private set; }

		public bool LicenseMode = false;


		////////////////

		public override void Initialize() {
			this.LicensedItems = new HashSet<string>();
		}

		public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (LicensesMod)this.mod;

			this.LicensedItems.Clear();

			if( tag.ContainsKey("license_count") ) {
				int count = tag.GetInt( "license_count" );
				string[] licenses = new string[count];

				for( int i=0; i<count; i++ ) {
					string item_name = tag.GetString( "license_" + i );

					this.LoadLicenses.Add( item_name );
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				{ "license_count", this.LicensedItems.Count }
			};

			int i = 0;
			foreach( string name in this.LicensedItems ) {
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


		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != this.player.whoAmI || player.whoAmI != Main.myPlayer ) { return; }

			TmlLoadHelpers.AddWorldLoadOncePromise( () => {
				var mymod = (LicensesMod)this.mod;

				// Preload starter item licenses
				foreach( string item_name in mymod.Config.FreeStarterItems ) {
					this.SetItemNameLicense( item_name, false );
				}

				foreach( string item_name in this.LoadLicenses ) {
					this.SetItemNameLicense( item_name, false );
				}
				this.LoadLicenses.Clear();
			} );
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
								int needed = LicenseItem.ComputeNeededLicenses( item );
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
			this.LicensedItems.Add( item_name );
			
			NihilismAPI.SetItemsWhitelistEntry( item_name, true );

			if( play_sound ) {
				Main.PlaySound( SoundID.Unlock, player.position );
			}
		}
	}
}
