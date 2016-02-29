﻿using UnityEngine;

public interface IWeapon{

    void Activate(Vector3 position, Vector3 rotation, double timeTriggered);

    void ReduceCooldowns(float delta);

    void SendResultToClients();

    void DisplayAnimation(Vector3 position, Vector3 rotation);
}
