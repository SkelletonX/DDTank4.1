using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class ThirteenSimpleNioBoss : ABrain
    {
        private int m_attackTurn = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            "Sísmica! <br/> Que assustador",
       
            "Largue todas as armas!",
       
            "Veja, você também pode suportar alguns!"
        };

        private static string[] ShootChat = new string[]{
             "Sinta meu poder!",

             "Envie para você, mas miserável",

             "Deixe você suprimir o bom cheiro "
        };

        private static string[] ShootedChat = new string[]{
           "Opa ~~Por que você está me atacando?？<br/>O que estou fazendo?？",

            "Oh ~~ Isso dói. !Por que eu deveria lutar?？<br/>Eu tenho que lutar…"

        };

        private static string[] AddBooldChat = new string[]{
            "Girar ~ <br/> gire ah gire ~ ~",
               
            "Aleluia ~ <br/> Luyaluya ~ ~",
                
            "Que mágica! Nos deu super poderes!"
         
        };

        private static string[] KillAttackChat = new string[]{
            "O rei está no horizonte! !"
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
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);
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
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);
        }


        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();
			((SimpleBoss)Body).SetRelateDemagemRect(-41, -187, 83, 140);

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
