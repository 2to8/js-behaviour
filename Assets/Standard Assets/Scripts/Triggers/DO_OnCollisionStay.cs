using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionStay : MonoBehaviour {
    public UnityEvent<Collision> trigger;

    void OnCollisionStay(Collision other) => trigger.Invoke(other);
}

}