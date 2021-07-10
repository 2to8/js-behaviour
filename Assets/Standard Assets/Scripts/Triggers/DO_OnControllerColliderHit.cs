using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnControllerColliderHit : MonoBehaviour {
    public UnityEvent<ControllerColliderHit> trigger;

    void OnControllerColliderHit(ControllerColliderHit hit) => trigger.Invoke(hit);
}

}