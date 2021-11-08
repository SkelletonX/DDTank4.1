using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class ThirteenTerrorNioBoss : ABrain
    {
        private int m_attackTurn = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            "Địa chấn ! <br/> Thật đáng sợ",
       
            "Đặt hết vũ khí xuống !",
       
            "Nhìn bạn cũng có thể có thể chịu được một số ít!"
        };

        private static string[] ShootChat = new string[]{
             "Cảm nhận sức mạnh của ta !",
                               
             "Gửi cho cậu nhưng viên kheo đau khổ",

             "Cho các ngươi nén mùi lợi hại "
        };

        private static string[] ShootedChat = new string[]{
           "哎呀~~你们为什么要攻击我？<br/>我在干什么？",
                   
            "噢~~好痛!我为什么要战斗？<br/>我必须战斗…"

        };

        private static string[] AddBooldChat = new string[]{
            "Xoay xoay ~ <br/> xoay ah xoay ~ ~",
               
            "Hallelujah ~ <br/> Luyaluya ~ ~",
                
            "Kì diệu quá! Đã đem đến cho ta sức mạnh siêu phàm !"
         
        };

        private static string[] KillAttackChat = new string[]{
            "君临天下！！"
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
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 0 && player.X < 0)
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
                KillAttack(0, 0);
                return;
            }

            if (m_attackTurn == 0)
            {
                NextAttack();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                Healing();
                m_attackTurn++;
            }
            else
            {

                AllAttack();
                m_attackTurn = 0;
            }
        }

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 10;
            int index = Game.Random.Next(0, KillAttackChat.Length);
            Body.Say(KillAttackChat[index], 1, 1000);
            Body.PlayMovie("beatA", 3000, 0);
            Body.RangeAttacking(fx, tx, "cry", 4000, null);
			//((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.5f;

            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);

            Body.PlayMovie("beat", 1000, 0);
            Body.RangeAttacking(Body.X - 4000, Body.X + 4000, "cry", 3000, null);
        }

        private void Healing()
        {
            int index = Game.Random.Next(0, AddBooldChat.Length);
            Body.Say(AddBooldChat[index], 1, 0);
            Body.SyncAtTime = true;
            Body.AddBlood(7500);
            Body.PlayMovie("renew", 1000, 4500);
			//((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);
        }


        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();
			//((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);

            if (target.X > Body.Y)
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

            if (target != null)
            {
                int mtX = Game.Random.Next(target.X - 30, target.X + 30);

                if (Body.ShootPoint(mtX, target.Y, 61, 1400, 10000, 1, 1.5f, 2300))
                {
                    Body.PlayMovie("beat2", 1500, 0);
                }
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
