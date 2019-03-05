using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Licenses.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly static object MyValidatorKey;
		public readonly static PromiseValidator EnterWorldValidator;


		////////////////

		static LicensesPlayer() {
			LicensesPlayer.MyValidatorKey = new object();
			LicensesPlayer.EnterWorldValidator = new PromiseValidator( LicensesPlayer.MyValidatorKey );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (LicensesMod)this.mod;

			this.TrialLicensedItems.Clear();
			this.LicensedItems.Clear();

			if( tag.ContainsKey( "trial_license_count" ) ) {
				int count = tag.GetInt( "trial_license_count" );
				for( int i = 0; i < count; i++ ) {
					string itemName = tag.GetString( "trial_license_" + i );
					this.PendingLoadTrialLicenses.Add( itemName );
				}
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "  Loaded for player "+this.player.name+" ("+this.player.whoAmI+") "+count+" trial licensed items..." );
				}
			}

			if( tag.ContainsKey("license_count") ) {
				int count = tag.GetInt( "license_count" );
				for( int i=0; i<count; i++ ) {
					string itemName = tag.GetString( "license_" + i );
					this.PendingLoadLicenses.Add( itemName );
				}
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "  Loaded for player "+this.player.name+" ("+this.player.whoAmI+") "+count+" licensed items..." );
				}
			}
		}

		public override TagCompound Save() {
			int i;
			var tags = new TagCompound {
				{ "trial_license_count", this.TrialLicensedItems.Count },
				{ "license_count", this.LicensedItems.Count }
			};

			i = 0;
			foreach( string name in this.TrialLicensedItems ) {
				tags["trial_license_" + i] = name;
				i++;
			}

			i = 0;
			foreach( string name in this.LicensedItems ) {
				tags["license_" + i] = name;
				i++;
			}

			return tags;
		}

		
		////////////////

		private void OnEnterWorldLocal() {
			var mymod = (LicensesMod)this.mod;

			foreach( string itemName in this.PendingLoadTrialLicenses ) {
				//this.TrialLicenseItemByName( itemName, false );	<- Not the same as Licensing
				this.TrialLicensedItems.Add( itemName );
			}
			foreach( string itemName in this.PendingLoadLicenses ) {
				this.LicenseItemByName( itemName, false );
			}

			this.PendingLoadTrialLicenses.Clear();
			this.PendingLoadLicenses.Clear();
		}

		private void PostOnEnterWorld() {
			Promises.TriggerValidatedPromise( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );

			Promises.AddWorldUnloadOncePromise( () => {
				Promises.ClearValidatedPromise( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );
			} );
		}

		////////////////

		public void OnEnterWorldForSingle() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				var mymod = (LicensesMod)this.mod;
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "Loading player for game mode..." );
				}

				this.OnEnterWorldLocal();
				this.PostOnEnterWorld();
				return false;
			} );
		}

		public void OnEnterWorldForClient() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				var mymod = (LicensesMod)this.mod;
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "Loading player for game mode..." );
				}

				this.OnEnterWorldLocal();
				this.PostOnEnterWorld();
				return false;
			} );
		}

		public void OnEnterWorldForServer() {
			Promises.AddValidatedPromise( LicensesMod.GameModeLoadValidator, () => {
				var mymod = (LicensesMod)this.mod;
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "Loading player for game mode..." );
				}

				this.PostOnEnterWorld();
				return false;
			} );
		}


		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			var mymod = (LicensesMod)this.mod;
			if( mymod.Config.NewPlayerStarterLicenses == 0 ) { return; }

			Item licenses = new Item();
			licenses.SetDefaults( mymod.ItemType<LicenseItem>(), true );
			licenses.stack = mymod.Config.NewPlayerStarterLicenses;

			items.Add( licenses );
		}
	}
}
