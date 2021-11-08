using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic;

namespace GameServerScript.AI.Game
{
    public class ExplorationGame : APVEGameControl
    {
        public int totalMissionCount = 0;
        public string missionIds = string.Empty;
        public override void OnCreated()
        {
        }

        public override void OnPrepated()
        {
            Game.SessionId = 0;
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
