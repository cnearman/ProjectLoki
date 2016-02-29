using UnityEngine;

public class HideCursorOnKeypress: MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
           Cursor.visible = !Cursor.visible;
        }
    }
}
