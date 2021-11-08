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
    public class ThirdSimpleKingFirst : ABrain
    {
        static int m_attackTurn = 0;

        private List<PhysicalObj> m_AllSkill = new List<PhysicalObj>();

        private TurnedLiving healLiving = null;

        static SimpleBoss DeadBoss = null;

        private int isSay = 0;

        static List<SimpleNpc> TotemObj = new List<SimpleNpc>();

        private int[] MoveArea = { 620, 477, 346 };

        private int moveIndex = 2;

        private int totemType = 0;
        
        private int npcID = 3006;

        private int lockTotemID = 3011;

        private int FagTotemID = 3010;

        private Point[] leftPoint = { new Point(219, 234) };

        private Point[] rightPoint = { new Point(1358, 234) };

        private Point[] totemLeftPoint = { new Point(568, 632), new Point(612,636), new Point(647,639), new Point(723,631), new Point(757,642), new Point(793,640),new Point(831,634) };

        private Point[] totemRightPoint = { new Point(873,645), new Point(915,630), new Point(962,639), new Point(1000,638), new Point(1031,632), new Point(1068,632) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg4"),
        };

        private static string[] ShootChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg2"),
        };

        private static string[] AddBooldChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg13"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg14"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg15"),
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg17"),
        };

        private static string[] AddEstateChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg18"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg19"),
        };

        private static string[] KillAttackChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg6"),
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg26"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg27"),
        };

        private static string[] LockTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg8"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg9"),
        };

        private static string[] FagTotemChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg10"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg11"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg12"),
        }; 

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg28"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg281"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg282"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg283"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg284"),
        };

        private static string[] ReliveChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg20"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg21"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg22"), 
        };

        private static string[] ReliveThankChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg23"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg24"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdSimpleKingFirst.msg25"),
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
            m_attackTurn = 0;
            DeadBoss = null;
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
            if (DeadBoss != null && !DeadBoss.IsLiving )
            {
                int index = Game.Random.Next(0, ReliveChat.Length);
                Body.Say(ReliveChat[index], 1, 0);
                Body.PlayMovie("castA", 0, 0);
                Body.CallFuction(new LivingCallBack(ReliveBoss), 2000);
                return;
            }

            if (m_attackTurn == 0)
            {
                PersonalAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 1)
            {
                if (TotemObj.Count == 0)
                {
                    TotemAttack();
                    m_attackTurn++;
                }
                else
                {
                    AllAttack();
                    m_attackTurn += 2;
                }
                return;
            }
            if (m_attackTurn == 2)
            {
                AllAttack();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 3)
            {
                Summon();
                m_attackTurn++;
                return;
            }
            if (m_attackTurn == 4)
            {
                Healing();
                m_attackTurn = 0;
                return;
            }
        }

        private void PersonalAttack()
        {
            foreach (SimpleNpc totem in TotemObj)
            {
                totem.PlayMovie("die", 0, 0);
                totem.Die(1000);
            }
            TotemObj.Clear();
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
            if (totemType == 0)
            {
                x = Game.Random.Next(0, totemLeftPoint.Length);
                TotemObj.Add(((PVEGame)Game).CreateNpc(lockTotemID, totemLeftPoint[x].X, totemLeftPoint[x].Y, 2, -1, -1));
            }
            else
            {
                x = Game.Random.Next(0, totemRightPoint.Length);
                TotemObj.Add(((PVEGame)Game).CreateNpc(FagTotemID, totemRightPoint[x].X, totemRightPoint[x].Y, 2, -1, -1));
            }
            foreach (SimpleNpc totem in TotemObj)
            {
                totem.SetRelateDemagemRect(-5, -65, 10, 44);
            }
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.8f;
            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);
            Body.PlayMovie("castA", 0, 4300, new LivingCallBack(FireSkill));
            Game.AddAction(new FocusAction(800, 600, 0, 1500, 1000));
            Game.AddAction(new FocusAction(800, 600, 3, 2000, 1000));
            Body.RangeAttacking(500, 1100, "castA", 5000, null);
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
                ((PVEGame)Game).RemovePhysicalObj(p,true);
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
                ((PVEGame)Game).RemovePhysicalObj(p,true);
            }
        }

        private void ReliveBoss()
        {
            DeadBoss.Reset();
            DeadBoss.Blood = DeadBoss.Blood / 100 * 20;
            DeadBoss.ActionStr = "raise";
            Game.AddLiving(DeadBoss);
            int index = Game.Random.Next(0, ReliveThankChat.Length);
            DeadBoss.Say(ReliveThankChat[index], 5000, 1000,3000);
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
            healLiving.AddBlood(1000);
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

                Body.CurrentDamagePlus = 0.8f;

                int index = Game.Random.Next(0, ShootChat.Length);
                Body.Say(ShootChat[index], 1, 0);


                int mtX = Game.Random.Next(target.X - 5, target.X + 5);

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
            DeadBoss = (SimpleBoss)Body;
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
