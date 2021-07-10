using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnDrawGizmos : MonoBehaviour {
    public UnityEvent trigger;

    void OnDrawGizmos() => trigger.Invoke();
}

}