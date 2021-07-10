using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_FixedUpdate : MonoBehaviour {
    public UnityEvent trigger;

    void FixedUpdate() => trigger.Invoke();
}

}