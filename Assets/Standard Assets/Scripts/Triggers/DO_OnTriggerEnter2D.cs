using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnTriggerEnter2D : MonoBehaviour {
    public UnityEvent<Collider2D> trigger;

    void OnTriggerEnter2D(Collider2D other) => trigger.Invoke(other);
}

}