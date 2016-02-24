using UnityEngine;
using System.Collections.Generic;

namespace ProjectLoki.Weapons
{
    public class DefaultDistribution : FireDistribution
    {
        private float pMaxVariance;

        public DefaultDistribution()
        {
            this.pMaxVariance = 0;
        }

        public DefaultDistribution(float maxVariance)
        {
            this.pMaxVariance = maxVariance;
        }

        public override IEnumerable<Fire> GetDistribution(float accuracy)
        {
            List<Fire> distribution = new List<Fire>();

            float variance = 1 - accuracy;

            float verticalAdjustment = Random.Range(0, pMaxVariance * variance);
            float horizontalAdjustment = Random.Range(0, pMaxVariance * variance);

            Vector3 angle = Vector3.forward + (Vector3.right * horizontalAdjustment) + (Vector3.up * verticalAdjustment);

            distribution.Add(new Fire(Vector3.zero, angle));

            return distribution;
        }
    }
}
