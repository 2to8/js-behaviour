using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEngine.Extensions {

public class UnityWebRequestAwaiter : INotifyCompletion {
    UnityWebRequestAsyncOperation asyncOp;
    Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted => asyncOp.isDone;

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
    }
}

public static partial class ExtensionMethods {
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp) =>
        new UnityWebRequestAwaiter(asyncOp);
}

/*
// Usage example:
UnityWebRequest www = new UnityWebRequest();
// ...
await www.SendWebRequest();
Debug.Log(req.downloadHandler.text);
*/

}