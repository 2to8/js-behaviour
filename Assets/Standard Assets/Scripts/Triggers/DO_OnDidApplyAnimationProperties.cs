using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnDidApplyAnimationProperties : MonoBehaviour {
    public UnityEvent trigger;

    void OnDidApplyAnimationProperties() => trigger.Invoke();
}

}