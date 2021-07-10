using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GameEngine.Utils {

/// <summary>
///     https://blog.csdn.net/weixin_43149049/article/details/88854014
/// </summary>
public class NetWorkUtil : MonoBehaviour {

    static NetWorkUtil instance;

    public Action<UnityWebRequest, Action<UnityWebRequest>, string> commonUWRBack = (uwr, deal, extraLogStr) => {
        if (!ReportUWRException(uwr, extraLogStr)) {
            //Debug.LogError(" uwr.responseCode " + uwr.responseCode);
            if (uwr.responseCode == 200) //200表示接受成功
            {
                //Debug.LogError("commonUWRBack text " + uwr.downloadHandler.text);
                deal?.Invoke(uwr);
            }
        }
    };

    public static NetWorkUtil Instance {
        get {
            if (instance == null) {
                var mounter = new GameObject("C_UnityWebRequest");
                instance = mounter.AddComponent<NetWorkUtil>();
            }

            return instance;
        }
    }

    public static bool DtctInvalidUri(string uri)
    {
        if (uri == string.Empty) {
            Debug.LogError("NetWorkUtil DtctInvalidUri Invalid url " + uri);

            return true;
        }

        return false;
    }

    static bool ReportUWRException(UnityWebRequest uwr, string extraLogStr = null)
    {
        if (!string.IsNullOrEmpty(uwr.error) || uwr.isNetworkError || uwr.isHttpError) {
            Debug.LogError("commonUWRBack error " + uwr.error);
            Debug.LogError("commonUWRBack url " + uwr.url);

            if (uwr.downloadHandler != null && uwr.downloadHandler.text != null) {
                Debug.LogError( /*Util.ChargeUnicodeStr( */uwr.downloadHandler.text /*)*/);
            }

            Debug.LogError("uwr.isNetworkError " + uwr.isNetworkError);
            Debug.LogError("uwr.isHttpError " + uwr.isHttpError);
            Debug.LogError("uwr.responseCode " + uwr.responseCode);

            if (extraLogStr != null) {
                Debug.LogError(extraLogStr);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///     GET请求 url是直接拼好的地址  参数都在url里面那种
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">请求发起后处理回调结果的委托,处理请求对象</param>
    public void Get(string url, Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null,
        Action<UnityWebRequest> a = null, Hashtable headerTbl = null, string extraLogStr = null)
    {
        //通过url访问的目前除了这个方法还有个Post2
        //Debug.LogError(" NetWorkUtil Get url " + url);
        if (actionResult == null) {
            actionResult = Instance.commonUWRBack;
        }

        StartCoroutine(_Get(url, actionResult, a, headerTbl, extraLogStr));
    }

    /// <summary>
    ///     下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }

    /// <summary>
    ///     请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<Texture2D> actionResult)
    {
        if (url == /*Const.emptyString*/string.Empty) {
            Debug.LogError(" NetWorkUtil GetTexture emptyString");
        }

        StartCoroutine(_GetTexture(url, actionResult));
    }

    /// <summary>
    ///     请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Func<Texture2D, Image> actionResult = null)
    {
        StartCoroutine(_GetTexture(url, null, actionResult));
    }

    /// <summary>
    ///     请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<AssetBundle> actionResult = null)
    {
        if (DtctInvalidUri(url)) {
            return;
        }

        StartCoroutine(_GetAssetBundle2(url, actionResult));
    }

    /// <summary>
    ///     等待的方式请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public IEnumerator GetAssetBundle2(string url, Action<AssetBundle> actionResult = null,
        Action<UnityWebRequest> duringCB = null)
    {
        if (DtctInvalidUri(url)) {
            yield break;
        }

        yield return StartCoroutine(_GetAssetBundle2(url, actionResult, duringCB));
    }

    /// <summary>
    ///     请求服务器地址上的音效
    /// </summary>
    /// <param name="url">有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<AudioClip> actionResult = null, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    /// <summary>
    ///     向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form表单参数</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void Post(string serverURL, WWWForm wwwform,
        Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null, Action<UnityWebRequest> a = null,
        Hashtable headerTable = null, string extraLogStr = null)
    {
        StartCoroutine(_Post(serverURL, wwwform, actionResult, a, headerTable));
    }

    /// <summary>
    ///     向服务器提交post请求 返回协程的方式
    /// </summary>
    /// <returns></returns>
    public IEnumerator Post2(string serverURL, WWWForm wwwform,
        Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null, Action<UnityWebRequest> a = null,
        Hashtable headerTable = null, string extraLogStr = null)
    {
        yield return StartCoroutine(_Post(serverURL, wwwform, actionResult, a, headerTable));
    }

    /// <summary>
    ///     向服务器提交post请求 目前传json给服务器使用这种方式
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址</param>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托,处理请求对象</param>
    public void UploadJson(string serverURL, string jsonStr,
        Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null, Action<UnityWebRequest> a = null,
        Hashtable headerTable = null, string extraLogStr = null)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        //Debug.LogError("  NetWorkUtil Post2 serverURL " + serverURL);
        var bodyRaw = Encoding.UTF8.GetBytes(jsonStr);
        StartCoroutine(_UpLoadByPost(serverURL, bodyRaw, actionResult, a, headerTable, extraLogStr));
    }

    /// <summary>
    ///     通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="contentType">请求头 类型设置</param>
    public void UploadByPut(string url, byte[] contentBytes, Action<UnityWebRequest> actionResult = null,
        Hashtable headerParams = null)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, headerParams));
    }

    /// <summary>
    ///     通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="contentType">设置header文件中的Content-Type属性</param>
    /// <param name="headerParams">请求头传的参数列表</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<UnityWebRequest> actionResult = null,
        Hashtable headerParams = null)
    {
        using (var uwr = UnityWebRequest.Put(url, contentBytes)) {
            if (headerParams != null) {
                foreach (string key in headerParams.Keys) {
                    uwr.SetRequestHeader(key, headerParams[key].ToString());
                }
            }

            uwr.SetRequestHeader("extName", "jpeg");

            yield return uwr.SendWebRequest();

            if (!ReportUWRException(uwr)) {
                actionResult(uwr);
            }
        }
    }

    /// <summary>
    ///     发送头文件给服务器
    /// </summary>
    /// <param name="uri"></param>
    public void SendUWRHead(string uri, Action<UnityWebRequest> actionResult = null)
    {
        StartCoroutine(_SendUWRHead(uri, actionResult));
    }

    /// <summary>
    ///     等待方式发送头文件给服务器
    /// </summary>
    /// <param name="url"></param>
    /// <param name="actionResult"></param>
    /// <returns></returns>
    public IEnumerator SendUWRHead2(string url, Action<UnityWebRequest> actionResult = null)
    {
        if (DtctInvalidUri(url)) {
            yield break;
        }

        yield return StartCoroutine(_SendUWRHead(url, actionResult));
    }

    static void TraverseHeaderTbl(ref UnityWebRequest uwr, Hashtable headerTbl)
    {
        if (headerTbl != null) {
            foreach (string key in headerTbl.Keys) {
                //Debug.LogError("NetWorkUtil TraverseHeaderTbl key " + key + "  headerTbl[key].ToString()  " + headerTbl[key].ToString());
                if (headerTbl[key] != null) {
                    uwr.SetRequestHeader(key, headerTbl[key].ToString());
                } else {
                    //一般服务端不强求传的参数 需要传null的情况下可以不在请求头设置这些参数
                    //但是如果像这种语句 uwr.SetRequestHeader(key, null)设置了参数为null则会报错
                    //所以遍历hashtable 的时候 如果value为null则 会设置为字符串
                    uwr.SetRequestHeader(key, "");
                }
            }
        }
    }

    /// <summary>
    ///     GET请求
    /// </summary>
    /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="a">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null,
        Action<UnityWebRequest> a = null, Hashtable headerTbl = null, string extraLogStr = null)
    {
        var uwr = UnityWebRequest.Get(url);

        //Debug.LogError("NetWorkUtil _Get url " + url);
        TraverseHeaderTbl(ref uwr, headerTbl);

        //Debug.LogError(uwr);
        yield return uwr.SendWebRequest();

        //if (url == Const.mainGoJsonServerUrl) {
        //	Debug.LogError("Get Back " + url);
        //}
        if (actionResult != null) {
            actionResult(uwr, a, extraLogStr);
        }
    }

    /// <summary>
    ///     下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);

        yield return uwr.SendWebRequest();

        if (actionResult != null) {
            actionResult(uwr);
        }
    }

    /// <summary>
    ///     请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    IEnumerator _GetTexture(string url, Action<Texture2D> actionResult = null, Func<Texture2D, Image> funcResult = null)
    {
        var uwr = new UnityWebRequest(url);
        var downloadTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = downloadTexture;

        yield return uwr.SendWebRequest();

        Texture2D t = null;

        if (!ReportUWRException(uwr)) {
            t = downloadTexture.texture;

            if (actionResult != null) {
                actionResult(t);
            }
        }
    }

    /// <summary>
    ///     请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    IEnumerator _GetAssetBundle2(string url, Action<AssetBundle> actionResult = null,
        Action<UnityWebRequest> duringCB = null)
    {
        //Debug.LogError("_GetAssetBundle  url  " + url);
        AssetBundle ab = null;
        UnityWebRequest request;
        request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        //NetWorkMidFrame.GetABDownloadPrg(request);

        yield return request.SendWebRequest();

        ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        yield return null;

        if (!ReportUWRException(request)) {
            //Debug.LogError("NetWorkUtil _GetAssetBundle2 Size " + request.downloadedBytes);
            //MsgUtil.CallFunc(MsgEvents.calDownLoadFileSize, request);
            if (actionResult != null) {
                actionResult(ab);
            }
        }
    }

    /// <summary>
    ///     请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult = null, AudioType audioType = AudioType.WAV)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType)) {
            yield return uwr.SendWebRequest();

            if (!ReportUWRException(uwr)) {
                if (actionResult != null) {
                    actionResult(DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }
    }

    /// <summary>
    ///     向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form表单参数</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="header">请求头</param>
    /// <param name="a">处理返回结果的委托</param>
    /// <returns></returns>
    IEnumerator _Post(string serverURL, WWWForm wwwform,
        Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null, Action<UnityWebRequest> a = null,
        Hashtable headerTbl = null, string extraLogStr = null)
    {
        var uwr = UnityWebRequest.Post(serverURL, wwwform);

        //Debug.LogError("NetWorkUtil _Post " + serverURL);

        //UnityWebRequest uwr = UnityWebRequest.Post(serverURL, );
        TraverseHeaderTbl(ref uwr, headerTbl);

        //uwr.SetRequestHeader(Const.contenTypeName, Const.contentTypeWWWForm);

        yield return uwr.SendWebRequest();

        if (actionResult != null) {
            actionResult(uwr, a, extraLogStr);
        }
    }

    /// <summary>
    ///     第二种方式向服务器提交post请求 目前测试只有这种方式提交json成功 注意参数
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托</param>
    /// <param name="header">请求头</param>
    /// <returns></returns>
    IEnumerator _UpLoadByPost(string serverURL, byte[] bodyRaw,
        Action<UnityWebRequest, Action<UnityWebRequest>, string> actionResult = null, Action<UnityWebRequest> a = null,
        Hashtable headerTbl = null, string extraLogStr = null)
    {
        var request = new UnityWebRequest(serverURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        TraverseHeaderTbl(ref request, headerTbl);

        yield return request.SendWebRequest();

        if (actionResult != null) {
            actionResult(request, a, extraLogStr);
        }
    }

    /// <summary>
    ///     请求头 不下载 看大小
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="cb"></param>
    /// <returns></returns>
    IEnumerator _SendUWRHead(string uri, Action<UnityWebRequest> cb = null)
    {
        using (var uwr = UnityWebRequest.Head(uri)) {
            yield return uwr.SendWebRequest();

            if (!ReportUWRException(uwr)) {
                cb(uwr);
            }
        }
    }

    public static void LogUwrAllResponseHeader(UnityWebRequest uwr)
    {
        var dic = uwr.GetResponseHeaders();

        foreach (var key in dic.Keys) {
            Debug.LogError("LoadFromNetMgr LogUwrAllResponseHeader key " + key + " dic[key]  " + dic[key]);
        }
    }

}

}