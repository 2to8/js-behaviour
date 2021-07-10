using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnMouseExit : MonoBehaviour {
    public UnityEvent trigger;

    void OnMouseExit() => trigger.Invoke();
}

}