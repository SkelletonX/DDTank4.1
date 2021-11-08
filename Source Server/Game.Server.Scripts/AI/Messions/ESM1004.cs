using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class ESM1004:ExplorationMission
    {
        public override void OnPrepareNewSession()
        {
            Game.AddLoadingFile(1, "bombs/12.swf", "tank.resource.bombs.Bomb12");
            Game.AddLoadingFile(1, "bombs/41.swf", "tank.resource.bombs.Bomb41");

            remoteIds = new int[] { 101, 102 };
            livingIds = new int[] { 1001, 1002 };

            Game.LoadResources(livingIds);
            Game.LoadResources(remoteIds);

            Game.LoadNpcGameOverResources(livingIds);
            Game.LoadNpcGameOverResources(remoteIds);

            int[] mapIds = { 1091, 1095, 1109, 1090, 1087, 1097 };
            Game.SetMap(GetMapId(mapIds,1091));

            npcCreateParamSimple = new NpcCreateParam(2, 10, 1, 5, 5, 10);
            npcCreateParamNormal = new NpcCreateParam(3, 15, 1, 5, 5, 10);
            npcCreateParamHard = new NpcCreateParam(4, 20, 1, 5, 5, 10);
            npcCreateParamTerror = new NpcCreateParam(5, 25, 1, 5, 5, 10);

            ballIds = new Dictionary<int, int>();
            ballIds.Add(101, 12);
            ballIds.Add(102, 41);
            base.OnPrepareNewSession();
        }
    }
}
