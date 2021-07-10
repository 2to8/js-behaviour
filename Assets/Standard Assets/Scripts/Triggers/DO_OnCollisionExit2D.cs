using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionExit2D : MonoBehaviour {
    public UnityEvent<Collision2D> trigger;

    void OnCollisionExit2D(Collision2D other) => trigger.Invoke(other);

}

}