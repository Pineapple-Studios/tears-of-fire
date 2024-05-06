using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInvisible : MonoBehaviour
{
    public static CursorInvisible Instance;

    public void InvisibleCursor()
    {
        Cursor.visible = false;
    }
}
