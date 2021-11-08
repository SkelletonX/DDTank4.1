using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
    public class ExplorationGame3 : ExplorationGame
    {
        public override void OnCreated()
        {
            Game.SetupMissions("1003,1003,1003,1003,1003");
            Game.TotalMissionCount = 5;
            base.OnCreated();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }
    }
}
