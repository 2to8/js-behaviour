using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Tests {



public class ConnectBaidu {



    static IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            callback(request);
        }
    }

    [MenuItem("Tests/Baidu")]
    static void baidu()
    {
        GetPosts("https://baidu.com/robots.txt");
    }

    // ... NetworkManager.cs
    public static void GetPosts(string url)
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(GetRequest(url, req => {
            if (req.isNetworkError || req.isHttpError) {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else {
                Debug.Log(req.downloadHandler.text);
            }
        }));
    }

}

}