using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
    class ExplorationGame1 : ExplorationGame
    {
        public override void OnCreated()
        {
            Game.SetupMissions("1001,1001,1001,1001,1001");
            Game.TotalMissionCount = 5;
            base.OnCreated();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }
    }
}
