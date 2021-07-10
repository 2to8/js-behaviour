using UnityEngine;

namespace IngameDebugConsole {

public class ChangeResolution : MonoBehaviour {

    public Vector2Int min = new Vector2Int(540, 960);
    public Vector2Int max = new Vector2Int(1600, 900);

    public void Change()
    {
        if (Screen.width > min.x) {
            Screen.SetResolution(min.x, min.y, false);
        } else {
            Screen.SetResolution(max.x, max.y, false);
        }
    }

}

}