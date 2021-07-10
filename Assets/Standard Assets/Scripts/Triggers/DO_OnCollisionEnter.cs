using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionEnter : MonoBehaviour {
    public UnityEvent<Collision> trigger;

    void OnCollisionEnter(Collision other) => trigger.Invoke(other);

}

}