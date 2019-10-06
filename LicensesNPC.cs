using Licenses.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Licenses {
	class LicensesNPC : GlobalNPC {
		public override void SetupShop( int npcType, Chest shop, ref int nextSlot ) {
			var mymod = (LicensesMod)this.mod;
			
			if( npcType == NPCID.Merchant ) {
				Item item = new Item();
				item.SetDefaults( ModContent.ItemType<TrialLicenseItem>(), true );

				shop.item[nextSlot++] = item;
			}
		}
	}
}
