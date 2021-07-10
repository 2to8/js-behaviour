using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseDrag : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseDrag() => trigger.Invoke();
}

}