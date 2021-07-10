using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnServerInitialized : MonoBehaviour {
    public UnityEvent trigger;

    void OnServerInitialized() => trigger.Invoke();
}

}