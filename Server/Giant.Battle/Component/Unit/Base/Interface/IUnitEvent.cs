﻿using Giant.EnumUtil;

namespace Giant.Battle
{
    /// <summary>
    /// 场景单位所有的事件
    /// </summary>
    interface IUnitEvent
    {
        void OnMove();

        void OnSkillStart(Skill skill);
        void OnSkillEnd(Skill skill);
        void OnAddBuff(int buffId);
        void OnRemoveBuff(int buffId);
        void OnDamage(Unit caster, int damage);
        void OnNatureChange(NatureType type, int value);

        void OnDead();
        void OnRelive();
    }
}
