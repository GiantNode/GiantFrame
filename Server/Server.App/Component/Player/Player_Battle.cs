﻿using Giant.Battle;
using UnityEngine;

namespace Server.App
{
    partial class Player
    {
        public void CastSkill(int skillId, int targetId, Vector2 direction)
        {
            PlayerUnit.CastSkill(skillId, targetId, direction);
        }
    }
}
