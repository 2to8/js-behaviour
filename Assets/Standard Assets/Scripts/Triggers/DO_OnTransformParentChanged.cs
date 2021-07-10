using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTransformParentChanged : MonoBehaviour {
    public UnityEvent trigger;

    void OnTransformParentChanged() => trigger.Invoke();
}

}