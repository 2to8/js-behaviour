using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace UniWebServer {

public class JsonResponse : Dictionary<string, object> {

    public new object this[string index] {
        get => TryGetValue(index, out var ret) ? ret : null;
        set => base[index] = value;
    }

    public static implicit operator string(JsonResponse target) => target.ToJson();
    public string ToJson() => JsonConvert.SerializeObject(this);
    public override string ToString() => ToJson();

}

public class Response {

    public int statusCode = 404;
    public string message = "Not Found";
    public bool useBytes = false;
    public byte[] dataBytes;
    public Headers headers;
    public MemoryStream stream;
    public StreamWriter writer;

    public Response()
    {
        headers = new Headers();

        stream = new MemoryStream();
        writer = new StreamWriter(stream, Encoding.UTF8);
    }

    public void SetBytes(byte[] data)
    {
        useBytes = true;
        dataBytes = data;
    }

    public void WriteJson(string text)
    {
        statusCode = 200;
        message = "OK.";
        headers.Add("Content-type", "application/json; charset=UTF-8");
        Write(text);
    }

    public void Write(string text)
    {
        writer.Write(text);
        writer.Flush();
    }

}

}
