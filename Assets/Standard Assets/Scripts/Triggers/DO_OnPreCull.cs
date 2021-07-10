using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnPreCull : MonoBehaviour {

    public UnityEvent trigger;

    void OnPreCull() => trigger.Invoke();
}

}