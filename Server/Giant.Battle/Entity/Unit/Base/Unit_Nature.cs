﻿namespace Giant.Battle
{
    public partial class Unit
    {
        protected NatureComponent NatureComponent { get; private set; }

        protected virtual void InitNature() 
        {
            NatureComponent = AddComponentWithParent<NatureComponent>();
        }

        public void NatureChange(NatureType type, int value)
        {
            MsgSource.OnNatureChange(this, type, value);
        }

        public void UpdateHP(int hp)
        {
            int value = NatureComponent.AddValue(NatureType.HP, hp);
            if (value <= 0)
            {
                IsDead = true;
                OnDead();
            }
        }
    }
}
