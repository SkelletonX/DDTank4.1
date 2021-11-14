using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GameObjects
{
    internal class RobotGamePlayer : GamePlayer
    {
        public RobotGamePlayer(int playerId, PlayerInfo info) : base(playerId, "", null, info)
        {
        }
        public void Equip(int equipid, int strength, int compose)
        {
            var item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(equipid), 1, 0);
            item.StrengthenLevel = strength;
            item.AgilityCompose = compose;
            item.AttackCompose = compose;
            item.DefendCompose = compose;
            item.LuckCompose = compose;
            this.EquipBag.AddItem(item, this.EquipBag.FindItemEpuipSlot(item.Template));
        }
    }
}
