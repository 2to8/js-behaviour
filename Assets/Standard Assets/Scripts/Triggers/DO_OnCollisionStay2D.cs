using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionStay2D : MonoBehaviour {
    public UnityEvent<Collision2D> trigger;

    void OnCollisionStay2D(Collision2D other) => trigger.Invoke(other);
}

}