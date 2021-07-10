using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnRenderObject : MonoBehaviour {
    public UnityEvent trigger;

    void OnRenderObject() => trigger.Invoke();
}

}