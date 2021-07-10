using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnWillRenderObject : MonoBehaviour {
    public UnityEvent trigger;

    void OnWillRenderObject() => trigger.Invoke();
}

}