using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnConnectedToServer : MonoBehaviour {
    public UnityEvent trigger;

    void OnConnectedToServer() => trigger.Invoke();
}

}