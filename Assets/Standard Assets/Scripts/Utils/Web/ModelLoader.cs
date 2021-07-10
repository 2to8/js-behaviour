using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// using Siccity.GLTFUtility;

namespace Utils.Web {

/// <summary>
/// https://theslidefactory.com/loading-3d-models-from-the-web-at-runtime-in-unity/
///“Learn how loading 3D models (GLTF and GLB)
/// from a web server works and then load them into Unity at runtime with local file caching.”
/// </summary>
public class ModelLoader : MonoBehaviour {

    GameObject wrapper;
    string filePath;

    private void Start()
    {
        filePath = $"{Application.persistentDataPath}/Files/";
        wrapper = new GameObject {
            name = "Model"
        };
    }

    public void DownloadFile(string url)
    {
        string path = GetFilePath(url);

        if (File.Exists(path)) {
            Debug.Log("Found file locally, loading...");
            LoadModel(path);

            return;
        }

        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) => {
            if (req.isNetworkError || req.isHttpError) {
                // Log any errors that may happen
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            } else {
                // Save the model into a new wrapper
                LoadModel(path);
            }
        }));
    }

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
    }

    void LoadModel(string path)
    {
        ResetWrapper();

        // GameObject model = Importer.LoadFromFile(path);
        // model.transform.SetParent(wrapper.transform);
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url)) {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));

            yield return req.SendWebRequest();

            callback(req);
        }
    }

    void ResetWrapper()
    {
        if (wrapper != null) {
            foreach (Transform trans in wrapper.transform) {
                Destroy(trans.gameObject);
            }
        }
    }

}

}