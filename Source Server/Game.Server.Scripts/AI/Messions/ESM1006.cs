using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class ESM1006 : ExplorationMission
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

            int[] mapIds = { 1103, 1101, 1106 };
            Game.SetMap(GetMapId(mapIds,1103));

            npcCreateParamSimple = new NpcCreateParam(2, 0, 2, 2, 15, 0);
            npcCreateParamNormal = new NpcCreateParam(3, 0, 3, 6, 15, 0);
            npcCreateParamHard = new NpcCreateParam(4, 0, 4, 6, 15, 0);
            npcCreateParamTerror = new NpcCreateParam(4, 0, 4, 6, 15, 0);

            ballIds = new Dictionary<int, int>();
            ballIds.Add(101, 12);
            ballIds.Add(102, 41);
            base.OnPrepareNewSession();
        }
    }
}
