using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_LateUpdate : MonoBehaviour {
    public UnityEvent trigger;

    void LateUpdate() => trigger.Invoke();
}

}