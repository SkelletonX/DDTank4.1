using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Game
{
     public class ExplorationGame5 : ExplorationGame
    {
        //TODO 这里涉及两种类型的地图，稍后做
        public override void OnCreated()
        {
            Game.SetupMissions(GetMissionIds());
            Game.TotalMissionCount = 5;
            base.OnCreated();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }

        private string GetMissionIds()
        {
            int[] missionIds = { 1005, 1006 };
            int index = Game.Random.Next(0, missionIds.Length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(missionIds[index] + ",");
            }
            string missionIdsList = sb.Remove(sb.Length - 1, 1).ToString();
            return missionIdsList;
        }
    }
}
