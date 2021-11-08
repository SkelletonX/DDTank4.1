using Game.Logic.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
    public class BossGuild1291 : APVEGameControl
    {
        public override void OnCreated()
        {
            Game.SetupMissions("1291");
            Game.TotalMissionCount = 1;
        }

        public override void OnPrepated()
        {
            //Game.SessionId = 0;
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
        }
    }
}
