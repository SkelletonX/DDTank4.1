using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class ESM1005 : ExplorationMission
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

            int[] mapIds = { 1097, 1112, 1105 };
            Game.SetMap(GetMapId(mapIds,1097));

            npcCreateParamSimple = new NpcCreateParam(0, 12, 0, 6, 0, 15);
            npcCreateParamNormal = new NpcCreateParam(0, 18, 0, 6, 0, 15);
            npcCreateParamHard = new NpcCreateParam(0, 24, 0, 6, 0, 15);
            npcCreateParamTerror = new NpcCreateParam(0, 30, 0, 6, 0, 15);

            ballIds = new Dictionary<int, int>();
            ballIds.Add(101, 12);
            ballIds.Add(102, 41);
            base.OnPrepareNewSession();
        }
    }
}
