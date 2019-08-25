using HamstarHelpers.Helpers.Debug;
using Nihilism;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Licenses {
	partial class LicensesPlayer : ModPlayer {
		private readonly ISet<ItemDefinition> PendingLoadTrialLicenses = new HashSet<ItemDefinition>();
		private readonly ISet<ItemDefinition> PendingLoadLicenses = new HashSet<ItemDefinition>();

		public readonly ISet<ItemDefinition> TrialLicensedItems = new HashSet<ItemDefinition>();
		public readonly ISet<ItemDefinition> LicensedItems = new HashSet<ItemDefinition>();

		public int LicenseMode = 0;

		////////////////

		public ItemDefinition TrialLicensedItem { get; private set; } = null;

		////

		public override bool CloneNewInstances => false;



		////////////////

		internal void ResetLicenses() {
			foreach( ItemDefinition itemDef in this.LicensedItems ) {
				NihilismAPI.UnsetItemWhitelistEntry( itemDef, true );
			}

			this.TrialLicensedItem = null;
			this.TrialLicensedItems.Clear();
			this.LicensedItems.Clear();
			this.PendingLoadTrialLicenses.Clear();
			this.PendingLoadLicenses.Clear();
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
			
			this.RunLicenseCheckForItem( Main.mouseItem );
		}
	}
}
