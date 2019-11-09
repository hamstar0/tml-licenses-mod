using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;


namespace Licenses {
	class LicensesCustomPlayer : CustomPlayerData {
		protected override void OnEnter( object data ) {
			if( this.PlayerWho != Main.myPlayer ) { return; }

			var myplayer = TmlHelpers.SafelyGetModPlayer<LicensesPlayer>( this.Player );

			if( Main.netMode == 0 ) {
				myplayer.OnEnterWorldForSingle();
			} else if( Main.netMode == 1 ) {
				myplayer.OnEnterWorldForClient();
			}
		}
	}
}
