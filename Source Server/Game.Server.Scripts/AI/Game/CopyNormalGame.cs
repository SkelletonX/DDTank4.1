using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class CopyNormalGame : APVEGameControl
    {
        public override void OnCreated()
        {
            Game.SetupMissions("1171,1172,1173,1174,1175,1176");
            //Game.SetupMissions("1174,1175,1176");
            //Game.TotalMissionCount = 2;
            //Game.SetupMissions("1378");
            Game.TotalMissionCount = 6;
        }

        public override void OnPrepated()
        {
            Game.SessionId = 0;
        }

        public override int CalculateScoreGrade(int score)
        {
            if(score > 800)
            {
                return 3;
            }
            else if(score > 725)
            {
                return 2;
            }
            else if(score > 650)
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
