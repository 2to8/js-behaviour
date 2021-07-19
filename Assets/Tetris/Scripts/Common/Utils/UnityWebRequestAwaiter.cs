using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace MainScene.BootScene.Utils
{
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
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
            continuation?.Invoke();
        }
    }
}