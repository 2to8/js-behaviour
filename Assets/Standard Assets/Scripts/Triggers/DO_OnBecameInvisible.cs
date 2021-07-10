using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnBecameInvisible : MonoBehaviour {
    public UnityEvent trigger;

    void OnBecameInvisible() => trigger.Invoke();
}

}