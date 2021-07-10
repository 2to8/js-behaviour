using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

// public class TT {

public class DO_TriggerStay2D : MonoBehaviour {
    public UnityEvent<Collider2D> trigger;

    void OnTriggerStay2D(Collider2D other) => trigger.Invoke(other);
}

// }

}