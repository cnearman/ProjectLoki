using UnityEngine;

public interface IWeapon{

    void Activate(Vector3 position, Vector3 rotation);

    void ReduceCooldowns(float delta);
}
