using UnityEngine;

namespace GameEngine.ViewModel {

public class UseConfigObject : MonoBehaviour {

    void OnGUI()
    {
        if (ConfigObject.configInstance != null) {
            GUILayout.Label("Found the config asset. The message was: " + ConfigObject.configInstance.text);
        }
    }

}

}