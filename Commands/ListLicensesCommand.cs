using Terraria;
using Terraria.ModLoader;


namespace Licenses.Commands {
	class ListLicensesCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "lic-list";
		public override string Usage => "/"+this.Command;
		public override string Description => "Lists all acquired licenses.";


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
