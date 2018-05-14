using Nihilism;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Licenses {
	class LicensesPlayer : ModPlayer {
		private readonly ISet<string> Licenses = new HashSet<string>();


		////////////////

		public override void Load( TagCompound tag ) {
			this.Licenses.Clear();

			if( tag.ContainsKey("license_count") ) {
				int count = tag.GetInt( "license_count" );

				for( int i=0; i<count; i++ ) {
					string item_name = tag.GetString( "license_" + i );

					this.LicenseItem( item_name );
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound {
				["license_count"] = this.Licenses.Count
			};

			int i = 0;
			foreach( string name in this.Licenses ) {
				tags["license_" + i++] = name;
			}

			return tags;
		}


		////////////////

		public void LicenseItem( string item_name ) {
			this.Licenses.Add( item_name );

			NihilismAPI.SetItemsWhitelistEntry( item_name );
		}
	}
}
