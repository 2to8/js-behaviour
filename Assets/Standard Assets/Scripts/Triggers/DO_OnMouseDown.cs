using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseDown : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseDown() => trigger.Invoke();
}

}