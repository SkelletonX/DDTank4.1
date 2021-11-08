// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.SimpleBomb
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic.Phy.Actions;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Maths;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
    public class SimpleBomb : BombObject
    {
        private bool digMap;
        protected List<BombAction> m_actions;
        private bool m_bombed;
        protected bool m_controled;
        private BaseGame m_game;
        private BallInfo m_info;
        private float m_lifeTime;
        private Living m_owner;
        protected List<BombAction> m_petActions;
        protected int m_petRadius;
        protected double m_power;
        protected int m_radius;
        protected Tile m_shape;
        protected BombType m_type;
        protected int m_angle;

        public SimpleBomb(
          int id,
          BombType type,
          Living owner,
          BaseGame game,
          BallInfo info,
          Tile shape,
          bool controled,
          int angle)
          : base(id, (float)info.Mass, (float)info.Weight, (float)info.Wind, (float)info.DragIndex)
        {
            this.m_owner = owner;
            this.m_game = game;
            this.m_info = info;
            this.m_shape = shape;
            this.m_type = type;
            this.m_power = info.Power;
            this.m_radius = info.Radii;
            this.m_controled = controled;
            this.m_bombed = false;
            this.m_lifeTime = 0.0f;
            this.m_angle = Math.Abs(angle);
            this.m_petRadius = 80;
            if (this.m_info.IsSpecial())
                this.digMap = false;
            else
                this.digMap = true;
        }

        public void Bomb()
        {
            this.StopMoving();
            this.m_isLiving = false;
            this.m_bombed = true;
        }

        private bool FindAddBloodLivingCount(List<Living> list, Living living)
        {
            int num = 0;
            foreach (Living byaddLiving in list)
            {
                if (this.m_game is PVEGame)
                {
                    if (((PVEGame)this.m_game).CanAddBlood(living, byaddLiving))
                        ++num;
                }
                else
                    ++num;
                if (num >= 2)
                    return true;
            }
            return false;
        }


        private void BombImp()//petSkill
        {
            List<Living> hitByHitPiont1 = this.m_map.FindHitByHitPiont(this.GetCollidePoint(), this.m_radius);
            foreach (Living living in hitByHitPiont1)
            {
                if (living is Player)
                    (living as Player).OnBeforeBomb((int)((double)this.m_lifeTime * 1000.0) + 1000);
                if (living.IsNoHole || living.NoHoleTurn)
                {
                    living.NoHoleTurn = true;
                    this.digMap = false;
                }
                living.SyncAtTime = false;
            }
            this.m_owner.SyncAtTime = false;
            try
            {
                if (this.digMap)
                    this.m_map.Dig(this.m_x, this.m_y, this.m_shape, (Tile)null);
                this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.BOMB, this.m_x, this.m_y, this.digMap ? 1 : 0, 0));
                switch (this.m_type)
                {
                    case BombType.FORZEN:
                        using (List<Living>.Enumerator enumerator = hitByHitPiont1.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Living current = enumerator.Current;
                                if (current.Config.DamageForzen)
                                {
                                    current.Properties2 = (object)(Convert.ToInt32(current.Properties2) - 1);
                                    if (Convert.ToInt32(current.Properties2) <= 0)
                                    {
                                        if (current is SimpleBoss || current is SimpleBoss && current.Config.CanFrost)
                                            ((SimpleBoss)current).DiedEvent();
                                        if (current is SimpleNpc)
                                            ((SimpleNpc)current).OnDie();
                                        if (current is SimpleNpc && current.Config.CanFrost)
                                        {
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.FORZEN, current.Id, 0, 0, 4));
                                            current.IsFrost = true;
                                        }
                                    }
                                }
                                if (!this.m_owner.IsFriendly(current))
                                {
                                    if (this.m_owner is SimpleBoss && new IceFronzeEffect(100).Start(current))
                                    {
                                        this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.FORZEN, current.Id, 0, 0, 0));
                                    }
                                    else
                                    {
                                        if (current is SimpleBoss && current.Distance(new Point(this.X, this.Y)) > (double)this.m_radius)
                                            return;
                                        if (new IceFronzeEffect(2).Start(current))
                                        {
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.FORZEN, current.Id, 0, 0, 0));
                                        }
                                        else
                                        {
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.FORZEN, -1, 0, 0, 0));
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.UNANGLE, current.Id, 0, 0, 0));
                                        }
                                    }
                                }
                                current.SendAfterShootedFrozen((int)(((double)this.m_lifeTime + 1.0) * 1000.0));
                            }
                            break;
                        }
                    case BombType.FLY:
                        if (this.m_y > 10 && (double)this.m_lifeTime > 0.0399999991059303)
                        {
                            if (this.m_map.FindYLineNotEmptyPointDown(this.m_x, this.m_y) != Point.Empty)
                            {
                                PointF pointF = new PointF(-this.vX, -this.vY).Normalize(5f);
                                this.m_x += (int)pointF.X;
                                this.m_y += (int)pointF.Y;
                            }
                            this.m_owner.SetXY(this.m_x, this.m_y);
                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.TRANSLATE, this.m_x, this.m_y, 0, 0));
                            this.m_owner.StartMoving();
                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.START_MOVE, this.m_owner.Id, this.m_owner.X, this.m_owner.Y, this.m_owner.IsLiving ? 1 : 0));
                            break;
                        }
                        break;
                    case BombType.CURE:
                        using (List<Living>.Enumerator enumerator = hitByHitPiont1.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Living current = enumerator.Current;
                                double num = !this.m_map.FindPlayers(this.GetCollidePoint(), this.m_radius) ? 1.0 : 0.4;
                                int para3 = this.m_info.ID == 10009 || this.m_owner.Config.CanHeal ? (int)((double)this.m_lifeTime * 2000.0) : (int)((double)((Player)this.m_owner).PlayerDetail.SecondWeapon.Template.Property7 * Math.Pow(1.1, (double)((Player)this.m_owner).PlayerDetail.SecondWeapon.StrengthenLevel) * num) + this.m_owner.FightBuffers.ConsortionAddBloodGunCount + this.m_owner.PetEffects.BonusPoint + this.m_owner.PetEffects.AddBloodPercent * current.MaxBlood / 100;
                                //num2 = this.m_owner.PetEffects.AddBloodPercent * current.MaxBlood / 100;
                                if (current is Player && this.m_game is PVPGame)
                                {
                                    current.AddBlood(para3);
                                    ((Player)current).TotalCure += para3;
                                    this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.CURE, current.Id, current.Blood, para3, 0));
                                }
                                if (this.m_game is PVEGame && ((PVEGame)this.m_game).CanAddBlood(this.m_owner, current))
                                {
                                    current.AddBlood(para3);
                                    if (current is SimpleBoss)
                                        ((SimpleBoss)current).TotalCure += para3;
                                    if (current is SimpleNpc)
                                        ((SimpleNpc)current).TotalCure += para3;
                                    this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.CURE, current.Id, current.Blood, para3, 0));
                                }
                            }
                            break;
                        }
                    default:
                        foreach (Living living in hitByHitPiont1)
                        {
                            if (!this.m_owner.IsFriendly(living) && (!(this.m_owner is Player) || !living.Config.IsHelper) && (living is Player || living.Config.CanTakeDamage))
                            {
                                if (!this.m_owner.IsFriendly(living))
                                {

                                        int damageAmount = this.MakeDamage(living);
                                        int criticalAmount = 0;
                                        if (damageAmount != 0)
                                        {
                                         
  
                                                criticalAmount = this.MakeCriticalDamage(living, damageAmount);
                                            this.m_owner.OnTakedDamage(living, ref damageAmount, ref criticalAmount);
                                            if (living.TakeDamage(this.m_owner, ref damageAmount, ref criticalAmount, "Fire",0))
                                                this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.KILL_PLAYER, living.Id, damageAmount + criticalAmount, criticalAmount != 0 ? 2 : 1, living.Blood));
                                            else
                                                this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.UNFORZEN, living.Id, 0, 0, 0));
                                            if (this.m_owner is Player && living is SimpleBoss)
                                                this.m_owner.TotalDameLiving += criticalAmount + damageAmount;
                                            if (living is Player)
                                            {
                                                int dander = ((TurnedLiving)living).Dander;
                                                if (this.m_owner.FightBuffers.ConsortionReduceDander > 0)
                                                {
                                                    dander -= dander * this.m_owner.FightBuffers.ConsortionReduceDander / 100;
                                                    ((TurnedLiving)living).Dander = dander;
                                                }
                                                this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.DANDER, living.Id, dander, 0, 0));
                                            }
                                            if ((living is SimpleBoss || living is SimpleNpc) && this.m_game.RoomType != eRoomType.FightFootballTime)
                                            {
                                                ((PVEGame)this.m_game).OnShooted();
                                                if (living.DoAction > -1)
                                                    this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.DO_ACTION, living.Id, 0, 0, living.DoAction));
                                            }
                                        }
                                        else if (living is SimpleBoss || living is SimpleNpc)
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.DO_ACTION, living.Id, 0, 0, 0));
                                        this.m_owner.OnAfterKillingLiving(living, damageAmount, criticalAmount);
                                        if (living.IsLiving)
                                        {
                                            living.StartMoving((int)(((double)this.m_lifeTime + 1.0) * 1000.0), 12);
                                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.START_MOVE, living.Id, living.X, living.Y, living.IsLiving ? 1 : 0));
                                        }
                                    
                                }
                            }
                        }
                        List<Living> hitByHitPiont2 = this.m_map.FindHitByHitPiont(this.GetCollidePoint(), this.m_petRadius);
                        if (this.m_owner.isPet && this.m_owner.PetEffects.ActivePetHit)
                        {
                            foreach (Living target in hitByHitPiont2)
                            {
                                if (target != this.m_owner)
                                {
                                    int num = this.MakePetDamage(target, this.GetCollidePoint());
                                    if (num > 0)
                                    {
                                        int damageAmount = num * this.m_owner.PetEffects.PetBaseAtt / 100;
                                        int criticalAmount = this.MakeCriticalDamage(target, damageAmount);
                                        if (target.PetTakeDamage(this.m_owner, ref damageAmount, ref criticalAmount, "PetFire"))
                                        {
                                            if (target is Player)
                                                this.m_petActions.Add(new BombAction(this.m_lifeTime, ActionType.PET, target.Id, damageAmount + criticalAmount, ((TurnedLiving)target).Dander, target.Blood));
                                            else
                                                this.m_petActions.Add(new BombAction(this.m_lifeTime, ActionType.PET, target.Id, damageAmount + criticalAmount, 0, target.Blood));
                                        }
                                    }
                                }
                            }
                            if (hitByHitPiont2.Count == 0)
                                this.m_petActions.Add(new BombAction(0.0f, ActionType.NULLSHOOT, 0, 0, 0, 0));
                            this.m_owner.PetEffects.ActivePetHit = false;
                            break;
                        }
                        break;
                }
                this.Die();
            }
            finally
            {
                this.m_owner.SyncAtTime = true;
                foreach (Living living in hitByHitPiont1)
                    living.SyncAtTime = true;
            }
        }

        protected override void CollideGround()
        {
            base.CollideGround();
            this.Bomb();
        }

        protected override void CollideObjects(Physics[] list)
        {
            for (int index = 0; index < list.Length; ++index)
            {
                Physics physics = list[index];
                physics.CollidedByObject((Physics)this);
                this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.PICK, physics.Id, 0, 0, 0));
            }
        }

        protected override void FlyoutMap()
        {
            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.FLY_OUT, 0, 0, 0, 0));
            base.FlyoutMap();
        }

        protected int MakePetDamage(Living target, Point p)
        {
            if (!(target is Player) && (target.Config.HaveShield || !target.Config.CanTakeDamage || target.Config.IsHelper))
                return 0;
            double baseDamage = this.m_owner.BaseDamage;
            double num1 = target.BaseGuard;
            double num2 = target.Defence;
            double attack = this.m_owner.Attack;
            if (target.AddArmor && (target as Player).DeputyWeapon != null)
            {
                int hertAddition = (int)target.getHertAddition((target as Player).DeputyWeapon);
                num1 += (double)hertAddition;
                num2 += (double)hertAddition;
            }
            if (this.m_owner.IgnoreArmor)
            {
                num1 = 0.0;
                num2 = 0.0;
            }
            float currentDamagePlus = this.m_owner.CurrentDamagePlus;
            double num3 = 0.95 * (num1 - (double)(3 * this.m_owner.Grade)) / (500.0 + num1 - (double)(3 * this.m_owner.Grade));
            double num4 = num2 - this.m_owner.Lucky >= 0.0 ? 0.95 * (num2 - this.m_owner.Lucky) / (600.0 + num2 - this.m_owner.Lucky) : 0.0;
            double num5 = 1.0 + attack * 0.001;
            double num6 = baseDamage * num5 * (1.0 - (num3 + num4 - num3 * num4)) * (double)currentDamagePlus;
            if (num6 < 0.0)
                return 1;
            return (int)num6;
        }

        protected int MakeCriticalDamage(Living target, int baseDamage)
        {
            double lucky = this.m_owner.Lucky;
            bool flag = lucky * 45.0 / (800.0 + lucky) + (double)this.m_owner.PetEffects.CritRate > (double)this.m_game.Random.Next(100);
            if (this.m_owner.PetEffects.CritActive)
            {
                flag = true;
                this.m_owner.PetEffects.CritActive = true;
            }
            if (!flag)
                return 0;
            int num1 = 0;
            int num2 = (int)(0.5 + lucky * 0.00015 + (double)baseDamage) * (100 - num1) / 100;
            if (this.m_owner.FightBuffers.ConsortionAddCritical > 0)
                num2 += this.m_owner.FightBuffers.ConsortionAddCritical;
            return num2;
        }

        protected int MakeDamage(Living target)
        {
            if ((target.Config.IsHelper || !target.Config.CanTakeDamage || target.Config.HaveShield) && (target is SimpleBoss || target is SimpleNpc))
                return 0;
            double baseDamage = this.m_owner.BaseDamage;
            double num1 = target.BaseGuard;
            double num2 = target.Defence;
            double attack = this.m_owner.Attack;
            if (target.AddArmor && (target as Player).DeputyWeapon != null)
            {
                int hertAddition = (int)target.getHertAddition((target as Player).DeputyWeapon);
                num1 += (double)hertAddition;
                num2 += (double)hertAddition;
            }
            if (this.m_owner.IgnoreArmor)
            {
                num1 = 0.0;
                num2 = 0.0;
            }
            float currentDamagePlus = this.m_owner.CurrentDamagePlus;
            float currentShootMinus = this.m_owner.CurrentShootMinus;
            double num3 = 0.95 * (num1 - (double)(3 * this.m_owner.Grade)) / (500.0 + num1 - (double)(3 * this.m_owner.Grade));
            double num4 = num2 - this.m_owner.Lucky >= 0.0 ? 0.95 * (num2 - this.m_owner.Lucky) / (600.0 + num2 - this.m_owner.Lucky) : 0.0;
            double num5 = ((double)this.m_owner.FightBuffers.WorldBossAddDamage * (1.0 - (num1 / 200.0 + num2 * 0.003)) + baseDamage * (1.0 + attack * 0.001) * (1.0 - (num3 + num4 - num3 * num4))) * (double)currentDamagePlus * (double)currentShootMinus;
            Point p = new Point(this.X, this.Y);
            double num6 = target.Distance(p);
            if (num6 >= (double)this.m_radius)
                return 0;
            double num7 = num5 * (1.0 - num6 / (double)this.m_radius / 4.0);
            if (this.m_owner is Player && target is Player && (target != this.m_owner && num7 > 0.0))
            {
                if (this.m_owner.Direction == 1)
                {
                    if (this.m_angle > 70 && this.m_angle < 90)
                        this.m_game.AddAction((IAction)new FightAchievementAction(this.m_owner, eFightAchievementType.AcrobatMaster, this.m_owner.Direction, 1200));
                    if (this.m_angle > 110 && this.m_angle < 130)
                        this.m_game.AddAction((IAction)new FightAchievementAction(this.m_owner, eFightAchievementType.EmperorOfPlayingBack, this.m_owner.Direction, 1200));
                }
                else
                {
                    if (this.m_angle > 70 && this.m_angle < 90)
                        this.m_game.AddAction((IAction)new FightAchievementAction(this.m_owner, eFightAchievementType.AcrobatMaster, this.m_owner.Direction, 1200));
                    if (this.m_angle > 110 && this.m_angle < 130)
                        this.m_game.AddAction((IAction)new FightAchievementAction(this.m_owner, eFightAchievementType.EmperorOfPlayingBack, this.m_owner.Direction, 1200));
                }
                ++this.m_owner.countBoom;
            }
            if (num7 < 0.0)
                return 1;
            return (int)num7;
        }

        public override void StartMoving()
        {
            base.StartMoving();
            this.m_actions = new List<BombAction>();
            this.m_petActions = new List<BombAction>();
            int lifeTime = this.m_game.LifeTime;
            while (this.m_isMoving && this.m_isLiving)
            {
                this.m_lifeTime += 0.04f;
                Point point1 = this.CompleteNextMovePoint(0.04f);
                this.MoveTo(point1.X, point1.Y);
                if (this.m_isLiving)
                {
                    if (Math.Round((double)this.m_lifeTime * 100.0) % 40.0 == 0.0 && point1.Y > 0)
                        this.m_game.AddTempPoint(point1.X, point1.Y);
                    if (this.m_controled && (double)this.vY > 0.0)
                    {
                        Living nearestEnemy = this.m_map.FindNearestEnemy(this.m_x, this.m_y, 150.0, this.m_owner);
                        if (nearestEnemy != null)
                        {
                            Point point2;
                            if (nearestEnemy is SimpleBoss)
                            {
                                Rectangle directDemageRect = nearestEnemy.GetDirectDemageRect();
                                point2 = new Point(directDemageRect.X - this.m_x + 20, directDemageRect.Y + directDemageRect.Height - this.m_y);
                            }
                            else
                                point2 = new Point(nearestEnemy.X - this.m_x, nearestEnemy.Y - this.m_y);
                            point2 = point2.Normalize(1000);
                            this.setSpeedXY(point2.X, point2.Y);
                            this.UpdateForceFactor(0.0f, 0.0f, 0.0f);
                            this.m_controled = false;
                            this.m_actions.Add(new BombAction(this.m_lifeTime, ActionType.CHANGE_SPEED, point2.X, point2.Y, 0, 0));
                        }
                    }
                }
                if (this.m_bombed)
                {
                    this.m_bombed = false;
                    this.BombImp();
                }
            }
        }

        public List<BombAction> Actions
        {
            get
            {
                return this.m_actions;
            }
        }

        public BallInfo BallInfo
        {
            get
            {
                return this.m_info;
            }
        }

        public bool DigMap
        {
            get
            {
                return this.digMap;
            }
        }

        public float LifeTime
        {
            get
            {
                return this.m_lifeTime;
            }
        }

        public Living Owner
        {
            get
            {
                return this.m_owner;
            }
        }

        public List<BombAction> PetActions
        {
            get
            {
                return this.m_petActions;
            }
        }
    }
}
