using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.JSAdapter {

public class RemoveMultiAudioListener : MonoBehaviour {

    void Start()
    {
        //Addressables.ClearDependencyCacheAsync;
        var audioListner = GetComponent<AudioListener>();

        if (audioListner != null) {
            GetAllLoadedScenes()
                .ForEach(scene => {
                    if (scene.isLoaded &&
                        scene.GetRootGameObjects()
                            .Any(gameObject =>
                                FindDescendents(gameObject)
                                    .Any(sub => sub.GetComponent<Camera>() != null &&
                                        sub.GetComponent<AudioListener>() != audioListner &&
                                        sub.GetComponent<AudioListener>() != null))) {
                        audioListner.enabled = false;
                    }
                });
        }
    }

    //https://www.what-could-possibly-go-wrong.com/scene-traversal-recipes-for-unity/
    IEnumerable<GameObject> FindDescendents(GameObject parentGameObject)
    {
        foreach (Transform childTransform in parentGameObject.transform) {
            var childGameObject = childTransform.gameObject;

            yield return childGameObject;

            foreach (var descendent in FindDescendents(childGameObject)) {
                yield return childGameObject;
            }
        }
    }

    public static IEnumerable<Scene> GetAllLoadedScenes()
    {
        for (var i = 0; i < SceneManager.sceneCount; i++) {
            yield return SceneManager.GetSceneAt(i);
        }
    }

}

}