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
    public class ThirdTerrorBlowNpc : ABrain
    {

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
            Body.PlayMovie("", 100, 1500);
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
            Body.PlayMovie("beatA", 1500, 1000);
            Game.AddAction(new FocusAction(Body.X, Body.Y, 0, 150, 1000));
            Body.RangeAttacking(Body.X - 200, Body.X + 200, "cry", 2000, null);
            Body.Die(2500);
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
