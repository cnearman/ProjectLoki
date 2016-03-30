using UnityEngine;
using System.Collections;

public class Spray : MonoBehaviour {

    public float life;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, life);
	}
}
