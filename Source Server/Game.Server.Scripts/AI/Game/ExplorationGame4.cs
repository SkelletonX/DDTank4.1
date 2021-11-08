using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
    public class ExplorationGame4 : ExplorationGame
    {
        public override void OnCreated()
        {
            Game.SetupMissions("1004,1004,1004,1004,1004");
            Game.TotalMissionCount = 5;
            base.OnCreated();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }
    }
}
