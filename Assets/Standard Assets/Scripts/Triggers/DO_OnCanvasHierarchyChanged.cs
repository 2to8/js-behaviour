using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnCanvasHierarchyChanged : MonoBehaviour {
    public UnityEvent trigger;

    void OnCanvasHierarchyChanged() => trigger.Invoke();
}

}