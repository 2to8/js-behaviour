using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnBeforeTransformParentChanged : MonoBehaviour {
    public UnityEvent trigger;

    void OnBeforeTransformParentChanged() => trigger.Invoke();
}

}