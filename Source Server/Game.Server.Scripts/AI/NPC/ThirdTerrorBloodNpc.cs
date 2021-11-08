using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;
using Game.Logic.Actions;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class ThirdTerrorBloodNpc : ABrain
    {

        //private bool isBegin = false;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            //if (!isBegin)
            //{
            //    isBegin = true;
            //    return;
            //}
            Game.AddAction(new FocusAction(Body.X, Body.Y, 0, 150, 1000));
            Body.CallFuction(new LivingCallBack(AddBlood), 1500);
        }

        public void AddBlood()
        {
            List<Living> listliving = Game.GetLivedLivings();
            foreach (Living living in listliving)
            {
                if (living.IsLiving)
                {
                    living.AddBlood(4000);
                }
            }
            List<TurnedLiving> turnedliving = Game.TurnQueue;
            foreach (TurnedLiving tliving in turnedliving)
            {
                if (tliving.IsLiving && !(tliving is Player))
                {
                    tliving.AddBlood(4000);
                }
            }
            Body.PlayMovie("renew", 0, 0);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
        }

        public override void OnDiedSay()
        {
        }

        public override void OnShootedSay(int delay)
        {
        }
    }
}
