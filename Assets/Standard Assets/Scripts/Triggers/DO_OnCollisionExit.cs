using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCollisionExit : MonoBehaviour {
    public UnityEvent<Collision> trigger;

    void OnCollisionExit(Collision other) => trigger.Invoke(other);

}

}