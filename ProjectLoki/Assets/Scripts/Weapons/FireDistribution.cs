using UnityEngine;
using System.Collections.Generic;

namespace ProjectLoki.Weapons
{
    public abstract class FireDistribution
    {
        public abstract IEnumerable<Fire> GetDistribution(float accuracy);
    }
}