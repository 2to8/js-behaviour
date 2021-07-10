using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTriggerStay : MonoBehaviour {
    public UnityEvent<Collider> trigger;

    void OnTriggerStay(Collider other) => trigger.Invoke(other);
}

}