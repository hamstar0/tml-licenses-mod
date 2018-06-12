using HamstarHelpers.Utilities.Errors;
using Terraria;
using Terraria.ModLoader;


namespace Licenses.Commands {
	class ListLicensesCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "licenseslist"; } }
		public override string Usage { get { return "/licenseslist"; } }
		public override string Description { get { return "Lists all acquired licenses."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (LicensesMod)this.mod;
			//if( !mymod.Config.DebugModeCheat ) {
			//	throw new UsageException( "Cheat mode not enabled." );
			//}

			var myplayer = Main.LocalPlayer.GetModPlayer<LicensesPlayer>();
			string list = string.Join( ", ", myplayer.LicensedItems );

			Main.NewText( list );
		}
	}
}
