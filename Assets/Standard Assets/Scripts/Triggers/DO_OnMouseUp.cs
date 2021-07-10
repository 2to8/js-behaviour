using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseUp : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseUp() => trigger.Invoke();
}

}