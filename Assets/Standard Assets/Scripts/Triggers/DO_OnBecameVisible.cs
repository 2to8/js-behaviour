using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnBecameVisible : MonoBehaviour {
    public UnityEvent trigger;

    void OnBecameVisible() => trigger.Invoke();
}

}