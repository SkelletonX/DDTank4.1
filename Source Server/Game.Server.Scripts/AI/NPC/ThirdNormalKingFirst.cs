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
    public class ThirdNormalKingFirst : ABrain
    {
        private List<PhysicalObj> m_AllSkill = new List<PhysicalObj>();

        private TurnedLiving healLiving = null;

        private int isSay = 0;

        private int[] MoveArea = { 620, 477, 346 };

        private int moveIndex = 2;

        private int totemType = 0;

        private int npcID = 3106;

        private int lockTotemID = 3111;

        private int FagTotemID = 3110;

        private Point[] leftPoint = { new Point(219, 234) };

        private Point[] rightPoint = { new Point(1358, 234) };

        private Point[] totemLeftPoint = { new Point(568, 632), new Point(612, 636), new Point(647, 639), new Point(723, 631), new Point(757, 642), new Point(793, 640), new Point(831, 634) };

        private Point[] totemRightPoint = { new Point(873, 645), new Point(915, 630), new Point(962, 639), new Point(1000, 638), new Point(1031, 632), new Point(1068, 632) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg4"),
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg2"),
        };

        private static string[] AddBooldChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg13"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg14"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg15"),
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg17"),
        };

        private static string[] AddEstateChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg18"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg19"),
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg6"),
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg26"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg27"),
        };

        private static string[] LockTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg8"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg9"),
        };

        private static string[] FagTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg10"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg11"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg12"),
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg28"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg281"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg282"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg283"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg284"),
        };

        private static string[] ReliveChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg20"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg21"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg22"), 
        };

        private static string[] ReliveThankChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg23"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg24"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalKingFirst.msg25"),
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
            (Game as PVEGame).Param6 = 0;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Game.AddAction(new ShowBloodItem(Body.Id, 0, 0));
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && (player.X > 0 && player.X < 350 || player.X > 1200 && player.X < 1600))
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
                KillAttack(0, 350);
                KillAttack(1200, 1600);
                return;
            }

            RandMove();
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 20;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.PlayMovie("castA", 1000, 0);
            Game.AddAction(new PlaySoundAction("078", 0));
            Body.RangeAttacking(fx, tx, "castA", 4000, null);
        }

        private void RandMove()
        {
            int index;
            do
            {
                index = Game.Random.Next(0, MoveArea.Length);
            } while (moveIndex == index);
            moveIndex = index;
            if (Body.Y > MoveArea[index])
            {
                Body.JumpTo(Body.X, MoveArea[index], null, 1500, 0, new LivingCallBack(NextAction));
                Body.PlayMovie("walk", 1000, 0);
            }
            else
            {
                Body.FallFrom(Body.X, MoveArea[index], null, 1500, 0, 15, new LivingCallBack(NextAction));
                Body.PlayMovie("walk", 1000, 0);
            }

        }

        private void NextAction()
        {
            //先判断另一个BOSS有没有死亡
            if ((Game as PVEGame).ParamLiving != null && !(Game as PVEGame).ParamLiving.IsLiving)
            {
                int index = Game.Random.Next(0, ReliveChat.Length);
                Body.Say(ReliveChat[index], 1, 0);
                Body.PlayMovie("castA", 0, 0);
                Body.CallFuction(new LivingCallBack(ReliveBoss), 2000);
                return;
            }

            if ((Game as PVEGame).Param6 == 0)
            {
                PersonalAttack();
                (Game as PVEGame).Param6++;
                return;
            }
            if ((Game as PVEGame).Param6 == 1)
            {
                int totemCount = 0;
                foreach (Living p in Game.FindAppointDeGreeNpc(3))
                {
                    totemCount++;
                }
                if (totemCount == 0)
                {
                    TotemAttack();
                    (Game as PVEGame).Param6++;
                }
                else
                {
                    AllAttack();
                    (Game as PVEGame).Param6 += 2;
                }
                return;
            }
            if ((Game as PVEGame).Param6 == 2)
            {
                AllAttack();
                (Game as PVEGame).Param6++;
                return;
            }
            if ((Game as PVEGame).Param6 == 3)
            {
                Summon();
                (Game as PVEGame).Param6++;
                return;
            }
            if ((Game as PVEGame).Param6 == 4)
            {
                Healing();
                (Game as PVEGame).Param6 = 0;
                return;
            }
        }

        private void PersonalAttack()
        {
            foreach (Living p in Game.FindAppointDeGreeNpc(3))
            {
                (p as SimpleNpc).PlayMovie("die", 0, 0);
                (p as SimpleNpc).Die(1000);
            }
            NextAttack();
        }

        private void TotemAttack()
        {
            totemType = Game.Random.Next(0, 2);
            if (totemType == 0)
            {
                int index = Game.Random.Next(0, LockTotemChat.Length);
                Body.Say(LockTotemChat[index], 1, 0);
            }
            else
            {
                int index = Game.Random.Next(0, FagTotemChat.Length);
                Body.Say(FagTotemChat[index], 1, 0);
            }
            Body.PlayMovie("call", 0, 2000);
            Body.CallFuction(new LivingCallBack(CreateTotem), 3000);
        }

        private void CreateTotem()
        {
            int x;
            SimpleNpc totem = null;
            if (totemType == 0)
            {
                x = Game.Random.Next(0, totemLeftPoint.Length);
                totem = ((PVEGame)Game).CreateNpc(lockTotemID, totemLeftPoint[x].X, totemLeftPoint[x].Y, 2, -1, -1);
            }
            else
            {
                x = Game.Random.Next(0, totemRightPoint.Length);
                totem = ((PVEGame)Game).CreateNpc(FagTotemID, totemRightPoint[x].X, totemRightPoint[x].Y, 2, -1, -1);
            }
            totem.SetRelateDemagemRect(-5, -65, 10, 44);
            totem.Degree = 3;
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.8f;
            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);
            Body.PlayMovie("castA", 0, 4300, new LivingCallBack(FireSkill));
            Game.AddAction(new FocusAction(800, 600, 0, 1500, 1000));
            Game.AddAction(new FocusAction(800, 600, 3, 2000, 1000));
            Body.RangeAttacking(400, 1200, "castA", 5000, null);
        }

        private void FireSkill()
        {
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving)
                {
                    m_AllSkill.Add(((PVEGame)Game).CreatePhysicalObj(player.X, player.Y, "Dici", "game.crazytank.assetmap.Dici", "1", 1, 0));
                }
            }
            Body.CallFuction(new LivingCallBack(ClearFire), 1000);
        }

        private void ClearFire()
        {
            PhysicalObj[] physics = null;

            physics = (((PVEGame)Game).FindPhysicalObjByName("Dici"));

            foreach (PhysicalObj p in physics)
            {
                ((PVEGame)Game).RemovePhysicalObj(p, true);
            }
        }

        private void Summon()
        {
            int count = 0;
            List<SimpleNpc> child = ((SimpleBoss)Body).Child;
            foreach (SimpleNpc npc in child)
            {
                if (npc.IsLiving)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                int index = Game.Random.Next(0, CallChat.Length);
                Body.Say(CallChat[index], 1, 0);
                Body.PlayMovie("call", 0, 2000);
                Body.CallFuction(new LivingCallBack(CreateChild), 3000);
            }
            else
            {
                int index = Game.Random.Next(0, AddEstateChat.Length);
                Body.Say(AddEstateChat[index], 1, 0);
                if (((SimpleBoss)Body).Degree == 2)
                    Game.AddAction(new FocusAction(1400, 110, 0, 2000, 3500));
                else
                    Game.AddAction(new FocusAction(0, 110, 0, 2000, 3500));
                Body.PlayMovie("call", 0, 0);
                Body.CallFuction(new LivingCallBack(CreateEstate), 3000);
            }
        }

        private void CreateChild()
        {
            if (((SimpleBoss)Body).Degree == 1)
                ((SimpleBoss)Body).CreateChild(npcID, leftPoint, 1, 1, 0, 0);
            else
                ((SimpleBoss)Body).CreateChild(npcID, rightPoint, 1, 1, 0, 0);
        }

        private void CreateEstate()
        {
            List<SimpleNpc> child = ((SimpleBoss)Body).Child;
            foreach (SimpleNpc npc in child)
            {
                if (!npc.IsLiving)
                    continue;
                npc.ChangeDamage(npc.BaseDamage);
                m_AllSkill.Add(((PVEGame)Game).CreatePhysicalObj(npc.X - 10, npc.Y, "Attack", "game.crazytank.assetmap.Buff_powup", "1", 1, 0));
            }
            Body.CallFuction(new LivingCallBack(ClearAttack), 1500);
        }

        private void ClearAttack()
        {
            PhysicalObj[] physics = null;

            physics = (((PVEGame)Game).FindPhysicalObjByName("Attack"));

            foreach (PhysicalObj p in physics)
            {
                ((PVEGame)Game).RemovePhysicalObj(p, true);
            }
        }

        private void ReliveBoss()
        {
            (Game as PVEGame).ParamLiving.Reset();
            (Game as PVEGame).ParamLiving.Blood = (Game as PVEGame).ParamLiving.Blood / 100 * 20;
            (Game as PVEGame).ParamLiving.ActionStr = "raise";
            Game.AddLiving((Game as PVEGame).ParamLiving);
            int index = Game.Random.Next(0, ReliveThankChat.Length);
            (Game as PVEGame).ParamLiving.Say(ReliveThankChat[index], 5000, 1000, 3000);
        }

        private void Healing()
        {
            int index = Game.Random.Next(0, AddBooldChat.Length);
            Body.Say(AddBooldChat[index], 1, 0);
            Body.SyncAtTime = true;
            List<TurnedLiving> livinglist = Game.TurnQueue;
            healLiving = null;
            int blood = int.MaxValue;
            foreach (TurnedLiving living in livinglist)
            {
                if (living.Blood < blood && (living is SimpleBoss))
                {
                    blood = living.Blood;
                    healLiving = living;
                }
            }
            if (healLiving != null)
            {
                Body.PlayMovie("castA", 0, 2000, new LivingCallBack(AddBloodLiving));
                Game.AddAction(new FocusAction(healLiving.X - 200, healLiving.Y - 200, 0, 1500, 3000));
            }
        }

        private void AddBloodLiving()
        {
            healLiving.AddBlood(2000);
        }

        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();
            if (target != null)
            {
                if (target.X > Body.X)
                {
                    Body.ChangeDirection(1, 800);
                }
                else
                {
                    Body.ChangeDirection(-1, 800);
                }

                int index = Game.Random.Next(0, ShootChat.Length);
                Body.Say(ShootChat[index], 1, 0);


                int mtX = Game.Random.Next(target.X - 10, target.X + 10);

                if (Body.ShootPoint(mtX, target.Y, 54, 1000, 10000, 1, 1, 1900))
                {
                    Body.PlayMovie("beatA", 1500, 0);
                }
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }


        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = Game.Random.Next(0, KillPlayerChat.Length);
            Body.Say(KillPlayerChat[index], 1, 0, 2000);
        }

        public override void OnDiedSay()
        {
            base.OnDiedSay();
        }

        public override void OnDiedEvent()
        {
            base.OnDiedEvent();
            (Game as PVEGame).ParamLiving = Body;
        }

        public override void OnShootedSay(int delay)
        {
            if (isSay == 0 && Body.IsLiving == true)
            {
                int index = Game.Random.Next(0, ShootedChat.Length);
                Body.Say(ShootedChat[index], 1, delay);
                isSay = 1;
            }
        }
    }
}
