using Game.Logic.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
    public class DragonWarNormalGame : APVEGameControl
    {
        public override void OnCreated()
        {
            base.OnCreated();
            Game.SetupMissions("5101,5102,5103,5104");
            Game.TotalMissionCount = 4;
        }

        public override int CalculateScoreGrade(int score)
        {
            if (score > 800)
            {
                return 3;
            }
            else if (score > 725)
            {
                return 2;
            }
            else if (score > 650)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnGameOverAllSession()
        {
            base.OnGameOverAllSession();
        }
    }
}
