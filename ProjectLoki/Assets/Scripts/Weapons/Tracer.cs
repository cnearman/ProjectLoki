using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

    public float life;
    public Vector3 offset; //this is cosmetic, you cant see the tracer at the moment bc its center cam
    public bool traceEffect; //this gives the illution of motion. looks better but can also be misleading

    //trace effect stuff
    Vector3 increase;
    Vector3 start;
    Vector3 end;
    float length;
    float increaseAmount;
    public float speed;

    // Use this for initialization
    void Start () {
        if (!traceEffect)
        {
            Destroy(gameObject, life);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(traceEffect)
        {
            increaseAmount += Time.deltaTime * speed;
            if (increaseAmount >= length)
            {
                //Debug.Log("pop");
                Destroy(gameObject);
            }
            increase += (end - start).normalized * Time.deltaTime * speed;
            GetComponent<LineRenderer>().SetPosition(0, start + increase);
        }
	}

    public void SetLocation(Vector3 startLoc, Vector3 endLoc)
    {
        GetComponent<LineRenderer>().SetPosition(0, startLoc + offset);
        GetComponent<LineRenderer>().SetPosition(1, endLoc);

        if (traceEffect)
        {
            start = startLoc + offset;
            end = endLoc;
            Vector3 temp = end - start;
            length = temp.magnitude;
        }
    }
}
