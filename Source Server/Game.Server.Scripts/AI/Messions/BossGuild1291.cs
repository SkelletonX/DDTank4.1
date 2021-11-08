using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerScript.AI.Messions
{
    public class BossGuild1291 : AMissionControl
    {
        private SimpleBoss boss = null;

        private int bossID = 50001;

        private int kill = 0;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            //get boss guild id
            foreach (Player p in Game.Players.Values)
            {
                if (p.PlayerDetail.PlayerCharacter.BossGuildID > 0)
                {
                    bossID = p.PlayerDetail.PlayerCharacter.BossGuildID;
                    break;
                }
            }
            //Game.AddLoadingFile(2, "image/game/effect/8/xiezi.swf", "asset.game.eight.xiezi");
            int[] resources = { bossID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.SetMap(12005);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig config = Game.BaseLivingConfig();
            config.isConsortiaBoss = true;
            boss = Game.CreateBoss(bossID, 273, 610, -1, 1, "born", config);
            boss.Say("Hãy thử sức mạnh của ngươi với ta nào!!!!", 0, 1500);
            boss.SetRelateDemagemRect(boss.NpcInfo.X, boss.NpcInfo.Y, boss.NpcInfo.Width, boss.NpcInfo.Height);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (boss != null && boss.IsLiving == false)
            {
                kill++;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            if (boss != null && boss.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            Game.TakeConsortiaBossAward(Game.IsWin);
        }
    }
}
