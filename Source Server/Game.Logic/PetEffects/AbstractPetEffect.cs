// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.AbstractPetEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness.Managers;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;

namespace Game.Logic.PetEffects
{
    public abstract class AbstractPetEffect
    {
        protected Random rand = new Random();
        public bool IsTrigger;
        private PetSkillElementInfo m_info;
        protected Living m_living;
        private ePetEffectType m_type;

        public AbstractPetEffect(ePetEffectType type, string ElementID)
        {
            this.m_type = type;
            this.m_info = PetMgr.FindPetSkillElement(int.Parse(ElementID));
            if (this.m_info != null)
                return;
            this.m_info = new PetSkillElementInfo();
            this.m_info.EffectPic = "";
            this.m_info.Pic = -1;
            this.m_info.Value = 1;
        }

        public virtual void OnAttached(Living living)
        {
        }

        public virtual void OnRemoved(Living living)
        {
        }

        public virtual void OnPaused(Living living)
        {
        }

        public virtual bool Start(Living living)
        {
            this.m_living = living;
            return this.m_living.PetEffectList.Add(this);
        }

        public virtual bool Pause()
        {
            return this.m_living != null && this.m_living.PetEffectList.Pause(this);
        }

        public virtual bool Stop()
        {
            if (this.m_living != null)
                return this.m_living.PetEffectList.Remove(this);
            return false;
        }

        public PetSkillElementInfo Info
        {
            get
            {
                return this.m_info;
            }
        }

        public ePetEffectType Type
        {
            get
            {
                return this.m_type;
            }
        }

        public int TypeValue
        {
            get
            {
                return (int)this.m_type;
            }
        }
    }
}
