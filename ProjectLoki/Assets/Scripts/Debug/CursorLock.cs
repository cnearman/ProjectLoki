using UnityEngine;
using System.Collections;

public class CursorLock : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Backslash))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
	}
}
