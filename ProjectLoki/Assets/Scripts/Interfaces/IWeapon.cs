using UnityEngine;

public interface IWeapon{

    void Activate(Vector3 position);

    void ReduceCooldowns(float delta);
}
