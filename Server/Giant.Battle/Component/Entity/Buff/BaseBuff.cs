﻿using Giant.Core;
using Giant.Model;
using Giant.Util;
using System;

namespace Giant.Battle
{
    public abstract class BaseBuff : Entity, IUpdate
    {
        protected Unit owner;
        public bool IsBuffEnd { get; set; }

        public int Id { get; private set; }
        public BuffType BuffType { get; private set; }
        public DateTime EndTime { get; private set; }

        public void Init(BuffModel model)
        {
            Id = model.Id;
            BuffType = (BuffType)model.BuffType;
            EndTime = TimeHelper.Now.AddSeconds(model.DuringTime);

            owner = GetParent<BuffComponent>().GetParent<Unit>();
        }

        public void Start()
        {
            IsBuffEnd = false;

            OnStart();
        }

        public void End()
        {
            IsBuffEnd = true;

            OnEnd();
            GetParent<BuffComponent>().RemoveBuff(InstanceId);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected virtual void OnStart() { }
        protected virtual void OnEnd() { }

        public virtual void Reset() { }
        public virtual void Update(double dt)
        {
            if (TimeHelper.Now >= EndTime)
            {
                End();
            }
        }
    }
}
