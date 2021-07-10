using Newtonsoft.Json;
using UnityEngine;
using UniWebServer;
using Request = UniWebServer.Request;

namespace GameEngine.ViewModel {

[RequireComponent(typeof(EmbeddedWebServerComponent))]
public class HttpTest : MonoBehaviour, IWebResource {

    public string path = "/test";
    public TextAsset html;
    public EmbeddedWebServerComponent server;
    public static HttpTest Instance;

    void Awake()
    {
        Instance = this;
        server = GetComponent<EmbeddedWebServerComponent>();
        server.AddResource(path, this);
    }

    void Start() { }

    public void HandleRequest(Request request, Response response)
    {
        response.statusCode = 200;
        response.message = "OK.";
        response.headers.Add("Content-type", "application/json; charset=UTF-8");

        response.Write(html != null ? JsonConvert.SerializeObject(html) : "hello, world");
    }

}

}