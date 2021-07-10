using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTransformChildrenChanged : MonoBehaviour {
    public UnityEvent trigger;

    void OnTransformChildrenChanged() => trigger.Invoke();
}

}