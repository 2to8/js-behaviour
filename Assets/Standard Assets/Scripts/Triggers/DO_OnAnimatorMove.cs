using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnAnimatorMove : MonoBehaviour {
    public UnityEvent trigger;

    void OnAnimatorMove() => trigger.Invoke();

}

}