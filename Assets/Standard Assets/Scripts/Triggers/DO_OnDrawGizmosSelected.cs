using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnDrawGizmosSelected : MonoBehaviour {
    public UnityEvent trigger;

    void OnDrawGizmosSelected() => trigger.Invoke();
}

}