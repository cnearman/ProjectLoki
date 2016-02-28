using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLoki.Weapons
{
    public class EffectClass : BaseClass
    {
        public EffectClassType Class;
    }

    public enum EffectClassType
    {
        None = 0,
        Neutral = 1,
        Destructible = 2,
        TeamA = 3,
        TeamB = 4
    }
}
