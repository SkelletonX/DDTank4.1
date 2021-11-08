// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.Living
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic.PetEffects;
using Game.Logic.Phy.Actions;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Maths;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game.Logic.Phy.Object
{
    public class Living : Physics
    {
        protected static int GHOST_MOVE_SPEED = 8;
        public bool AddArmor;
        public double Agility;
        public double Attack;
        public double BaseDamage;
        public double BaseGuard;
        private bool m_blockTurn;
        public bool ControlBall;
        public int countBoom;
        public float CurrentDamagePlus;
        public bool CurrentIsHitTarget;
        public float CurrentShootMinus;
        public double Defence;
        public int EffectsCount;
        public bool EffectTrigger;
        public int Experience;
        public int FlyingPartical;
        public int Grade;
        public bool IgnoreArmor;
        public int LastLifeTimeShoot;
        public double Lucky;
        private string m_action;
        private bool m_autoBoot;
        protected int m_blood;
        private LivingConfig m_config;
        private Rectangle m_demageRect;
        public int m_direction;
        private int m_doAction;
        private EffectList m_effectList;
        private int m_FallCount;
        private FightBufferInfo m_fightBufferInfo;
        private int m_FindCount;
        protected BaseGame m_game;
        private bool m_isAttacking;
        private bool m_isFrost;
        private bool m_isHide;
        private bool m_isNoHole;
        private bool m_isSeal;
        protected int m_maxBlood;
        private string m_modelId;
        private string m_name;
        private int m_degree;
        private PetEffectInfo m_petEffects;
        private int m_pictureTurn;
        private int m_specialSkillDelay;
        private int m_state;
        protected bool m_syncAtTime;
        private int m_team;
        private eLivingType m_type;
        private bool m_vaneOpen;
        public int mau;
        public int MaxBeatDis;
        public bool NoHoleTurn;
        public bool PetEffectTrigger;
        private Random rand;
        public List<int> ScoreArr;
        public int ShootMovieDelay;
        public int TotalDameLiving;
        public int TotalHitTargetCount;
        public int TotalHurt;
        public int TotalKill;
        public int TotalShootCount;
        public int TurnNum;
        public int TotalCure;
        private PetEffectList petEffectList_0;
        private bool bool_1;

        public event KillLivingEventHanlde AfterKilledByLiving;

        public event KillLivingEventHanlde AfterKillingLiving;

        public event LivingTakedDamageEventHandle BeforeTakeDamage;

        public event LivingEventHandle BeginAttacked;

        public event LivingEventHandle BeginAttacking;

        public event LivingEventHandle BeginNextTurn;

        public event LivingEventHandle BeginSelfTurn;

        public event LivingEventHandle Died;

        public event LivingEventHandle EndAttacking;

        public event LivingTakedDamageEventHandle TakePlayerDamage;

        public Living(
          int id,
          BaseGame game,
          int team,
          string name,
          string modelId,
          int maxBlood,
          int immunity,
          int direction)
          : base(id)
        {
            this.BaseDamage = 10.0;
            this.BaseGuard = 10.0;
            this.Defence = 10.0;
            this.Attack = 10.0;
            this.Agility = 10.0;
            this.Lucky = 10.0;
            this.Grade = 1;
            this.Experience = 10;
            this.m_vaneOpen = false;
            this.m_action = "";
            this.m_game = game;
            this.m_team = team;
            this.m_name = name;
            this.m_modelId = modelId;
            this.m_maxBlood = maxBlood;
            this.m_direction = direction;
            this.m_state = 0;
            this.m_doAction = -1;
            this.MaxBeatDis = 100;
            this.AddArmor = false;
            this.m_effectList = new EffectList(this, immunity);
            this.petEffectList_0 = new PetEffectList(this, immunity);
            this.m_petEffects = new PetEffectInfo();
            this.m_fightBufferInfo = new FightBufferInfo();
            this.SetupPetEffect();
            this.m_config = new LivingConfig();
            this.m_syncAtTime = true;
            this.m_type = eLivingType.Living;
            this.rand = new Random();
            this.ScoreArr = new List<int>();
            this.m_autoBoot = false;
            this.m_pictureTurn = 0;
            this.TotalCure = 0;
            this.bool_1 = false;
        }

        public virtual int AddBlood(int value)
        {
            return this.AddBlood(value, 0);
        }

        public virtual int AddBlood(int value, int type)
        {
            this.m_blood += value;
            if (this.m_blood > this.m_maxBlood)
                this.m_blood = this.m_maxBlood;
            if (this.m_syncAtTime)
                this.m_game.SendGameUpdateHealth(this, type, value);
            if (this.m_blood <= 0)
                this.Die();
            return value;
        }

        public void AddEffect(AbstractEffect effect, int delay)
        {
            this.m_game.AddAction((IAction)new LivingDelayEffectAction(this, effect, delay));
        }

        public void AddPetEffect(AbstractPetEffect effect, int delay)
        {
            this.m_game.AddAction((IAction)new LivingDelayPetEffectAction(this, effect, delay));
        }

        public void SendAfterShootedFrozen(int delay)
        {
            this.m_game.AddAction((IAction)new LivingAfterShootedFrozen(this, delay));
        }

        public void SendAfterShootedAction(int delay)
        {
            this.m_game.AddAction((IAction)new LivingAfterShootedAction(this, delay));
        }

        public void AddRemoveEnergy(int value)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "energy", value.ToString());
        }

        public bool isPet
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool Beat(
          Living target,
          string action,
          int demageAmount,
          int criticalAmount,
          int delay)
        {
            return this.Beat(target, action, demageAmount, criticalAmount, delay, 1, 1);
        }

        public bool Beat(
          Living target,
          string action,
          int demageAmount,
          int criticalAmount,
          int delay,
          int livingCount,
          int attackEffect)
        {
            if (target != null && target.IsLiving)
            {
                demageAmount = this.MakeDamage(target);
                this.OnBeforeTakedDamage(target, ref demageAmount, ref criticalAmount);
                this.StartAttacked();
                if ((int)target.Distance(this.X, this.Y) <= this.MaxBeatDis)
                {
                    this.Direction = this.X - target.X <= 0 ? 1 : -1;
                    this.m_game.AddAction((IAction)new LivingBeatAction(this, target, demageAmount, criticalAmount, action, delay, livingCount, attackEffect));
                    return true;
                }
            }
            return false;
        }

        public void BeatDirect(
          Living target,
          string action,
          int delay,
          int livingCount,
          int attackEffect)
        {
            this.m_game.AddAction((IAction)new LivingBeatDirectAction(this, target, action, delay, livingCount, attackEffect));
        }

        public void BoltMove(int x, int y, int delay)
        {
            this.m_game.AddAction((IAction)new LivingBoltMoveAction(this, x, y, "", delay, 0));
        }

        public double BoundDistance(Point p)
        {
            List<double> source = new List<double>();
            foreach (Rectangle rectangle in this.GetDirectBoudRect())
            {
                for (int x = rectangle.X; x <= rectangle.X + rectangle.Width; x += 10)
                {
                    source.Add(Math.Sqrt((double)((x - p.X) * (x - p.X) + (rectangle.Y - p.Y) * (rectangle.Y - p.Y))));
                    source.Add(Math.Sqrt((double)((x - p.X) * (x - p.X) + (rectangle.Y + rectangle.Height - p.Y) * (rectangle.Y + rectangle.Height - p.Y))));
                }
                for (int y = rectangle.Y; y <= rectangle.Y + rectangle.Height; y += 10)
                {
                    source.Add(Math.Sqrt((double)((rectangle.X - p.X) * (rectangle.X - p.X) + (y - p.Y) * (y - p.Y))));
                    source.Add(Math.Sqrt((double)((rectangle.X + rectangle.Width - p.X) * (rectangle.X + rectangle.Width - p.X) + (y - p.Y) * (y - p.Y))));
                }
            }
            return source.Min();
        }

        public void CallFuction(LivingCallBack func, int delay)
        {
            if (this.m_game == null)
                return;
            this.m_game.AddAction((IAction)new LivingCallFunctionAction(this, func, delay));
        }

        public override void CollidedByObject(Physics phy)
        {
            if (!(phy is SimpleBomb) || !this.Config.CanCollied)
                return;
            ((SimpleBomb)phy).Bomb();
        }

        public static double ComputDX(double v, float m, float af, float f, float dt)
        {
            return v * (double)dt + ((double)f - (double)af * v) / (double)m * (double)dt * (double)dt;
        }

        public static double ComputeVx(double dx, float m, float af, float f, float t)
        {
            return (dx - (double)f / (double)m * (double)t * (double)t / 2.0) / (double)t + (double)af / (double)m * dx * 0.7;
        }

        public static double ComputeVy(double dx, float m, float af, float f, float t)
        {
            return (dx - (double)f / (double)m * (double)t * (double)t / 2.0) / (double)t + (double)af / (double)m * dx * 1.3;
        }

        public void ChangeDirection(Living obj, int delay)
        {
            int direction = this.FindDirection(obj);
            if (delay > 0)
                this.m_game.AddAction((IAction)new LivingChangeDirectionAction(this, direction, delay));
            else
                this.Direction = direction;
        }

        public void ChangeDirection(int direction, int delay)
        {
            if (delay > 0)
                this.m_game.AddAction((IAction)new LivingChangeDirectionAction(this, direction, delay));
            else
                this.Direction = direction;
        }

        public override void Die()
        {
            if (this is Player && this.m_game is PVEGame)
                (this.m_game as PVEGame).SetupStyle(0);
            if (this.m_blood > 0)
            {
                this.m_blood = 0;
                this.m_doAction = -1;
                if (this.m_syncAtTime)
                    this.m_game.SendGameUpdateHealth(this, 6, 0);
            }
            if (!this.IsLiving)
                return;
            if (this.IsAttacking)
                this.StopAttacking();
            base.Die();
            this.OnDied();
            this.m_game.CheckState(0);
        }

        public virtual void Die(int delay)
        {
            if (!this.IsLiving || this.m_game == null)
                return;
            this.m_game.AddAction((IAction)new LivingDieAction(this, delay));
        }

        public double Distance(Point p)
        {
            List<double> source = new List<double>();
            Rectangle directDemageRect = this.GetDirectDemageRect();
            for (int x = directDemageRect.X; x <= directDemageRect.X + directDemageRect.Width; x += 10)
            {
                source.Add(Math.Sqrt((double)((x - p.X) * (x - p.X) + (directDemageRect.Y - p.Y) * (directDemageRect.Y - p.Y))));
                source.Add(Math.Sqrt((double)((x - p.X) * (x - p.X) + (directDemageRect.Y + directDemageRect.Height - p.Y) * (directDemageRect.Y + directDemageRect.Height - p.Y))));
            }
            for (int y = directDemageRect.Y; y <= directDemageRect.Y + directDemageRect.Height; y += 10)
            {
                source.Add(Math.Sqrt((double)((directDemageRect.X - p.X) * (directDemageRect.X - p.X) + (y - p.Y) * (y - p.Y))));
                source.Add(Math.Sqrt((double)((directDemageRect.X + directDemageRect.Width - p.X) * (directDemageRect.X + directDemageRect.Width - p.X) + (y - p.Y) * (y - p.Y))));
            }
            return source.Min();
        }

        public bool FallFrom(int x, int y, string action, int delay, int type, int speed)
        {
            return this.FallFrom(x, y, action, delay, type, speed, (LivingCallBack)null);
        }

        public bool FallFrom(
          int x,
          int y,
          string action,
          int delay,
          int type,
          int speed,
          LivingCallBack callback)
        {
            Point point = this.m_map.FindYLineNotEmptyPointDown(x, y);
            if (point == Point.Empty)
                point = new Point(x, this.m_game.Map.Bound.Height + 1);
            if (this.Y >= point.Y)
                return false;
            this.m_game.AddAction((IAction)new LivingFallingAction(this, point.X, point.Y, speed, action, delay, type, callback));
            return true;
        }

        public bool FallFromTo(
          int x,
          int y,
          string action,
          int delay,
          int type,
          int speed,
          LivingCallBack callback)
        {
            this.m_game.AddAction((IAction)new LivingFallingAction(this, x, y, speed, action, delay, type, callback));
            return true;
        }

        public int FindDirection(Living obj)
        {
            return obj.X > this.X ? 1 : -1;
        }

        public bool FlyTo(int X, int Y, int x, int y, string action, int delay, int speed)
        {
            return this.FlyTo(X, Y, x, y, action, delay, speed, (LivingCallBack)null);
        }

        public bool FlyTo(
          int X,
          int Y,
          int x,
          int y,
          string action,
          int delay,
          int speed,
          LivingCallBack callback)
        {
            this.m_game.AddAction((IAction)new LivingFlyToAction(this, X, Y, x, y, action, delay, speed, callback));
            this.m_game.AddAction((IAction)new LivingFallingAction(this, x, y, 0, action, delay, 0, callback));
            return true;
        }

        public Rectangle GetDirectDemageRect()
        {
            return new Rectangle(this.X + this.m_demageRect.X, this.Y + this.m_demageRect.Y, this.m_demageRect.Width, this.m_demageRect.Height);
        }

        public List<Rectangle> GetDirectBoudRect()
        {
            List<Rectangle> rectangleList = new List<Rectangle>();
            int x = this.X + this.Bound.X;
            int y = this.Y + this.Bound.Y;
            int width = this.Bound.Width;
            int height = this.Bound.Height;
            rectangleList.Add(new Rectangle(x, y, width, height));
            return rectangleList;
        }

        public double getHertAddition(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null)
                return 0.0;
            double property7 = (double)item.Template.Property7;
            double strengthenLevel = (double)item.StrengthenLevel;
            return Math.Round(property7 * Math.Pow(1.1, strengthenLevel) - property7) + property7;
        }

        public bool GetSealState()
        {
            return this.m_isSeal;
        }

        public void GetShootForceAndAngle(
          ref int x,
          ref int y,
          int bombId,
          int minTime,
          int maxTime,
          int bombCount,
          float time,
          ref int force,
          ref int angle)
        {
            if (minTime >= maxTime)
                return;
            BallInfo ball = BallMgr.FindBall(bombId);
            if (this.m_game == null || ball == null)
                return;
            Map map = this.m_game.Map;
            Point shootPoint = this.GetShootPoint();
            float num1 = (float)(x - shootPoint.X);
            float num2 = (float)(y - shootPoint.Y);
            float af = map.airResistance * (float)ball.DragIndex;
            float f1 = map.gravity * (float)ball.Weight * (float)ball.Mass;
            float f2 = map.wind * (float)ball.Wind;
            float mass = (float)ball.Mass;
            for (float t = time; (double)t <= 4.0; t += 0.6f)
            {
                double vx = Living.ComputeVx((double)num1, mass, af, f2, t);
                double vy = Living.ComputeVy((double)num2, mass, af, f1, t);
                if (vy < 0.0 && vx * (double)this.m_direction > 0.0)
                {
                    double num3 = Math.Sqrt(vx * vx + vy * vy);
                    if (num3 < 2000.0)
                    {
                        force = (int)num3;
                        angle = (int)(Math.Atan(vy / vx) / Math.PI * 180.0);
                        if (vx < 0.0)
                        {
                            angle += 180;
                            break;
                        }
                        break;
                    }
                }
            }
            x = shootPoint.X;
            y = shootPoint.Y;
        }

        public Point GetShootPoint()
        {
            return !(this is SimpleBoss) ? (this.m_direction > 0 ? new Point(this.X - this.m_rect.X + 5, this.Y + this.m_rect.Y - 5) : new Point(this.X + this.m_rect.X - 5, this.Y + this.m_rect.Y - 5)) : (this.m_direction > 0 ? new Point(this.X - ((SimpleBoss)this).NpcInfo.FireX, this.Y + ((SimpleBoss)this).NpcInfo.FireY) : new Point(this.X + ((SimpleBoss)this).NpcInfo.FireX, this.Y + ((SimpleBoss)this).NpcInfo.FireY));
        }

        public bool IconPicture(eMirariType type, bool result)
        {
            this.m_game.SendPlayerPicture(this, (int)type, result);
            return true;
        }

        public bool IsFriendly(Living living)
        {
            if ((living == null || !living.Config.IsHelper) && !(living is Player))
                return living.Team == this.Team;
            return false;
        }

        public bool JumpTo(int x, int y, string action, int delay, int type)
        {
            return this.JumpTo(x, y, action, delay, type, 20, (LivingCallBack)null);
        }

        public bool JumpTo(int x, int y, string ation, int delay, int type, LivingCallBack callback)
        {
            return this.JumpTo(x, y, ation, delay, type, 20, callback);
        }

        public bool JumpTo(
          int x,
          int y,
          string action,
          int delay,
          int type,
          int speed,
          LivingCallBack callback)
        {
            Point notEmptyPointDown = this.m_map.FindYLineNotEmptyPointDown(x, y);
            if (notEmptyPointDown.Y >= this.Y)
                return false;
            this.m_game.AddAction((IAction)new LivingJumpAction(this, notEmptyPointDown.X, notEmptyPointDown.Y, speed, action, delay, type, callback));
            return true;
        }

        public bool JumpTo(
          int x,
          int y,
          string action,
          int delay,
          int type,
          int speed,
          LivingCallBack callback,
          int value)
        {
            Point notEmptyPointDown = this.m_map.FindYLineNotEmptyPointDown(x, y);
            if (notEmptyPointDown.Y >= this.Y && value != 1)
                return false;
            this.m_game.AddAction((IAction)new LivingJumpAction(this, notEmptyPointDown.X, notEmptyPointDown.Y, speed, action, delay, type, callback));
            return true;
        }

        public bool JumpToSpeed(
          int x,
          int y,
          string action,
          int delay,
          int type,
          int speed,
          LivingCallBack callback)
        {
            Point notEmptyPointDown = this.m_map.FindYLineNotEmptyPointDown(x, y);
            int y1 = notEmptyPointDown.Y;
            this.m_game.AddAction((IAction)new LivingJumpAction(this, notEmptyPointDown.X, notEmptyPointDown.Y, speed, action, delay, type, callback));
            return true;
        }

        protected int MakeDamage(Living target)
        {
            double baseDamage = this.BaseDamage;
            double num1 = target.BaseGuard;
            double num2 = target.Defence;
            double attack = this.Attack;
            if (target.AddArmor && (target as Player).DeputyWeapon != null)
            {
                int hertAddition = (int)this.getHertAddition((target as Player).DeputyWeapon);
                num1 += (double)hertAddition;
                num2 += (double)hertAddition;
            }
            if (this.IgnoreArmor)
            {
                num1 = 0.0;
                num2 = 0.0;
            }
            float currentDamagePlus = this.CurrentDamagePlus;
            float currentShootMinus = this.CurrentShootMinus;
            double num3 = 0.95 * (num1 - (double)(3 * this.Grade)) / (500.0 + num1 - (double)(3 * this.Grade));
            double num4 = num2 - this.Lucky >= 0.0 ? 0.95 * (num2 - this.Lucky) / (600.0 + num2 - this.Lucky) : 0.0;
            double num5 = 1.0 + attack * 0.001;
            double num6 = baseDamage * num5 * (1.0 - (num3 + num4 - num3 * num4)) * (double)currentDamagePlus * (double)currentShootMinus;
            Point point = new Point(this.X, this.Y);
            if (num6 < 0.0)
                return 1;
            return (int)num6;
        }

        public int MakeDamage(Living target, bool them = false)
        {
            double num1 = target.BaseGuard;
            double num2 = target.Defence;
            double attack = this.Attack;
            if (target.AddArmor && (target as Player).DeputyWeapon != null)
            {
                int hertAddition = (int)this.getHertAddition((target as Player).DeputyWeapon);
                num1 += (double)hertAddition;
                num2 += (double)hertAddition;
            }
            if (this.IgnoreArmor)
            {
                num1 = 0.0;
                num2 = 0.0;
            }
            float currentDamagePlus = this.CurrentDamagePlus;
            float currentShootMinus = this.CurrentShootMinus;
            double num3 = 0.95 * (num1 - (double)(3 * this.Grade)) / (500.0 + num1 - (double)(3 * this.Grade));
            double num4 = num2 - this.Lucky >= 0.0 ? 0.95 * (num2 - this.Lucky) / (600.0 + num2 - this.Lucky) : 0.0;
            double num5 = this.BaseDamage * (1.0 + attack * 0.001) * (1.0 - (num3 + num4 - num3 * num4)) * (double)currentDamagePlus * (double)currentShootMinus;
            Point point = new Point(this.X, this.Y);
            if (num5 < 0.0)
                return 1;
            return (int)num5;
        }

        public bool MoveTo(int x, int y, string action, int delay)
        {
            return this.MoveTo(x, y, action, delay, (LivingCallBack)null);
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          int delay,
          int speed,
          LivingCallBack callback)
        {
            return this.MoveTo(x, y, action, delay, "", speed, callback, 0);
        }

        public bool MoveTo(int x, int y, string action, int delay, LivingCallBack callback)
        {
            if ((this.m_x != x || this.m_y != y) && x >= 0 && x <= this.m_map.Bound.Width)
            {
                List<Point> path = new List<Point>();
                int x1 = this.m_x;
                int y1 = this.m_y;
                int direction = x > x1 ? 1 : -1;
                while ((x - x1) * direction > 0)
                {
                    Point nextWalkPoint = this.m_map.FindNextWalkPoint(x1, y1, direction, this.STEP_X, this.STEP_Y);
                    if (nextWalkPoint != Point.Empty)
                    {
                        path.Add(nextWalkPoint);
                        x1 = nextWalkPoint.X;
                        y1 = nextWalkPoint.Y;
                    }
                    else
                        break;
                }
                if (path.Count > 0)
                {
                    this.m_game.AddAction((IAction)new LivingMoveToAction(this, path, action, delay, 4, callback));
                    return true;
                }
            }
            return false;
        }

        public bool MoveTo(int x, int y, string action, int delay, int speed)
        {
            return this.MoveTo(x, y, action, "", speed, delay, (LivingCallBack)null, 0);
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          int delay,
          LivingCallBack callback,
          int speed)
        {
            return this.MoveTo(x, y, action, "", speed, delay, callback, 0);
        }

        public bool MoveTo(int x, int y, string action, int delay, string sAction, int speed)
        {
            return this.MoveTo(x, y, action, delay, sAction, speed, (LivingCallBack)null);
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          int delay,
          string sAction,
          int speed,
          LivingCallBack callback)
        {
            return this.MoveTo(x, y, action, delay, sAction, speed, callback, 0);
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          string sAction,
          int speed,
          int delay,
          LivingCallBack callback)
        {
            return this.MoveTo(x, y, action, sAction, speed, delay, callback, 0);
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          int delay,
          string sAction,
          int speed,
          LivingCallBack callback,
          int delayCallback)
        {
            if ((this.m_x != x || this.m_y != y) && x >= 0 && x <= this.m_map.Bound.Width)
            {
                List<Point> path = new List<Point>();
                int x1 = this.m_x;
                int y1 = this.m_y;
                int direction = x > x1 ? 1 : -1;
                if (!(action == "fly"))
                {
                    while ((x - x1) * direction > 0)
                    {
                        Point nextWalkPoint = this.m_map.FindNextWalkPoint(x1, y1, direction, speed * this.STEP_X, speed * this.STEP_Y);
                        if (nextWalkPoint != Point.Empty)
                        {
                            path.Add(nextWalkPoint);
                            x1 = nextWalkPoint.X;
                            y1 = nextWalkPoint.Y;
                        }
                        else
                            break;
                    }
                }
                else
                {
                    Point point1 = new Point(x, y);
                    Point point2 = new Point(x1, y1);
                    Point point3 = new Point(x - point2.X, y - point2.Y);
                    while (point3.Length() > (double)speed)
                    {
                        point3.Normalize(speed);
                        point2 = new Point(point2.X + point3.X, point2.Y + point3.Y);
                        point3 = new Point(x - point2.X, y - point2.Y);
                        if (!(point2 != Point.Empty))
                        {
                            path.Add(point1);
                            break;
                        }
                        path.Add(point2);
                    }
                }
                if (path.Count > 0)
                {
                    this.m_game.AddAction((IAction)new LivingMoveToAction(this, path, action, delay, speed, sAction, callback, delayCallback));
                    return true;
                }
            }
            return false;
        }

        public bool MoveTo(
          int x,
          int y,
          string action,
          string sAction,
          int speed,
          int delay,
          LivingCallBack callback,
          int delayCallback)
        {
            if ((this.m_x != x || this.m_y != y) && x >= 0 && x <= this.m_map.Bound.Width)
            {
                List<Point> path = new List<Point>();
                int x1 = this.m_x;
                int y1 = this.m_y;
                int direction = x > x1 ? 1 : -1;
                Point point1 = new Point(x1, y1);
                int x2 = this.m_x;
                int y2 = this.m_y;
                if (!this.Config.IsFly)
                {
                    while ((x - x1) * direction > 0)
                    {
                        Point nextWalkPointDown = this.m_map.FindNextWalkPointDown(x1, y1, direction, speed * this.STEP_X, speed * this.STEP_Y);
                        if (nextWalkPointDown != Point.Empty)
                        {
                            path.Add(nextWalkPointDown);
                            x1 = nextWalkPointDown.X;
                            y1 = nextWalkPointDown.Y;
                        }
                        else
                            break;
                    }
                }
                else
                {
                    Point point2 = new Point(x - point1.X, y - point1.Y);
                    while (point2.Length() > (double)speed)
                    {
                        point2 = point2.Normalize(speed);
                        point1 = new Point(point1.X + point2.X, point1.Y + point2.Y);
                        point2 = new Point(x - point1.X, y - point1.Y);
                        if (!(point1 != Point.Empty))
                        {
                            path.Add(new Point(x, y));
                            break;
                        }
                        path.Add(point1);
                    }
                }
                if (path.Count > 0)
                {
                    this.m_game.AddAction((IAction)new LivingMoveToAction2(this, path, action, sAction, speed, delay, callback, delayCallback));
                    return true;
                }
            }
            return false;
        }

        public void NoFly(bool value)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "nofly", value.ToString());
        }

        public virtual void OnAfterKillingLiving(Living target, int damageAmount, int criticalAmount)
        {
            if (target.Team != this.Team)
            {
                this.CurrentIsHitTarget = true;
                this.TotalHurt += damageAmount + criticalAmount;
                if (!target.IsLiving)
                    ++this.TotalKill;
                this.m_game.CurrentTurnTotalDamage = damageAmount + criticalAmount;
                this.m_game.TotalHurt += damageAmount + criticalAmount;
            }
            if (this.AfterKillingLiving == null)
                return;
            this.AfterKillingLiving(this, target, damageAmount, criticalAmount);
        }

        public virtual void OnAfterTakedBomb()
        {
            if (this is SimpleBoss)
                this.OnAfterTakedBomb();
            if (!(this is SimpleNpc))
                return;
            this.OnAfterTakedBomb();
        }

        public void OnAfterTakedDamage(Living target, int damageAmount, int criticalAmount)
        {
            if (this.AfterKilledByLiving == null)
                return;
            this.AfterKilledByLiving(this, target, damageAmount, criticalAmount);
        }

        public virtual void OnAfterTakedFrozen()
        {
            if (!(this is SimpleNpc))
                return;
            this.OnAfterTakedFrozen();
        }

        public virtual void BeforeTakedDamage(
          Living source,
          ref int damageAmount,
          ref int criticalAmount)
        {
            this.OnBeforeTakedDamage(source, ref damageAmount, ref criticalAmount);
        }

        protected void OnBeforeTakedDamage(Living source, ref int damageAmount, ref int criticalAmount)
        {
            if (this.BeforeTakeDamage == null)
                return;
            this.BeforeTakeDamage(this, source, ref damageAmount, ref criticalAmount);
        }

        protected void OnBeginNewTurn()
        {
            if (this.BeginNextTurn == null)
                return;
            this.BeginNextTurn(this);
        }

        protected void OnBeginSelfTurn()
        {
            if (this.BeginSelfTurn == null)
                return;
            this.BeginSelfTurn(this);
        }

        protected void OnDied()
        {
            if (this.Died != null)
                this.Died(this);
            if (!(this is Player) || !(this.Game is PVEGame))
                return;
            ((PVEGame)this.Game).DoOther();
        }

        public void OnSmallMap(bool state)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "onSmallMap", state.ToString());
        }

        protected void OnStartAttacked()
        {
            if (this.BeginAttacked == null)
                return;
            this.BeginAttacked(this);
        }

        protected void OnStartAttacking()
        {
            if (this.BeginAttacking == null)
                return;
            this.BeginAttacking(this);
        }

        protected void OnStopAttacking()
        {
            if (this.EndAttacking == null)
                return;
            this.EndAttacking(this);
        }

        public void OnTakedDamage(Living source, ref int damageAmount, ref int criticalAmount)
        {
            if (this.TakePlayerDamage == null)
                return;
            this.TakePlayerDamage(this, source, ref damageAmount, ref criticalAmount);
        }

        public virtual void PickBall(Ball ball)
        {
            ball.Die();
            string currentAction = ball.CurrentAction;
            ball.PlayMovie(ball.ActionMapping[currentAction], 1000, 0);
        }

        public virtual void PickPhy(PhysicalObj phy)
        {
            if (!this.m_syncAtTime)
                return;
            phy.Die();
            string name = phy.Name;
            if (name != null)
            {
                string str = name;
                if (str != null)
                {
                    switch (str)
                    {
                        case "shield-1":
                            --(this.Game as PVEGame).TotalKillCount;
                            break;
                        case "shield-2":
                            (this.Game as PVEGame).TotalKillCount -= 2;
                            break;
                        case "shield-3":
                            (this.Game as PVEGame).TotalKillCount -= 3;
                            break;
                        case "shield-4":
                            (this.Game as PVEGame).TotalKillCount -= 4;
                            break;
                        case "shield-5":
                            (this.Game as PVEGame).TotalKillCount -= 5;
                            break;
                        case "shield-6":
                            (this.Game as PVEGame).TotalKillCount -= 5;
                            break;
                        case "shield1":
                            ++(this.Game as PVEGame).TotalKillCount;
                            break;
                        case "shield2":
                            (this.Game as PVEGame).TotalKillCount += 2;
                            break;
                        case "shield3":
                            (this.Game as PVEGame).TotalKillCount += 3;
                            break;
                        case "shield4":
                            (this.Game as PVEGame).TotalKillCount += 4;
                            break;
                        case "shield5":
                            (this.Game as PVEGame).TotalKillCount += 5;
                            break;
                        case "shield6":
                            (this.Game as PVEGame).TotalKillCount += 6;
                            break;
                    }
                }
            }
            if ((this.Game as PVEGame).TotalKillCount > 0)
                return;
            (this.Game as PVEGame).TotalKillCount = 0;
        }

        public virtual void PickBox(Box box)
        {
            if (box.Type > 1)
            {
                box.Die();
                if ((this as Player).psychic >= (this as Player).MaxPsychic)
                    return;
                (this as Player).psychic += box.Type == 2 ? 10 : 20;
            }
            else
            {
                box.UserID = this.Id;
                box.Die();
                if (this.m_syncAtTime)
                    this.m_game.SendGamePickBox(this, box.Id, 0, "");
                if (!this.IsLiving || !(this is Player))
                    return;
                (this as Player).OpenBox(box.Id);
            }
        }

        public bool PlayerBeat(
          Living target,
          string action,
          int demageAmount,
          int criticalAmount,
          int delay)
        {
            if (target == null || !target.IsLiving)
                return false;
            demageAmount = this.MakeDamage(target);
            this.OnBeforeTakedDamage(target, ref demageAmount, ref criticalAmount);
            this.StartAttacked();
            this.m_game.AddAction((IAction)new LivingBeatAction(this, target, demageAmount, criticalAmount, action, delay, 1, 0));
            return true;
        }

        public void PlayMovie(string action, int delay, int MovieTime)
        {
            this.PlayMovie(action, delay, MovieTime, (LivingCallBack)null);
        }

        public void PlayMovie(string action, int delay, int MovieTime, LivingCallBack callBack)
        {
            this.m_game.AddAction((IAction)new LivingPlayeMovieAction(this, action, delay, MovieTime, callBack));
        }

        public override void PrepareNewTurn()
        {
            this.ShootMovieDelay = 0;
            this.CurrentDamagePlus = 1f;
            this.CurrentShootMinus = 1f;
            this.IgnoreArmor = false;
            this.ControlBall = false;
            this.NoHoleTurn = false;
            this.CurrentIsHitTarget = false;
            this.OnBeginNewTurn();
        }

        public virtual void PrepareSelfTurn()
        {
            this.OnBeginSelfTurn();
        }

        public bool RangeAttacking(int fx, int tx, string action, int delay, bool directDamage)
        {
            return this.RangeAttacking(fx, tx, action, delay, true, directDamage, (List<Player>)null);
        }

        public bool RangeAttacking(int fx, int tx, string action, int delay, List<Player> players)
        {
            if (!this.IsLiving)
                return false;
            this.m_game.AddAction((IAction)new LivingRangeAttackingAction(this, fx, tx, action, delay, players));
            return true;
        }

        public bool RangeAttacking(
          int fx,
          int tx,
          string action,
          int delay,
          List<Living> exceptPlayers,
          int type)
        {
            bool flag;
            if (this.IsLiving)
            {
                this.m_game.AddAction((IAction)new LivingRangeAttackingAction3(this, fx, tx, action, delay, exceptPlayers, type));
                flag = true;
            }
            else
                flag = false;
            return flag;
        }

        public bool RangeAttacking(
          int fx,
          int tx,
          string action,
          int delay,
          bool removeFrost,
          bool directDamage,
          List<Player> players)
        {
            if (!this.IsLiving)
                return false;
            this.m_game.AddAction((IAction)new LivingRangeAttackingAction2(this, fx, tx, action, delay, removeFrost, directDamage, players));
            return true;
        }

        public virtual int ReducedBlood(int value)
        {
            this.m_blood += value;
            if (this.m_blood > this.m_maxBlood)
                this.m_blood = this.m_maxBlood;
            if (this.m_syncAtTime)
                this.m_game.SendGameUpdateHealth(this, 1, value);
            if (this.m_blood <= 0)
                this.Die();
            return value;
        }

        public virtual void Reset()
        {
            this.m_blood = this.m_maxBlood;
            this.m_isFrost = false;
            this.m_isHide = false;
            this.m_isNoHole = false;
            this.m_isLiving = true;
            this.m_blockTurn = false;
            this.TurnNum = 0;
            this.TotalHurt = 0;
            this.TotalKill = 0;
            this.TotalShootCount = 0;
            this.TotalHitTargetCount = 0;
            this.TotalCure = 0;
        }

        public void Say(string msg, int type, int delay)
        {
            this.m_game.AddAction((IAction)new LivingSayAction(this, msg, type, delay, 1000));
        }

        public void Say(string msg, int type, int delay, int finishTime)
        {
            this.m_game.AddAction((IAction)new LivingSayAction(this, msg, type, delay, finishTime));
        }

        public void Seal(Player player, int type, int delay)
        {
            this.m_game.AddAction((IAction)new LivingSealAction(this, (Living)player, type, delay));
        }

        public void Seal(Living target, int type, int delay)
        {
            this.m_game.AddAction((IAction)new LivingSealAction(this, target, type, delay));
        }

        public void SetHidden(bool state)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "visible", state.ToString());
        }

        public void SetIceFronze(Living living)
        {
            new IceFronzeEffect(2).Start(this);
            this.BeginNextTurn -= new LivingEventHandle(this.SetIceFronze);
        }

        public void SetIndian(bool state)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendPlayerPicture(this, 34, state);
        }

        public void SetNiutou(bool state)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendPlayerPicture(this, 33, state);
        }

        public void SetOffsetY(int p)
        {
            this.m_game.method_34(this, "offsetY", p.ToString());
        }

        public void SetRelateDemagemRect(int x, int y, int width, int height)
        {
            this.m_demageRect.X = x;
            this.m_demageRect.Y = y;
            this.m_demageRect.Width = width;
            this.m_demageRect.Height = height;
        }

        public void SetSeal(bool state)
        {
            if (this.m_isSeal == state)
                return;
            this.m_isSeal = state;
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "silenceMany", state.ToString());
        }

        public void SetSeal(bool state, int type)
        {
            if (this.m_isSeal == state)
                return;
            this.m_isSeal = state;
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGameUpdateSealState(this, type);
        }

        public void SetSystemState(bool state)
        {
            if (this.m_isSeal != state)
                this.m_isSeal = state;
            this.m_game.SendGamePlayerProperty(this, "system", state.ToString());
        }

        public void SetTargeting(bool state)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendPlayerPicture(this, 7, state);
        }

        public void SetupPetEffect()
        {
            this.m_petEffects = new PetEffectInfo();
            this.m_petEffects.CritActive = false;
            this.m_petEffects.ActivePetHit = false;
            this.m_petEffects.PetDelay = 0;
            this.m_petEffects.PetBaseAtt = 0;
            this.m_petEffects.CurrentUseSkill = 0;
            this.m_petEffects.ActiveGuard = false;
        }

        public void SetVisible(bool state)
        {
            this.m_game.method_34(this, "visible", state.ToString());
        }

        public void SetXY(int x, int y, int delay)
        {
            this.m_game.AddAction((IAction)new LivingDirectSetXYAction(this, x, y, delay));
        }

        public bool Shoot(int bombId, int x, int y, int force, int angle, int bombCount, int delay)
        {
            this.m_game.AddAction((IAction)new LivingShootAction(this, bombId, x, y, force, angle, bombCount, delay, 0, 0.0f, 0));
            return true;
        }
        /*
                public bool ShootImp(int bombId, int x, int y, int force, int angle, int bombCount)
                {
                    BallInfo ball = BallMgr.FindBall(bombId);
                    Tile tile = BallMgr.FindTile(bombId);
                    BombType ballType = BallMgr.GetBallType(bombId);
                    int num1 = (int)((double)this.m_map.wind * 10.0);
                    if (ball == null)
                        return false;
                    GSPacketIn pkg = new GSPacketIn((short)91, this.Id);
                    pkg.Parameter1 = this.Id;
                    pkg.WriteByte((byte)2);
                    pkg.WriteInt(num1);
                    pkg.WriteBoolean(num1 > 0);
                    pkg.WriteByte(this.m_game.GetVane(num1, 1));
                    pkg.WriteByte(this.m_game.GetVane(num1, 2));
                    pkg.WriteByte(this.m_game.GetVane(num1, 3));
                    pkg.WriteInt(bombCount);
                    float val1 = 0.0f;
                    SimpleBomb simpleBomb1 = (SimpleBomb)null;
                    for (int index = 0; index < bombCount; ++index)
                    {
                        double num2 = 1.0;
                        int num3 = 0;
                        switch (index)
                        {
                            case 1:
                                num2 = 0.9;
                                num3 = -5;
                                break;
                            case 2:
                                num2 = 1.1;
                                num3 = 5;
                                break;
                        }
                        int num4 = (int)((double)force * num2 * Math.Cos((double)(angle + num3) / 180.0 * Math.PI));
                        int num5 = (int)((double)force * num2 * Math.Sin((double)(angle + num3) / 180.0 * Math.PI));
                        SimpleBomb simpleBomb2 = new SimpleBomb(this.m_game.PhysicalId++, ballType, this, this.m_game, ball, tile, this.ControlBall, angle);
                        simpleBomb2.SetXY(x, y);
                        simpleBomb2.setSpeedXY(num4, num5);
                        this.m_map.AddPhysical((Physics)simpleBomb2);
                        simpleBomb2.StartMoving();
                        if (index == 0)
                            simpleBomb1 = simpleBomb2;
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                        pkg.WriteBoolean(simpleBomb2.DigMap);
                        pkg.WriteInt(simpleBomb2.Id);
                        pkg.WriteInt(x);
                        pkg.WriteInt(y);
                        pkg.WriteInt(num4);
                        pkg.WriteInt(num5);
                        pkg.WriteInt(simpleBomb2.BallInfo.ID);
                        if ((uint)this.FlyingPartical > 0U)
                            pkg.WriteString(this.FlyingPartical.ToString());
                        else
                            pkg.WriteString(ball.FlyingPartical);
                        pkg.WriteInt(simpleBomb2.BallInfo.Radii * 1000 / 4);
                        pkg.WriteInt((int)simpleBomb2.BallInfo.Power * 1000);
                        pkg.WriteInt(simpleBomb2.Actions.Count);
                        foreach (BombAction action in simpleBomb2.Actions)
                        {
                            pkg.WriteInt(action.TimeInt);
                            pkg.WriteInt(action.Type);
                            pkg.WriteInt(action.Param1);
                            pkg.WriteInt(action.Param2);
                            pkg.WriteInt(action.Param3);
                            pkg.WriteInt(action.Param4);
                        }
                        val1 = Math.Max(val1, simpleBomb2.LifeTime);
                    }
                    if (this is Player && this.countBoom > 0 && (this.countBoom >= 3 && bombCount >= 3))
                        this.m_game.AddAction((IAction)new FightAchievementAction(this, eFightAchievementType.GodOfPrecision, this.Direction, 1200));
                    int count = simpleBomb1.PetActions.Count;
                    if (count > 0 && this.PetEffects.PetBaseAtt > 0)
                    {
                        if (simpleBomb1.PetActions[0].Type == -1)
                        {
                            pkg.WriteInt(0);
                        }
                        else
                        {
                            pkg.WriteInt(count);
                            foreach (BombAction petAction in simpleBomb1.PetActions)
                            {
                                pkg.WriteInt(petAction.Param1);
                                pkg.WriteInt(petAction.Param2);
                                pkg.WriteInt(petAction.Param4);
                                pkg.WriteInt(petAction.Param3);
                            }
                        }
                        pkg.WriteInt(1);
                    }
                    else
                    {
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                    }
                    this.m_game.SendToAll(pkg);
                    int num6 = (int)(((double)val1 + 1.0 + (double)(bombCount / 3)) * 1000.0) + this.PetEffects.PetDelay + this.SpecialSkillDelay;
                    this.m_game.WaitTime((int)(((double)val1 + 2.0 + (double)(bombCount / 3)) * 1000.0) + this.PetEffects.PetDelay + this.SpecialSkillDelay);
                    this.LastLifeTimeShoot = num6;
                    return true;
                }*/
        public bool ShootImp(int bombId, int x, int y, int force, int angle, int bombCount, int shootCount) // protected function __shoot(event:CrazyTankSocketEvent) : void
        {
            BallInfo ballInfo = BallMgr.FindBall(bombId);
            Tile shape = BallMgr.FindTile(bombId);
            BombType ballType = BallMgr.GetBallType(bombId);
            int num = (int)(this.m_map.wind * 10f);
            if (ballInfo != null)
            {
                GSPacketIn gSPacketIn = new GSPacketIn(91, base.Id);
                gSPacketIn.Parameter1 = base.Id;
                gSPacketIn.WriteByte(2);
                gSPacketIn.WriteInt(num);
                gSPacketIn.WriteBoolean(num > 0);
                gSPacketIn.WriteByte(this.m_game.GetVane(num, 1));
                gSPacketIn.WriteByte(this.m_game.GetVane(num, 2));
                gSPacketIn.WriteByte(this.m_game.GetVane(num, 3));
                gSPacketIn.WriteInt(bombCount);
                float num2 = 0f;
                SimpleBomb simpleBomb = null;
                for (int i = 0; i < bombCount; i++)
                {
                    double num3 = 1.0;
                    int num4 = 0;
                    if (i == 1)
                    {
                        num3 = 0.9;
                        num4 = -5;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            num3 = 1.1;
                            num4 = 5;
                        }
                    }
                    int num5 = (int)((double)force * num3 * Math.Cos((double)(angle + num4) / 180.0 * 3.1415926535897931));
                    int num6 = (int)((double)force * num3 * Math.Sin((double)(angle + num4) / 180.0 * 3.1415926535897931));
                    SimpleBomb simpleBomb2 = new SimpleBomb(this.m_game.PhysicalId++, ballType, this, this.m_game, ballInfo, shape, this.ControlBall, angle);
                    simpleBomb2.SetXY(x, y);
                    simpleBomb2.setSpeedXY(num5, num6);
                    this.m_map.AddPhysical(simpleBomb2);
                    simpleBomb2.StartMoving();
                    if (i == 0)
                    {
                        simpleBomb = simpleBomb2;
                    }
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteBoolean(simpleBomb2.DigMap);
                    gSPacketIn.WriteInt(simpleBomb2.Id);
                    gSPacketIn.WriteInt(x);
                    gSPacketIn.WriteInt(y);
                    gSPacketIn.WriteInt(num5);
                    gSPacketIn.WriteInt(num6);
                    gSPacketIn.WriteInt(simpleBomb2.BallInfo.ID);
                    if (this.FlyingPartical != 0)
                    {
                        gSPacketIn.WriteString(this.FlyingPartical.ToString());
                    }
                    else
                    {
                        gSPacketIn.WriteString(ballInfo.FlyingPartical);
                    }
                    gSPacketIn.WriteInt(simpleBomb2.BallInfo.Radii * 1000 / 4);
                    gSPacketIn.WriteInt((int)simpleBomb2.BallInfo.Power * 1000);
                    gSPacketIn.WriteInt(simpleBomb2.Actions.Count);
                    foreach (BombAction current in simpleBomb2.Actions)
                    {
                        gSPacketIn.WriteInt(current.TimeInt);
                        gSPacketIn.WriteInt(current.Type);
                        gSPacketIn.WriteInt(current.Param1);
                        gSPacketIn.WriteInt(current.Param2);
                        gSPacketIn.WriteInt(current.Param3);
                        gSPacketIn.WriteInt(current.Param4);
                    }
                    num2 = Math.Max(num2, simpleBomb2.LifeTime);
                }
                int num7 = 0;
                int count = simpleBomb.PetActions.Count;
                if (count > 0 && this.PetEffects.PetBaseAtt > 0)
                {
                    num7 = 2;
                    if (simpleBomb.PetActions[0].Type == -1)
                    {
                        gSPacketIn.WriteInt(0);
                    }
                    else
                    {
                        gSPacketIn.WriteInt(count);
                        foreach (BombAction current2 in simpleBomb.PetActions)
                        {
                            gSPacketIn.WriteInt(current2.Param1);
                            gSPacketIn.WriteInt(current2.Param2);
                            gSPacketIn.WriteInt(current2.Param4);
                            gSPacketIn.WriteInt(current2.Param3);
                        }
                    }
                    gSPacketIn.WriteInt(1);
                }
                else
                {
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteInt(0);
                }
                gSPacketIn.WriteInt(0);

                this.m_game.SendToAll(gSPacketIn);
                this.m_game.WaitTime((int)((num2 + 2f + (float)num7 + (float)(bombCount / 3)) * 1000f));
                return true;
            }
            return false;
        }

        public virtual bool PetTakeDamage(
          Living source,
          ref int damageAmount,
          ref int criticalAmount,
          string msg)
        {
            bool flag = false;
            if (this.m_blood > 0)
            {
                this.m_blood -= damageAmount + criticalAmount;
                if (this.m_blood <= 0)
                    this.Die();
                flag = true;
            }
            return flag;
        }

        public bool ShootPoint(
          int x,
          int y,
          int bombId,
          int minTime,
          int maxTime,
          int bombCount,
          float time,
          int delay)
        {
            this.m_game.AddAction((IAction)new LivingShootAction(this, bombId, x, y, 0, 0, bombCount, minTime, maxTime, time, delay));
            return true;
        }

        public bool ShootPoint(
          int x,
          int y,
          int bombId,
          int minTime,
          int maxTime,
          int bombCount,
          float time,
          int delay,
          LivingCallBack callBack)
        {
            this.m_game.AddAction((IAction)new LivingShootAction(this, bombId, x, y, 0, 0, bombCount, minTime, maxTime, time, delay, callBack));
            return true;
        }

        public virtual void SpeedMultX(int value)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "speedX", value.ToString());
        }

        public void SpeedMultX(int value, string _tpye)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, _tpye, value.ToString());
        }

        public void SpeedMultY(int value)
        {
            if (!this.m_syncAtTime)
                return;
            this.m_game.SendGamePlayerProperty(this, "speedY", value.ToString());
        }

        public void OffSeal(Living target, int delay)
        {
            this.m_game.AddAction((IAction)new LivingOffSealAction(this, target, delay));
        }

        public int ChangeMaxBeatDis
        {
            get
            {
                return this.MaxBeatDis;
            }
            set
            {
                this.MaxBeatDis = value;
            }
        }

        public void StartAttacked()
        {
            this.OnStartAttacked();
        }

        public virtual void StartAttacking()
        {
            if (this.m_isAttacking)
                return;
            this.m_isAttacking = true;
            this.OnStartAttacking();
        }

        public override void StartMoving()
        {
            this.StartMoving(0, 30);
        }

        public virtual void StartMoving(int delay, int speed)
        {
            if (this.Config.IsFly)
                return;
            Point point = this.m_map.FindYLineNotEmptyPointDown(this.X, this.Y);
            if (point == Point.Empty)
                point = new Point(this.X, this.m_game.Map.Bound.Height + 1);
            if (point.Y == this.Y)
                return;
            if (this.m_map.IsOutMap(point.X, point.Y))
            {
                this.Die();
                if (this.Game.CurrentLiving != this && this.Game.CurrentLiving is Player && (this is Player && this.Team != this.Game.CurrentLiving.Team))
                {
                    Player currentLiving = this.Game.CurrentLiving as Player;
                    currentLiving.PlayerDetail.OnKillingLiving((AbstractGame)this.m_game, 1, this.Id, this.IsLiving, 0);
                    ++this.Game.CurrentLiving.TotalKill;
                    currentLiving.CalculatePlayerOffer(this as Player);
                }
            }
            if (this.m_map.IsEmpty(this.X, this.Y))
                this.FallFrom(this.X, this.Y, (string)null, delay, 0, speed);
            base.StartMoving();
        }

        public virtual void StopAttacking()
        {
            if (!this.m_isAttacking)
                return;
            this.m_isAttacking = false;
            this.OnStopAttacking();
        }

        public virtual bool TakeDamage(
          Living source,
          ref int damageAmount,
          ref int criticalAmount,
          string msg,
          int delay)
        {
            if (this.Config.IsHelper && (this is SimpleNpc || this is SimpleBoss) && source is Player)
                return false;
            bool flag = false;
            if (!this.IsFrost && this.m_blood > 0)
            {
                if (source != this || source.Team == this.Team)
                {
                    this.OnBeforeTakedDamage(source, ref damageAmount, ref criticalAmount);
                    this.StartAttacked();
                }
                int num1 = damageAmount + criticalAmount < 0 ? 1 : damageAmount + criticalAmount;
                if (this is Player)
                {
                    int reduceDamePlus = (this as Player).PlayerDetail.PlayerCharacter.ReduceDamePlus;
                    int num2 = num1 * reduceDamePlus / 100;
                    num1 -= num2;
                }
                this.m_blood -= num1;
                int num3 = this.m_maxBlood * 30 / 100;
                if (this.m_syncAtTime)
                {
                    if (this is SimpleBoss && (((SimpleBoss)this).NpcInfo.ID == 1207 || ((SimpleBoss)this).NpcInfo.ID == 1307))
                        this.m_game.SendGameUpdateHealth(this, 6, num1);
                    else
                        this.m_game.SendGameUpdateHealth(this, 1, num1);
                }
                this.OnAfterTakedDamage(source, damageAmount, criticalAmount);
                if (this.m_blood <= 0)
                {
                    if (criticalAmount > 0 && this is Player)
                        this.m_game.AddAction((IAction)new FightAchievementAction(source, eFightAchievementType.ExpertInStrokes, source.Direction, 1000));
                    this.Die();
                }
                source.OnAfterKillingLiving(this, damageAmount, criticalAmount);
                flag = true;
            }
            this.EffectList.StopEffect(typeof(IceFronzeEffect));
            this.EffectList.StopEffect(typeof(HideEffect));
            this.EffectList.StopEffect(typeof(NoHoleEffect));
            return flag;
        }

        public void OnMakeDamage(Living living)
        {
            if (this.BeginAttacked == null)
                return;
            this.BeginAttacked(living);
        }

        public string ActionStr
        {
            get
            {
                return this.m_action;
            }
            set
            {
                this.m_action = value;
            }
        }

        public bool AutoBoot
        {
            get
            {
                return this.m_autoBoot;
            }
            set
            {
                this.m_autoBoot = value;
            }
        }

        public bool BlockTurn
        {
            get
            {
                return this.m_blockTurn;
            }
            set
            {
                this.m_blockTurn = value;
            }
        }

        public int Blood
        {
            get
            {
                return this.m_blood;
            }
            set
            {
                this.m_blood = value;
            }
        }

        public LivingConfig Config
        {
            get
            {
                return this.m_config;
            }
            set
            {
                this.m_config = value;
            }
        }

        public int Degree
        {
            get
            {
                return this.m_degree;
            }
            set
            {
                this.m_degree = value;
            }
        }

        public void ChangeDamage(double value)
        {
            this.BaseDamage += value;
            if (this.BaseDamage >= 0.0)
                return;
            this.BaseDamage = 0.0;
        }

        public int Direction
        {
            get
            {
                return this.m_direction;
            }
            set
            {
                if (this.m_direction == value)
                    return;
                this.m_direction = value;
                this.SetRect(-this.m_rect.X - this.m_rect.Width, this.m_rect.Y, this.m_rect.Width, this.m_rect.Height);
                this.SetRectBomb(-this.m_rectBomb.X - this.m_rectBomb.Width, this.m_rectBomb.Y, this.m_rectBomb.Width, this.m_rectBomb.Height);
                this.SetRelateDemagemRect(-this.m_demageRect.X - this.m_demageRect.Width, this.m_demageRect.Y, this.m_demageRect.Width, this.m_demageRect.Height);
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendLivingUpdateDirection(this);
            }
        }

        public int DoAction
        {
            get
            {
                return this.m_doAction;
            }
            set
            {
                if (this.m_doAction == value)
                    return;
                this.m_doAction = value;
            }
        }

        public EffectList EffectList
        {
            get
            {
                return this.m_effectList;
            }
        }

        public int FallCount
        {
            get
            {
                return this.m_FallCount;
            }
            set
            {
                this.m_FallCount = value;
            }
        }

        public FightBufferInfo FightBuffers
        {
            get
            {
                return this.m_fightBufferInfo;
            }
            set
            {
                this.m_fightBufferInfo = value;
            }
        }

        public int FindCount
        {
            get
            {
                return this.m_FindCount;
            }
            set
            {
                this.m_FindCount = value;
            }
        }

        public int FireX { get; set; }

        public int FireY { get; set; }

        public BaseGame Game
        {
            get
            {
                return this.m_game;
            }
        }

        public bool IsAttacking
        {
            get
            {
                return this.m_isAttacking;
            }
        }

        public bool IsFrost
        {
            get
            {
                return this.m_isFrost;
            }
            set
            {
                if (this.m_isFrost == value)
                    return;
                this.m_isFrost = value;
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendGameUpdateFrozenState(this);
            }
        }

        public bool IsHide
        {
            get
            {
                return this.m_isHide;
            }
            set
            {
                if (this.m_isHide == value)
                    return;
                this.m_isHide = value;
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendGameUpdateHideState(this);
            }
        }

        public bool IsNoHole
        {
            get
            {
                return this.m_isNoHole;
            }
            set
            {
                if (this.m_isNoHole == value)
                    return;
                this.m_isNoHole = value;
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendGameUpdateNoHoleState(this);
            }
        }

        public bool IsSay { get; set; }

        public int MaxBlood
        {
            get
            {
                return this.m_maxBlood;
            }
            set
            {
                this.m_maxBlood = value;
            }
        }

        public string ModelId
        {
            get
            {
                return this.m_modelId;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public PetEffectInfo PetEffects
        {
            get
            {
                return this.m_petEffects;
            }
            set
            {
                this.m_petEffects = value;
            }
        }

        public int PictureTurn
        {
            get
            {
                return this.m_pictureTurn;
            }
            set
            {
                this.m_pictureTurn = value;
            }
        }

        public bool SetSeal2
        {
            get
            {
                return this.m_isSeal;
            }
            set
            {
                if (this.m_isSeal == value)
                    return;
                this.m_isSeal = value;
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendGameUpdateSealState(this, 0);
            }
        }

        public int SpecialSkillDelay
        {
            get
            {
                return this.m_specialSkillDelay;
            }
            set
            {
                this.m_specialSkillDelay = value;
            }
        }

        public int State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                if (this.m_state == value)
                    return;
                this.m_state = value;
                if (!this.m_syncAtTime)
                    return;
                this.m_game.SendLivingUpdateAngryState(this);
            }
        }

        public bool SyncAtTime
        {
            get
            {
                return this.m_syncAtTime;
            }
            set
            {
                this.m_syncAtTime = value;
            }
        }

        public int Team
        {
            get
            {
                return this.m_team;
            }
        }

        public eLivingType Type
        {
            get
            {
                return this.m_type;
            }
            set
            {
                this.m_type = value;
            }
        }

        public bool VaneOpen
        {
            get
            {
                return this.m_vaneOpen;
            }
            set
            {
                this.m_vaneOpen = value;
            }
        }

        public int STEP_X
        {
            get
            {
                return this.Game.Map.Info.ID == 1164 ? 1 : 3;
            }
        }

        public int STEP_Y
        {
            get
            {
                return this.Game.Map.Info.ID == 1164 ? 3 : 7;
            }
        }

        public PetEffectList PetEffectList
        {
            get
            {
                return this.petEffectList_0;
            }
        }
    }
}
