﻿using UnityEngine;
using System.Collections.Generic;

public class DefaultProjectile : IProjectile {
    public IEnumerable<Effect> Effects;

    public void ApplyEffects(RaycastHit hit)
    {
        Debug.Log(hit.collider.name);
    }

    public void ApplyEffects(RaycastHit hit, Vector3 direction)
    {
        Debug.Log(hit.collider.name);

        if(hit.collider.GetComponent<Player>())
        {
            if(hit.collider.GetComponent<PhotonView>().isMine)
            {
                GameObject gm = GameObject.Find("Connect_Test");
                gm.GetComponent<GameManager>().ShowDamageMarkers(direction);
            }
        }
    }
}
