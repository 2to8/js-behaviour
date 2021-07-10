using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseOver : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseOver() => trigger.Invoke();
}

}