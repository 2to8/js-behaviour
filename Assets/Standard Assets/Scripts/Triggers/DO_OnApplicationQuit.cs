using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnApplicationQuit : MonoBehaviour {

    public UnityEvent trigger;

    void OnApplicationQuit() => trigger.Invoke();
}

}