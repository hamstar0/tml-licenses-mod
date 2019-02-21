using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly ISet<string> PendingLoadLicenses = new HashSet<string>();

		public ISet<string> LicensedItems { get; private set; }
		public string TrialLicensedItem { get; private set; }

		public int LicenseMode = 0;



		////////////////

		public override bool CloneNewInstances => false;

		public override void Initialize() {
			this.LicensedItems = new HashSet<string>();
			this.TrialLicensedItem = "";
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
