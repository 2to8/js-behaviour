using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTriggerExit : MonoBehaviour {
    public UnityEvent<Collider> trigger;

    void OnTriggerExit(Collider other) => trigger.Invoke(other);
}

}