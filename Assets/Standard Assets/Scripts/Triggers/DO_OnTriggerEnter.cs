using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTriggerEnter : MonoBehaviour {
    public UnityEvent<Collider> trigger;

    void OnTriggerEnter(Collider other) => trigger.Invoke(other);
}

}