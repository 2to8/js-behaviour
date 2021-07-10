using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionEnter2D : MonoBehaviour {
    public UnityEvent<Collision2D> trigger;

    void OnCollisionEnter2D(Collision2D other) => trigger.Invoke(other);
}

}