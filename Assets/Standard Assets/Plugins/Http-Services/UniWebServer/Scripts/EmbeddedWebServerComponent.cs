using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UniWebServer {

public class EmbeddedWebServerComponent : MonoBehaviour {

    public bool startOnAwake = true;
    public int port = 8079;
    public int workerThreads = 2;
    public bool processRequestsInMainThread = true;
    public bool logRequests = true;
    WebServer server;
    Dictionary<string, IWebResource> resources = new Dictionary<string, IWebResource>();

    void Start()
    {
        if(processRequestsInMainThread) Application.runInBackground = true;
        server = new WebServer(port, workerThreads, processRequestsInMainThread);
        server.logRequests = logRequests;
        server.HandleRequest += HandleRequest;
        if(startOnAwake) {
            server.Start();
        }
    }

    void OnApplicationQuit()
    {
        server.Dispose();
    }

    void Update()
    {
        if(server.processRequestsInMainThread) {
            server.ProcessRequests();
        }
    }

    void HandleRequest(Request request, Response response)
    {
        // get first part of the directory
        string folderRoot = Helper.GetFolderRoot(request.uri.LocalPath);
        var path = folderRoot;

        var check = resources.ContainsKey(path);
        if(!check) {
            var tmp = Regex.Replace(request.uri.LocalPath, @"^/*(.*)$", "$1").Split('/').FirstOrDefault();
            check = resources.ContainsKey(path = request.uri.LocalPath) ||
                resources.ContainsKey(path = $"/{tmp}") || resources.ContainsKey(path = $"{tmp}");
        }
        if(check) {
            try {
                resources[path].HandleRequest(request, response);
            } catch(Exception e) {
                response.statusCode = 500;
                response.Write(e.ToString());
            }
        } else {
            response.statusCode = 404;
            response.message = "Not Found.";
            var paths = string.Join("\n", resources.Keys);
            response.Write(request.uri.LocalPath + $" (Root: {folderRoot}) not found.\n{paths}");
        }
    }

    public void AddResource(string path, IWebResource resource)
    {
        resources[path] = resource;
    }

}

}
