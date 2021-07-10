// using Newtonsoft.Json;
// using UnityEngine;
// using UniWebServer;
//
// namespace GameCore.Game {
//
// [ RequireComponent(typeof(EmbeddedWebServerComponent)) ]
// public class HttpTest : MonoBehaviour, IWebResource {
//
//     public string path = "/upload";
//     public TextAsset html;
//     EmbeddedWebServerComponent server;
//
//     void Start()
//     {
//         server = GetComponent<EmbeddedWebServerComponent>();
//         server.AddResource(path, this);
//     }
//
//     public void HandleRequest(Request request, Response response)
//     {
//         response.statusCode = 200;
//         response.message = "OK.";
//         response.headers.Add("Content-type", "application/json; charset=UTF-8");
//
//         response.Write(JsonConvert.SerializeObject(html));
//     }
//
// }
//
// }

