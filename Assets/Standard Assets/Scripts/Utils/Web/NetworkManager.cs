using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Utils.Web {

public class NetworkManager : MonoBehaviour
{
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    public void GetPosts2()
    {
        StartCoroutine(GetRequest("/posts", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                Post[] posts = JsonConvert.DeserializeObject<Post[]>(req.downloadHandler.text);

                foreach(Post post in posts)
                {
                    Debug.Log(post.title);
                }
            }
        }));
    }

    // Inside NetworkingManager.cs
    //using UnityEngine.UI

    // ...

    public Image image;

    // ...

    public void DownloadImage(string url)
    {
        StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                // Get the texture out using a helper downloadhandler
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                // Save it into the Image UI's sprite
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }));
    }

    // In NetworkManager.cs

    // ...
    public AudioSource audioSource;

    // ...
    public void DownloadSound(string url)
    {
        StartCoroutine(SoundRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                // Get the sound out using a helper class
                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
                // Load the clip into our audio source and play
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }
        }));


    }

    IEnumerator SoundRequest(string url, Action<UnityWebRequest> callback)
    {
        // Note, we try to download an OGGVORBIS (ogg) file because Windows doesn't support
        // MPEG readily. If you're on a mac, you can try MPEG (mp3)
        using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }

    IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }

    // ... NetworkManager.cs
    public void GetPosts()
    {
        StartCoroutine(GetRequest("https://jsonplaceholder.typicode.com/posts", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                Debug.Log(req.downloadHandler.text);
            }
        }));
    }
}

}