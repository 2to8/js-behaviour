using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnApplicationPause : MonoBehaviour {
    public UnityEvent<bool> trigger;

    void OnApplicationPause(bool pauseStatus) => trigger.Invoke(pauseStatus);
}

}