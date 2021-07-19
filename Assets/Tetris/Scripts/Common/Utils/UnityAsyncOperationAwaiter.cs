namespace MainScene.BootScene.Utils
{
    public static class UnityAsyncOperationAwaiter
    {
        /**
     *https://gist.github.com/mattyellen/d63f1f557d08f7254345bff77bfdc8b3
     * Allows the use of async/await (instead of yield) with any Unity AsyncOperation
     * Example:
var getRequest = UnityWebRequest.Get("http://www.google.com");
await getRequest.SendWebRequest();
var result = getRequest.downloadHandler.text;
     */
        // public static TaskAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        // {
        //     var tcs = new TaskCompletionSource<object>();
        //     asyncOp.completed += obj => {
        //         tcs.SetResult(null);
        //     };
        //
        //     return ((Task)tcs.Task).GetAwaiter();
        // }

        // public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        // {
        //     var tcs = new TaskCompletionSource<object>();
        //     asyncOp.completed += obj => {
        //         tcs.SetResult(null);
        //     };
        //
        //     return ((Task)tcs.Task).GetAwaiter();
        // }
    }
}