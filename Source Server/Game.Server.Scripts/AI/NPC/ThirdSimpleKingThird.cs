using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class ThirdSimpleKingThird : ABrain
    {
        private int m_attackTurn = 0;

        private int m_attackCount = 3;

        private int totemType = 0;

        private int npcID = 3003;

        private SimpleNpc TotemBlood = null;

        private int blowTotemID = 3012;

        private int bloodTotemID = 3013;

        private int isSay = 0;

        #region NPC 说话内容
        private static string[] KillPlayerChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg19"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg20"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg21"),
        };
        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg5"),
        };
        private static string[] SeriesShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg3"),
        };
        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg22"),
        };
        private static string[] DieChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg18"),
        };
        private static string[] ShooteChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg23"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg231"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg232"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg233"),
        };
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg15"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg17"),
        };
        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg12"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg13"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg14"),
        };
        private static string[] BlowTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg6"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg8"),
        };
        private static string[] BloodTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg9"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg10"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingThird.msg11"),
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            isSay = 0;
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 100 && player.X < Body.X + 100)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (result)
            {
                KillAttack(Body.X - 200, Body.X + 200);
                return;
            }

            if (m_attackTurn == 0)
            {
                AllAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 1)
            {
                ClearTotem();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 2)
            {
                TotemAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 3)
            {
                int npcCount = ((SimpleBoss)Body).CurrentLivingNpcNum;
                if (npcCount < 8)
                    Summon();
                else
                    BeginAttack();
                m_attackTurn = 0;
                return;
            }
        }

        private void ClearTotem()
        {
            if (TotemBlood != null)
            {
                TotemBlood.PlayMovie("die", 0, 1000);
                Body.CallFuction(new LivingCallBack(NextAttack), 2000);
                return;
            }
            Body.CallFuction(new LivingCallBack(NextAttack), 0);
        }

        private void TotemAttack()
        {
            totemType = Game.Random.Next(0, 2);

            if (totemType == 0)
            {
                int index = Game.Random.Next(0, BlowTotemChat.Length);
                Body.Say(BlowTotemChat[index], 1, 1500);
            }
            else
            {
                int index = Game.Random.Next(0, BloodTotemChat.Length);
                Body.Say(BloodTotemChat[index], 1, 1500);
            }
            Body.PlayMovie("callA", 2500, 3000, new LivingCallBack(CreateTotem));
        }

        private void CreateTotem()
        {
            int delay = GetMaxDelay();
            if (totemType == 0)
            {
                int x = Game.FindBombPlayerX(100);
                SimpleNpc npc = ((PVEGame)Game).CreateNpc(blowTotemID, x, 500, 1, 1, 1);
                ((PVEGame)Game).ChangeMissionDelay(1, delay + 100);
            }
            else
            {
                int rand = Game.Random.Next(0, 2);
                int x = 0;
                if (rand == 0)
                    x = Game.Random.Next(Body.X - 200, Body.X - 100);
                else
                    x = Game.Random.Next(Body.X + 100, Body.X + 200);
                TotemBlood = ((PVEGame)Game).CreateNpc(bloodTotemID, x, 500, 1, 1, 2);
                TotemBlood.SetRelateDemagemRect(-15, -120, 30, 120);
                ((PVEGame)Game).ChangeMissionDelay(2, delay + 100);
            }
        }

        public int GetMaxDelay()
        {
            List<Player> players = Game.GetAllFightPlayers();

            int maxDelay = 0;

            foreach (Player player in players)
            {
                if (player.Delay > maxDelay)
                {
                    maxDelay = player.Delay;
                }
            }
            return maxDelay;
        }

        private void NextAttack()
        {
            if (TotemBlood != null)
            {
                TotemBlood.Die(0);
                TotemBlood = null;
            }
            BeginAttack();
        }

        private void BeginAttack()
        {
            int index = Game.Random.Next(645, 845);
            int idx = Game.Random.Next(0, 2);
            if (idx == 0)
                Body.MoveTo(index, Body.Y, "walk", 2000, new LivingCallBack(PersonalAttack));
            else
                Body.MoveTo(index, Body.Y, "walk", 2000, new LivingCallBack(SeriesAttack));
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.7f;
            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 1500);
            Body.PlayMovie("beatC", 3500, 0);
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "cry", 5500, null);
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 20;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1500);
            Body.PlayMovie("beatC", 3500, 0);
            Body.RangeAttacking(Body.X - 101, Body.X + 101, "cry", 5500, null);
        }

        private void AttackPlayer(Player player, int ShootDelay, int MovieDelay, int value)
        {
            if (player.X > Body.X)
            {
                Body.ChangeDirection(1, MovieDelay - 100);
            }
            else
            {
                Body.ChangeDirection(-1, MovieDelay - 100);
            }

            int mtX = Game.Random.Next(player.X - 10, player.X + 10);

            if (value == 0)
            {
                if (Body.ShootPoint(mtX, player.Y, 55, 1000, 10000, 1, 2.0f, ShootDelay))
                {
                    Body.PlayMovie("beatB", MovieDelay, 0);
                }
            }
            else
            {
                Body.CurrentDamagePlus = 0.8f;
                if (Body.ShootPoint(mtX, player.Y, 54, 1000, 10000, 1, 2.0f, ShootDelay))
                {
                    Body.PlayMovie("beatA", MovieDelay, 0);
                }
            }

        }

        private void PersonalAttack()
        {
            Player target = Game.FindRandomPlayer();
            if (target != null)
            {
                int index = Game.Random.Next(0, ShootChat.Length);
                Body.Say(ShootChat[index], 1, 1500);
                AttackPlayer(target, 4000, 3000, 0);
            }
        }

        private void SeriesAttack()
        {
            List<Player> target = Game.GetAllFightPlayers();
            int attackCount = 0;
            int shootDelay = 4000;
            int movieDelay = 3000;
            foreach (Player player in target)
            {
                if (player.IsLiving)
                {
                    if (attackCount < m_attackCount)
                    {
                        attackCount++;
                        int index = Game.Random.Next(0, SeriesShootChat.Length);
                        Body.Say(SeriesShootChat[index], 1, 1500);
                        AttackPlayer(player, shootDelay, movieDelay, 1);
                        shootDelay += 1800;
                        movieDelay += 1800;
                    }
                }
            }
            if (attackCount < m_attackCount)
            {
                for (int i = attackCount; i < m_attackCount; i++)
                {
                    Player player = Game.FindRandomPlayer();
                    if (player != null)
                    {
                        AttackPlayer(player, shootDelay, movieDelay, 1);
                        shootDelay += 1800;
                        movieDelay += 1800;
                    }
                }
            }
        }
        private void Summon()
        {
            int index = Game.Random.Next(0, CallChat.Length);
            Body.Say(CallChat[index], 1, 1500);
            Body.PlayMovie("callB", 1500, 3000, new LivingCallBack(CreateChild));
        }

        private void CreateChild()
        {
            Point[] Point = { new Point(Body.X + 200, 500), new Point(Body.X - 200, 500), new Point(Body.X + 100, 500), new Point(Body.X - 100, 500) };
            ((SimpleBoss)Body).CreateChild(npcID, Point, 8, 4, 0);
        }


        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 1000);
        }

        public override void OnDiedSay()
        {
            int index = Game.Random.Next(0, DieChat.Length);
            Body.Say(DieChat[index], 1, 1000);
            Game.AddAction(new FocusAction(Body.X, Body.Y, 0, 1000, 1000));
        }

        public override void OnDiedEvent()
        {
            if (TotemBlood != null)
            {
                TotemBlood.Die();
                Game.RemoveLiving(TotemBlood.Id);
                TotemBlood = null;
            }
        }

        public override void OnShootedSay(int delay)
        {
            if (isSay == 0 && Body.IsLiving == true)
            {
                int index = Game.Random.Next(0, ShooteChat.Length);
                Body.Say(ShooteChat[index], 1, delay);
                isSay = 1;
            }
        }
    }
}
