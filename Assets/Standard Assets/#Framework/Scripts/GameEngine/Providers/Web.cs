// using System;
// using JetBrains.Annotations;
// using UnityEngine;
//
// namespace Engine.Providers {
//
// public class Web {
//
//     [ CanBeNull ]
//     public static T Rpc<T>(Action<T> callback = null) where T : IWebRpc<T>, new()
//     {
//         var obj = new T();
//         callback?.Invoke(obj);
//
//         return obj;
//     }
//
//     [ CanBeNull ]
//     public static T Rpc<T>(Func<T, bool> callback = null) where T : IWebRpc<T>, new()
//     {
//         var obj = new T();
//         var ret = callback?.Invoke(obj);
//
//         if( ret == false ) {
//             obj.IsFailed = true;
//         } else {
//             obj.IsCompleted = true;
//         }
//
//         return obj;
//     }
//
//     public abstract class IWebRpc<T>:object where T : IWebRpc<T> {
//
//         public bool IsCompleted;
//         public bool IsFailed;
//         public virtual void Finish(T res) { }
//         public virtual void Failed() { }
//
//     }
//
//     public class User {
//
//         public class GetServerTime : IWebRpc<GetServerTime> {
//
//             public static int request_timestamp;
//             public static float request_time;
//
//             // 实体参数
//             public int server_timestamp;
//
//             public override void Finish(GetServerTime result)
//             {
//                 if( request_timestamp == 0 ) {
//                     request_timestamp = server_timestamp;
//                     request_time = Time.time;
//                 }
//             }
//
//             public override void Failed() { }
//
//             public static long TimeStamp()
//             {
//                 var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
//                 var timeStamp = (long)(DateTime.Now - startTime).TotalSeconds;                  // 相差秒数
//
//                 return timeStamp;
//             }
//
//             public static DateTime fromTimeStamp(int seconds = 0)
//             {
//                 long unixTimeStamp = 1478162177;
//                 var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
//                 var dt = startTime.AddSeconds(unixTimeStamp + seconds);
//
//                 return dt;
//             }
//
//             public void TestRun()
//             {
//                 Rpc<GetServerTime>(t => { });
//             }
//
//         }
//
//     }
//
// }
//
// }

