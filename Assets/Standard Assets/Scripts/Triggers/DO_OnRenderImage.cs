using UnityEngine;
using UnityEngine.Events;

namespace Triggers {

public class DO_OnRenderImage : MonoBehaviour {
    public UnityEvent<RenderTexture, RenderTexture> trigger;

    void OnRenderImage(RenderTexture src, RenderTexture dest) => trigger.Invoke(src, dest);
}

}