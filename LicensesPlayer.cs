using HamstarHelpers.Helpers.DebugHelpers;
using Nihilism;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly ISet<string> PendingLoadTrialLicenses = new HashSet<string>();
		private readonly ISet<string> PendingLoadLicenses = new HashSet<string>();

		public int LicenseMode = 0;

		////////////////

		public ISet<string> TrialLicensedItems { get; private set; }
		public ISet<string> LicensedItems { get; private set; }
		public string TrialLicensedItem { get; private set; }

		////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.TrialLicensedItem = "";
			this.TrialLicensedItems = new HashSet<string>();
			this.LicensedItems = new HashSet<string>();
		}

		////

		internal void ResetLicenses() {
			foreach( string itemName in this.LicensedItems ) {
				NihilismAPI.UnsetItemWhitelistEntry( itemName, true );
			}
			if( string.IsNullOrEmpty(this.TrialLicensedItem) ) {
				NihilismAPI.UnsetItemWhitelistEntry( this.TrialLicensedItem, true );
			}

			this.TrialLicensedItem = "";
			this.TrialLicensedItems = new HashSet<string>();
			this.LicensedItems = new HashSet<string>();
		}


		////////////////
		
		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
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
	}
}
