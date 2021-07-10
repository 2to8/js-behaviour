using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseUpAsButton : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseUpAsButton() => trigger.Invoke();
}

}