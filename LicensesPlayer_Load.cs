using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Licenses.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly static object MyValidatorKey;
		public readonly static CustomLoadHookValidator<object> EnterWorldValidator;


		////////////////

		static LicensesPlayer() {
			LicensesPlayer.MyValidatorKey = new object();
			LicensesPlayer.EnterWorldValidator = new CustomLoadHookValidator<object>( LicensesPlayer.MyValidatorKey );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			var self = this;
			var mymod = (LicensesMod)this.mod;

			this.TrialLicensedItems.Clear();
			this.LicensedItems.Clear();

			if( tag.ContainsKey( "trial_license_key_count" ) ) {
				int count = tag.GetInt( "trial_license_key_count" );
				for( int i = 0; i < count; i++ ) {
					string itemKey = tag.GetString( "trial_license_key_" + i );
					this.PendingLoadTrialLicenses.Add( new ItemDefinition(itemKey) );
				}
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "  Loaded for player "+this.player.name+" ("+this.player.whoAmI+") "+count+" trial licensed items..." );
				}
			}

			if( tag.ContainsKey("license_key_count") ) {
				int count = tag.GetInt( "license_key_count" );
				for( int i=0; i<count; i++ ) {
					string itemKey = tag.GetString( "license_key_" + i );
					this.PendingLoadLicenses.Add( new ItemDefinition(itemKey) );
				}
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Alert( "  Loaded for player "+this.player.name+" ("+this.player.whoAmI+") "+count+" licensed items..." );
				}
			}
		}

		public override TagCompound Save() {
			int i;
			var tags = new TagCompound {
				{ "trial_license_key_count", this.TrialLicensedItems.Count },
				{ "license_key_count", this.LicensedItems.Count }
			};

			i = 0;
			foreach( ItemDefinition itemDef in this.TrialLicensedItems ) {
				tags["trial_license_key_" + i] = itemDef.ToString();
				i++;
			}

			i = 0;
			foreach( ItemDefinition itemDef in this.LicensedItems ) {
				tags["license_key_" + i] = itemDef.ToString();
				i++;
			}

			return tags;
		}

		
		////////////////

		private void OnEnterWorldLocal() {
			var mymod = (LicensesMod)this.mod;

			foreach( ItemDefinition itemDef in this.PendingLoadTrialLicenses ) {
				//this.TrialLicenseItemByName( itemName, false );	<- Not the same as Licensing
				this.TrialLicensedItems.Add( itemDef );
			}
			foreach( ItemDefinition itemDef in this.PendingLoadLicenses ) {
				this.LicenseItemByDefinition( itemDef, false );
			}

			this.PendingLoadTrialLicenses.Clear();
			this.PendingLoadLicenses.Clear();
		}

		private void PostOnEnterWorld() {
			CustomLoadHooks.TriggerHook( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );

			LoadHooks.AddWorldUnloadOnceHook( () => {
				CustomLoadHooks.ClearHook( LicensesPlayer.EnterWorldValidator, LicensesPlayer.MyValidatorKey );
			} );
		}

		////////////////

		public void OnEnterWorldForSingle() {
			CustomLoadHooks.AddHook( LicensesMod.GameModeLoadValidator, (_) => {
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
			CustomLoadHooks.AddHook( LicensesMod.GameModeLoadValidator, (_) => {
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
			CustomLoadHooks.AddHook( LicensesMod.GameModeLoadValidator, (_) => {
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
