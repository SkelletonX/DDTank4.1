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
    public class NullAiFly : ABrain
    {
        public override void OnBeginSelfTurn()
        {
        }

        public override void OnBeginNewTurn()
        {
        }

        public override void OnCreated()
        {
        }

        public override void OnStartAttacking()
        {
        }

        public override void OnStopAttacking()
        {
        }

        public override void OnKillPlayerSay()
        {
        }

        public override void OnDiedSay()
        {
        }
    }
}