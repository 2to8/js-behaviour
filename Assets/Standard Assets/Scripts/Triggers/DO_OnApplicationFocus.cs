using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnApplicationFocus : MonoBehaviour {

    public UnityEvent<bool> trigger;

    void OnApplicationFocus(bool hasFocus) => trigger.Invoke(hasFocus);
}

}