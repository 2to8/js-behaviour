using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTriggerExit2D : MonoBehaviour {
    public UnityEvent<Collider2D> trigger;

    void OnTriggerExit2D(Collider2D other) => trigger.Invoke(other);
}

}