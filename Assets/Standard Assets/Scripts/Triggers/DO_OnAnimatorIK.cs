using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnAnimatorIK : MonoBehaviour {
    public UnityEvent<int> trigger;

    void OnAnimatorIK(int layerIndex) => trigger.Invoke(layerIndex);
}

}