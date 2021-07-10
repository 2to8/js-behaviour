using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnJointBreak2D : MonoBehaviour {
    public UnityEvent<Joint2D> trigger;

    void OnJointBreak2D(Joint2D brokenJoint) => trigger.Invoke(brokenJoint);
}

}