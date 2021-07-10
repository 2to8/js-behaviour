using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnJointBreak : MonoBehaviour {
    public UnityEvent<float> trigger;

    void OnJointBreak(float breakForce) => trigger.Invoke(breakForce);
}

}