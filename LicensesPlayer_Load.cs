using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using Licenses.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (LicensesMod)this.mod;

			this.LicensedItems.Clear();

			if( tag.ContainsKey("license_count") ) {
				int count = tag.GetInt( "license_count" );
				string[] licenses = new string[count];

				for( int i=0; i<count; i++ ) {
					string item_name = tag.GetString( "license_" + i );

					this.PendingLoadLicenses.Add( item_name );
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				{ "license_count", this.LicensedItems.Count }
			};

			int i = 0;
			foreach( string name in this.LicensedItems ) {
				tags["license_" +i++] = name;
			}

			return tags;
		}

		
		////////////////

		private void OnEnterWorldLocal() {
			var mymod = (LicensesMod)this.mod;

			foreach( string item_name in this.PendingLoadLicenses ) {
				this.SetItemNameLicense( item_name, false );
			}
			this.PendingLoadLicenses.Clear();

			TmlLoadHelpers.TriggerCustomPromise( "LicensesOnEnterWorld" );
			TmlLoadHelpers.AddWorldUnloadOncePromise( () => {
				TmlLoadHelpers.ClearCustomPromise( "LicensesOnEnterWorld" );
			} );
		}

		public void OnEnterWorldForSingle() {
			TmlLoadHelpers.AddCustomPromise( "LicensesOnGameModeLoad", () => {
				this.OnEnterWorldLocal();
				return false;
			} );
		}

		public void OnEnterWorldForClient() {
			TmlLoadHelpers.AddCustomPromise( "LicensesOnGameModeLoad", () => {
				this.OnEnterWorldLocal();
				return false;
			} );
		}

		public void OnEnterWorldForServer() {
			TmlLoadHelpers.AddCustomPromise( "LicensesOnGameModeLoad", () => {
				TmlLoadHelpers.TriggerCustomPromise( "LicensesOnEnterWorld" );
				TmlLoadHelpers.AddWorldUnloadOncePromise( () => {
					TmlLoadHelpers.ClearCustomPromise( "LicensesOnEnterWorld" );
				} );
				return false;
			} );
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
	}
}
