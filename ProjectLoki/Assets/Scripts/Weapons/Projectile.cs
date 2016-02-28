using UnityEngine;
using System.Collections.Generic;

public class DefaultProjectile : IProjectile {
    public IEnumerable<Effect> Effects;

    public void ApplyEffects(RaycastHit hit)
    {
        Debug.Log(hit.collider.name);
    }
}
