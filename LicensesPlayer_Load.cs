using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Licenses.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator EnterWorldValidator;


		////////////////

		static LicensesPlayer() {
			LicensesPlayer.MyValidatorKey = new object();
			LicensesPlayer.EnterWorldValidator = new PromiseValidator( LicensesMod.MyValidatorKey );
		}


		////////////////

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
				this.LicenseItemByName( item_name, false );
			}
			this.PendingLoadLicenses.Clear();
		}

		private void OnEnterWorldFinish() {
			Promises.TriggerValidatedPromise( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );

			Promises.AddWorldUnloadOncePromise( () => {
				Promises.ClearValidatedPromise( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );
			} );
		}

		////////////////

		public void OnEnterWorldForSingle() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				this.OnEnterWorldLocal();
				this.OnEnterWorldFinish();
				return false;
			} );
		}

		public void OnEnterWorldForClient() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				this.OnEnterWorldLocal();
				this.OnEnterWorldFinish();
				return false;
			} );
		}

		public void OnEnterWorldForServer() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				this.OnEnterWorldFinish();
				return false;
			} );
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcore_death ) {
			var mymod = (LicensesMod)this.mod;
			if( mymod.Config.NewPlayerStarterLicenses == 0 ) { return; }

			Item licenses = new Item();
			licenses.SetDefaults( mymod.ItemType<LicenseItem>(), true );
			licenses.stack = mymod.Config.NewPlayerStarterLicenses;

			items.Add( licenses );
		}
	}
}
