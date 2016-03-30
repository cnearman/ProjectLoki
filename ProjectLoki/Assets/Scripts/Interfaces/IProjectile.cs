using UnityEngine;
using System.Collections;

public interface IProjectile  {
    void ApplyEffects(RaycastHit hit);
    void ApplyEffects(RaycastHit hit, Vector3 direction);
}
