using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class ESM1001 : ExplorationMission
    {
        public override void OnPrepareNewSession()
        {
            // 加载文件
            Game.AddLoadingFile(1, "bombs/12.swf", "tank.resource.bombs.Bomb12");
            Game.AddLoadingFile(1, "bombs/41.swf", "tank.resource.bombs.Bomb41");

            // 远程 与 近身 NPC 编号列表
            remoteIds = new int[] { 101, 102 };
            livingIds = new int[] { 1001, 1002 };

            Game.LoadResources(livingIds);
            Game.LoadResources(remoteIds);
            Game.LoadNpcGameOverResources(livingIds);
            Game.LoadNpcGameOverResources(remoteIds);

            // 设置 关卡地图
            int[] mapIds = { 1093, 1092, 1108, 1089, 1099 };
            Game.SetMap(GetMapId(mapIds,1093));

            // 设置 刷怪规则
            npcCreateParamSimple = new NpcCreateParam(0, 8, 0, 4, 0, 15);
            npcCreateParamNormal = new NpcCreateParam(0, 12, 0, 4, 0, 15);
            npcCreateParamHard = new NpcCreateParam(0, 16, 0, 4, 0, 15);
            npcCreateParamTerror = new NpcCreateParam(0, 20, 0, 4, 0, 15);

            // 加载 远程NPC使用炸弹
            ballIds = new Dictionary<int, int>();
            ballIds.Add(101, 12);
            ballIds.Add(102, 41);

            base.OnPrepareNewSession();
        }
    }
}
