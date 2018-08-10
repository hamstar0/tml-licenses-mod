using Licenses.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses {
	class LicensesNPC : GlobalNPC {
		public override void SetupShop( int npc_type, Chest shop, ref int next_slot ) {
			var mymod = (LicensesMod)this.mod;
			
			if( npc_type == NPCID.Merchant ) {
				Item item = new Item();
				item.SetDefaults( mymod.ItemType<TrialLicenseItem>(), true );

				shop.item[next_slot++] = item;
			}
		}
	}
}
