using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseEnter : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseEnter() => trigger.Invoke();
}

}