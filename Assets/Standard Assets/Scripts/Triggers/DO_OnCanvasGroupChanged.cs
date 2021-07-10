using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCanvasGroupChanged : MonoBehaviour {
    public UnityEvent trigger;

    void OnCanvasGroupChanged() => trigger.Invoke();
}

}