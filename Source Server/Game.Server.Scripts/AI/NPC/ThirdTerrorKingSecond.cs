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
    public class ThirdTerrorKingSecond : ABrain
    {
        private bool inattack = false;

        private bool isSay = false;

        private Player attackObj = null;

        private int eyeShot = 100;

        private int rightHand = 0;

        private int firstBossDir = -1;

        private int secondBossDir = -1;

        private int beatarea = 0;

        private int leftHand = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg9"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg10"),
        };
        private static string[] KillPlayerChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg14"),
        };
        private static string[] KillAttackChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg15"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg16"),
        };
        private static string[] SeePlayerChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg11"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg12"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg13"),
        };
        private static string[] MoveChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg6"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg8"),
        };
        private static string[] HuffyPlayerChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg17"),
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
            (Game as PVEGame).Param5 = 0;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();

            if (inattack)
            {
                inattack = false;
                ChangeDirection(3);
                int index = Game.Random.Next(0, AllAttackChat.Length);
                Body.Say(AllAttackChat[index], 1, 1000);
                Body.CallFuction(new LivingCallBack(AllAttack), 3000);
                return;
            }

            if ((Game as PVEGame).Param5 == 3)
            {
                int bossCount = 0;
                List<TurnedLiving> living = ((PVEGame)Game).TurnQueue;
                foreach (TurnedLiving turnedLiving in living)
                {
                    if (turnedLiving is SimpleBoss)
                        bossCount++;
                }
                if (bossCount < 2)
                {
                    Game.AddAction(new FocusAction(1300, 600, 0, 0, 1000));
                    SimpleBoss boss = ((PVEGame)Game).CreateBoss(3314, 1300, 600, -1, 0);
                    boss.FallFrom(1300, 702, "fall", 0, 2, 1000);
                    boss.SetRelateDemagemRect(-12, -50, 23, 37);
                    boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorKingSecond.msg1"), 0, 2000, 5000);
                    boss.AddDelay(16);
                    (Game as PVEGame).Param5++;
                    isSay = false;
                    foreach (Living p in Game.FindAppointDeGreeNpc(3))
                    {
                        Game.AddAction(new ShowBloodItem(p.Id, 0, 0));
                    }
                    return;
                }
            }
            (Game as PVEGame).Param5++;
            int beatArea = Game.Random.Next(((SimpleBoss)Body).NpcInfo.MoveMin, ((SimpleBoss)Body).NpcInfo.MoveMax);
            beatArea += eyeShot;
            StartBeat(beatArea);
        }

        private void StartBeat(int beatArea)
        {
            bool val = false;
            int moveArea = 0;
            if (Body.Degree == 1)
                Body.Direction = firstBossDir;
            else
                Body.Direction = secondBossDir;
            attackObj = FindNearestObject();
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving)
                {
                    if ((Math.Abs(Body.Y - player.Y) < 100 && Body.Direction == 1 && player.X < Body.X && Body.X - player.X < 100) || (Math.Abs(Body.Y - player.Y) < 100 && Body.Direction == -1 && player.X > Body.X && player.X - Body.X < 100))
                    {
                        if (Body.Direction == -1)
                            Body.ChangeDirection(1, 0);
                        else
                            Body.ChangeDirection(-1, 0);
                        attackObj = player;
                        val = true;
                        MoveAttack();
                        return;
                    }
                    if (Body.X >= 1300 && player.X >= 1300)
                    {
                        KillAttack();
                        return;
                    }
                }
            }
            if (Body.Direction == -1)
            {
                if (attackObj.X < Body.X && Body.X - attackObj.X < beatArea && Math.Abs(Body.Y - attackObj.Y) < 100)
                {
                    if (Body.X - attackObj.X < eyeShot)
                    {
                        MoveAttack();
                        return;
                    }
                    val = true;
                    moveArea = Body.X - (Body.X - attackObj.X - eyeShot);
                }
            }
            else
            {
                if (attackObj.X > Body.X && attackObj.X - Body.X < beatArea && Math.Abs(Body.Y - attackObj.Y) < 100)
                {
                    if (attackObj.X - Body.X < eyeShot)
                    {
                        MoveAttack();
                        return;
                    }
                    val = true;
                    moveArea = Body.X + (attackObj.X - Body.X - eyeShot);
                }
            }
            if (val)
            {
                Body.MoveTo(moveArea, Body.Y, "walk", 100, new LivingCallBack(MoveAttack));
                return;
            }
            beatarea = beatArea - eyeShot;
            Move();
        }

        private Player FindNearestObject()
        {
            Player nearestPlayer = Game.FindNearestPlayer(Body.X, Body.Y);
            int temp = int.MaxValue;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && Math.Abs(Body.X - player.X) < temp && Math.Abs(Body.Y - player.Y) < 100)
                {
                    if (Body.Direction == 1 && player.X > Body.X || Body.Direction == -1 && player.X < Body.X)
                    {
                        nearestPlayer = player;
                        temp = Math.Abs(Body.X - player.X);
                    }
                }
            }
            return nearestPlayer;
        }

        private void ChangeDirection(int count)
        {
            int direction = Body.Direction;
            for (int i = 0; i < count; i++)
            {
                Body.ChangeDirection(-direction, i * 200 + 100);
                Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
            }
        }

        private void Move()
        {
            int rand = Game.Random.Next(0, 5);
            if (isSay && rand == 0)
            {
                int index = Game.Random.Next(0, MoveChat.Length);
                Body.Say(MoveChat[index], 0, 1000, 2000);
                Body.CallFuction(new LivingCallBack(BeginMove), 3000);
                return;
            }
            isSay = true;
            BeginMove();
        }

        private void BeginMove()
        {
            if (Body.Direction == -1)
            {
                if (Body.X - beatarea >= 0)
                    Body.MoveTo(Body.X - beatarea, Body.Y, "walk", 100);
                else
                {
                    rightHand = beatarea - Body.X;
                    Body.MoveTo(0, Body.Y, "walk", 100, new LivingCallBack(RightHand));
                }
            }
            else
            {
                if (Body.X + beatarea <= 1300)
                    Body.MoveTo(Body.X + beatarea, Body.Y, "walk", 100);
                else
                {
                    leftHand = Body.X + beatarea - 1300;
                    Body.MoveTo(1300, Body.Y, "walk", 100, new LivingCallBack(LeftHand));
                }
            }
        }

        private void RightHand()
        {
            Body.ChangeDirection(1, 0);
            if (Body.Degree == 1)
                firstBossDir = 1;
            else
                secondBossDir = 1;
            isSay = false;
            StartBeat(rightHand + eyeShot);
        }

        private void LeftHand()
        {
            Body.ChangeDirection(-1, 0);
            if (Body.Degree == 1)
                firstBossDir = -1;
            else
                secondBossDir = -1;
            isSay = false;
            StartBeat(leftHand + eyeShot);
        }

        private void MoveAttack()
        {
            int index = Game.Random.Next(0, SeePlayerChat.Length);
            Body.Say(SeePlayerChat[index], 0, 1000);
            Body.PlayMovie("surprise", 0, 2000, new LivingCallBack(MoveToPlayer));
        }

        public void MoveToPlayer()
        {
            int dis = (int)attackObj.Distance(Body.X, Body.Y);
            if (dis > 60)
            {
                dis -= 60;

                if (attackObj.X <= Body.X)
                {
                    Body.MoveTo(Body.X - dis, Body.Y, "walk", 0, new LivingCallBack(StartAttack));
                }
                else
                {
                    Body.MoveTo(Body.X + dis, Body.Y, "walk", 0, new LivingCallBack(StartAttack));
                }
            }
            else
                StartAttack();
        }

        private void StartAttack()
        {
            bool isice = attackObj.IsFrost;
            Body.PlayMovie("beatA", 0, 2000);
            Body.Beat(attackObj, "", 800, 0, 0);
            if (isice)
            {
                int index = Game.Random.Next(0, HuffyPlayerChat.Length);
                Body.PlayMovie("surprise", 2500,1000);
                Body.Say(HuffyPlayerChat[index], 0, 3500,3000);
                Body.PlayMovie("beatB", 6000, 1000);
                Body.Beat(attackObj, "", 6800, 0, 0);
                Body.PlayMovie("beatB", 7500, 1000);
                Body.Beat(attackObj, "", 8300, 0, 0);
                Body.PlayMovie("beatB", 9000, 1000);
                Body.Beat(attackObj, "", 9800, 0, 0);
            }
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.5f;
            Body.PlayMovie("beatB", 0, 2000);
            Body.RangeAttacking(-1, Game.Map.Info.ForegroundWidth + 1, "beatA", 1800, null);
        }

        private void KillAttack()
        {
            Body.ChangeDirection(1, 0);
            Body.PlayMovie("surprise", 0, 0);
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1500, 0);
            Body.PlayMovie("beatB", 2500, 0);
            Body.RangeAttacking(Body.X - 1, Game.Map.Info.ForegroundWidth + 1, "beatB", 3300, null);
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }

        public override void OnShootedSay(int delay)
        {
            inattack = true;
            ((SimpleBoss)Body).Delay = 0;
        }
    }
}
