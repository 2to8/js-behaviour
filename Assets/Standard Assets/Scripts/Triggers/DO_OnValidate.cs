using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnValidate : MonoBehaviour {
    public UnityEvent trigger;

    void OnValidate() => trigger.Invoke();
}

}