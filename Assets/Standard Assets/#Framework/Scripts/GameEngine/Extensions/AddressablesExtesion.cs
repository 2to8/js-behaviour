using System;
using UniRx.Async;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameEngine.Extensions {

public static class AddressablesExtesion {

    public static UniTask<T>.Awaiter GetAwaiter<T>(this AsyncOperationHandle<T> operation)
    {
        var tcs = new UniTaskCompletionSource<T>();
        Action<AsyncOperationHandle<T>> eventHandler = null;

        eventHandler = res => {
            // operation.Completed -= eventHandler; // we can't seem to do this!?
            tcs.TrySetResult(res.Result);
        };

        operation.Completed += eventHandler;

        return tcs.Task.GetAwaiter();
    }

    public static UniTask.Awaiter GetAwaiter(this AsyncOperationHandle operation) => operation.ToUniTask().GetAwaiter();

    // bonus .. you can await UnitytEvents
    public static UniTask.Awaiter GetAwaiter(this UnityEvent uevent)
    {
        var tcs = new UniTaskCompletionSource();
        UnityAction eventHandler = null;

        eventHandler = () => {
            uevent.RemoveListener(eventHandler);
            tcs.TrySetResult();
        };

        uevent.AddListener(eventHandler);

        return tcs.Task.GetAwaiter();
    }

    // public static TaskAwaiter<T> GetAwaiter<T>(this AsyncOperationHandle<T> ap)
    // {
    //     var tcs = new TaskCompletionSource<T>();
    //     ap.Completed += op => tcs.TrySetResult(op.Result);
    //     return tcs.Task.GetAwaiter();
    // }

    // public static AsyncOperationAwaiter GetAwaiter(this AsyncOperationHandle operation)
    // {
    //     return new AsyncOperationAwaiter(operation);
    // }
    //
    // public static AsyncOperationAwaiter<T> GetAwaiter<T>(this AsyncOperationHandle<T> operation) {
    //     return new AsyncOperationAwaiter<T>(operation);
    // }
    //
    // public struct AsyncOperationAwaiter : INotifyCompletion
    // {
    //     private readonly AsyncOperationHandle _operation;
    //
    //     public AsyncOperationAwaiter(AsyncOperationHandle operation)
    //     {
    //         _operation = operation;
    //     }
    //
    //     public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;
    //
    //     public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();
    //
    //     public object GetResult() => _operation.Result;
    // }
    //
    // public struct AsyncOperationAwaiter<T> : INotifyCompletion
    // {
    //     private readonly AsyncOperationHandle<T> _operation;
    //
    //     public AsyncOperationAwaiter(AsyncOperationHandle<T> operation)
    //     {
    //         _operation = operation;
    //     }
    //
    //     public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;
    //
    //     public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();
    //
    //     public T GetResult() => _operation.Result;
    // }

}

}