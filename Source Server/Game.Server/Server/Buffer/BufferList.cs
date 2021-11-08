// Decompiled with JetBrains decompiler
// Type: Game.Server.Buffer.BufferList
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Buffer
{
    public class BufferList
    {
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private object object_0;
        protected List<AbstractBuffer> m_buffers;
        protected ArrayList m_clearList;
        protected volatile sbyte m_changesCount;
        private GamePlayer gamePlayer_0;
        protected ArrayList m_changedBuffers;
        private int int_0;

        public BufferList(GamePlayer player)
        {
            this.m_changedBuffers = new ArrayList();
            this.gamePlayer_0 = player;
            this.object_0 = new object();
            this.m_buffers = new List<AbstractBuffer>();
            this.m_clearList = new ArrayList();
        }

        public void LoadFromDatabase(int playerId)
        {
            lock (this.object_0)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    BufferInfo[] userBuffer = playerBussiness.GetUserBuffer(playerId);
                    this.BeginChanges();
                    foreach (BufferInfo info in userBuffer)
                        BufferList.CreateBuffer(info)?.Start(this.gamePlayer_0);
                    foreach (ConsortiaBufferInfo info in playerBussiness.GetUserConsortiaBuffer(this.gamePlayer_0.PlayerCharacter.ConsortiaID))
                        BufferList.CreateConsortiaBuffer(info)?.Start(this.gamePlayer_0);
                    this.CommitChanges();
                }
                this.Update();
            }
        }

        public void SaveToDatabase()
        {
            lock (this.object_0)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    foreach (AbstractBuffer buffer in this.m_buffers)
                        playerBussiness.SaveBuffer(buffer.Info);
                    foreach (BufferInfo clear in this.m_clearList)
                        playerBussiness.SaveBuffer(clear);
                    this.m_clearList.Clear();
                }
            }
        }

        public bool AddBuffer(AbstractBuffer buffer)
        {
            lock (this.m_buffers)
                this.m_buffers.Add(buffer);
            this.OnBuffersChanged(buffer);
            return true;
        }

        public bool RemoveBuffer(AbstractBuffer buffer)
        {
            lock (this.m_buffers)
            {
                if (this.m_buffers.Remove(buffer))
                    this.m_clearList.Add((object)buffer.Info);
            }
            this.OnBuffersChanged(buffer);
            return true;
        }

        public void UpdateBuffer(AbstractBuffer buffer)
        {
            this.OnBuffersChanged(buffer);
        }

        protected void OnBuffersChanged(AbstractBuffer buffer)
        {
            if (!this.m_changedBuffers.Contains((object)buffer))
                this.m_changedBuffers.Add((object)buffer);
            if (this.int_0 > 0 || this.m_changedBuffers.Count <= 0)
                return;
            this.UpdateChangedBuffers();
        }

        public void BeginChanges()
        {
            Interlocked.Increment(ref this.int_0);
        }

        public void CommitChanges()
        {
            int num = Interlocked.Decrement(ref this.int_0);
            if (num < 0)
            {
                if (BufferList.ilog_0.IsErrorEnabled)
                    BufferList.ilog_0.Error((object)("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
                Thread.VolatileWrite(ref this.int_0, 0);
            }
            if (num > 0 || this.m_changedBuffers.Count <= 0)
                return;
            this.UpdateChangedBuffers();
        }

        public void UpdateChangedBuffers()
        {
            List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();
            Dictionary<string, BufferInfo> bufflist = new Dictionary<string, BufferInfo>();
            foreach (AbstractBuffer changedBuffer in this.m_changedBuffers)
                abstractBufferList.Add(changedBuffer);
            foreach (AbstractBuffer allBuffer in this.GetAllBuffers())
            {
                if (this.IsConsortiaBuff(allBuffer.Info.Type) && this.gamePlayer_0.IsConsortia())
                    bufflist.Add(allBuffer.Info.Data, allBuffer.Info);
            }
            GSPacketIn pkg = this.gamePlayer_0.Out.SendUpdateBuffer(this.gamePlayer_0, abstractBufferList.ToArray());
            if (this.gamePlayer_0.CurrentRoom != null)
                this.gamePlayer_0.CurrentRoom.SendToAll(pkg, this.gamePlayer_0);
            this.gamePlayer_0.Out.SendUpdateConsotiaBuffer(this.gamePlayer_0, bufflist);
            this.m_changedBuffers.Clear();
            bufflist.Clear();
        }

        public List<AbstractBuffer> GetAllBuffers()
        {
            List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();
            lock (this.object_0)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                    abstractBufferList.Add(buffer);
            }
            return abstractBufferList;
        }

        public bool IsConsortiaBuff(int type)
        {
            if (type > 100)
                return type < 115;
            return false;
        }

        public virtual AbstractBuffer GetOfType(Type bufferType)
        {
            lock (this.m_buffers)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                {
                    if (buffer.GetType().Equals(bufferType))
                        return buffer;
                }
            }
            return (AbstractBuffer)null;
        }

        public virtual AbstractBuffer GetOfType(BuffType type)
        {
            lock (this.m_buffers)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                {
                    if ((BuffType)buffer.Info.Type == type)
                        return buffer;
                }
            }
            return (AbstractBuffer)null;
        }

        public virtual void ResetAllPayBuffer()
        {
            List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();
            lock (this.m_buffers)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                {
                    if (buffer.Check() && buffer.IsPayBuff())
                    {
                        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(buffer.Info.TemplateID);
                        buffer.Info.ValidCount = itemTemplate.Property3;
                        this.UpdateBuffer(buffer);
                    }
                }
            }
        }

        public List<AbstractBuffer> GetAllBuffer()
        {
            List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();
            lock (this.object_0)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                    abstractBufferList.Add(buffer);
            }
            return abstractBufferList;
        }

        public void Update()
        {
            foreach (AbstractBuffer abstractBuffer in this.GetAllBuffer())
            {
                try
                {
                    if (!abstractBuffer.Check())
                        abstractBuffer.Stop();
                }
                catch (Exception ex)
                {
                    BufferList.ilog_0.Error((object)ex);
                }
            }
        }

        public static AbstractBuffer CreateBuffer(
          ItemTemplateInfo template,
          int ValidDate)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                BeginDate = DateTime.Now,
                ValidDate = ValidDate * 24 * 60,
                Value = template.Property2,
                Type = template.Property1,
                ValidCount = template.Property3,
                TemplateID = template.TemplateID,
                IsExist = true
            });
        }

        public static AbstractBuffer CreateConsortiaBuffer(ConsortiaBufferInfo info)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                Data = info.BufferID.ToString(),
                BeginDate = info.BeginDate,
                ValidDate = info.ValidDate,
                Value = info.Value,
                Type = info.Type,
                ValidCount = 1,
                IsExist = true
            });
        }

        public static AbstractBuffer CreatePayBuffer(
          int type,
          int Value,
          int ValidMinutes,
          int id)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                Data = id.ToString(),
                BeginDate = DateTime.Now,
                ValidDate = ValidMinutes,
                Value = Value,
                Type = type,
                ValidCount = 1,
                IsExist = true
            });
        }

        public static AbstractBuffer CreateBufferHour(
          ItemTemplateInfo template,
          int ValidHour)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                BeginDate = DateTime.Now,
                ValidDate = ValidHour * 60,
                Value = template.Property2,
                Type = template.Property1,
                IsExist = true
            });
        }
        public static AbstractBuffer CreateSaveLifeBuffer(int ValidCount)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                TemplateID = 11919,
                BeginDate = DateTime.Now,
                ValidDate = 1440,
                Value = 30,
                Type = 51,
                ValidCount = ValidCount,
                IsExist = true
            });
        }
        public static AbstractBuffer CreateBufferMinutes(
          ItemTemplateInfo template,
          int ValidMinutes)
        {
            return BufferList.CreateBuffer(new BufferInfo()
            {
                TemplateID = template.TemplateID,
                BeginDate = DateTime.Now,
                ValidDate = ValidMinutes,
                Value = template.Property2,
                Type = template.Property1,
                ValidCount = 1,
                IsExist = true
            });
        }

        public static AbstractBuffer CreateBuffer(BufferInfo info)
        {
            AbstractBuffer abstractBuffer = (AbstractBuffer)null;
            switch (info.Type)
            {
                case 11:
                    abstractBuffer = (AbstractBuffer)new KickProtectBuffer(info);
                    break;
                case 12:
                    abstractBuffer = (AbstractBuffer)new OfferMultipleBuffer(info);
                    break;
                case 13:
                    abstractBuffer = (AbstractBuffer)new GPMultipleBuffer(info);
                    break;
                case 15:
                    abstractBuffer = (AbstractBuffer)new PropsBuffer(info);
                    break;
                case 50:
                    abstractBuffer = (AbstractBuffer)new AgiBuffer(info);
                    break;
                case 51:
                    abstractBuffer = (AbstractBuffer)new SaveLifeBuffer(info);
                    break;
                case 52:
                    abstractBuffer = (AbstractBuffer)new ReHealthBuffer(info);
                    break;
                case 70:
                    abstractBuffer = (AbstractBuffer)new CaddyGoodsBuffer(info);
                    break;
                case 71:
                    abstractBuffer = (AbstractBuffer)new TrainGoodsBuffer(info);
                    break;
                case 73:
                    abstractBuffer = (AbstractBuffer)new CardGetBuffer(info);
                    break;
                case 101:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddBloodGunCountBuffer(info);
                    break;
                case 102:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddDamageBuffer(info);
                    break;
                case 103:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddCriticalBuffer(info);
                    break;
                case 104:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddMaxBloodBuffer(info);
                    break;
                case 105:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddPropertyBuffer(info);
                    break;
                case 106:
                    abstractBuffer = (AbstractBuffer)new ConsortionReduceEnergyUseBuffer(info);
                    break;
                case 107:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddEnergyBuffer(info);
                    break;
                case 108:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddEffectTurnBuffer(info);
                    break;
                case 109:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddOfferRateBuffer(info);
                    break;
                case 110:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddPercentGoldOrGPBuffer(info);
                    break;
                case 111:
                    abstractBuffer = (AbstractBuffer)new ConsortionAddSpellCountBuffer(info);
                    break;
                case 112:
                    abstractBuffer = (AbstractBuffer)new ConsortionReduceDanderBuffer(info);
                    break;
            }
            return abstractBuffer;
        }

        public List<AbstractBuffer> GetAllBufferByTemplate()
        {
            List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();
            lock (this.object_0)
            {
                foreach (AbstractBuffer buffer in this.m_buffers)
                {
                    if (buffer.Info.TemplateID > 100)
                        abstractBufferList.Add(buffer);
                }
            }
            return abstractBufferList;
        }
    }
}
