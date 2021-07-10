using Database;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.Utilities;
using SqlCipher4Unity3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using System.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
#endif
using UnityEngine;
using UnityEngine.Assertions;

// using example;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
namespace GameEngine.Utils {

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

// [ScriptedImporter( 1, "sqlite" )]
// public class SrtImporter : ScriptedImporter {
//     public override void OnImportAsset( AssetImportContext ctx ) {
//         TextAsset subAsset = new TextAsset( File.ReadAllText( ctx.assetPath ) );
//         ctx.AddObjectToAsset( "text", subAsset );
//         ctx.SetMainObject( subAsset );
//     }
// }
#endif

public class DataService {

    readonly SQLiteConnection _connection;
    public string dbPath;
    public bool encrypt = true;
    static bool showed;

    public DataService(ref TextAsset asset, string DatabaseName, string password = null)
    {
    #if UNITY_EDITOR

        // dbPath = AssetDatabase.FindAssets($"t:TextAsset {DatabaseName}")
        //     .Select(AssetDatabase.GUIDToAssetPath)
        //     .FirstOrDefault();
        //
        // Debug.Log(dbPath);
        //
        // Assert.IsNull(dbPath);

        dbPath = AssetDatabase.GetAssetPath(asset);

        // string.Format(@"Assets/Settings/{0}", DatabaseName);

        // string.Format(@"Assets/Resources/{0}", DatabaseName);
    #endif

        // check if file exists in Application.persistentDataPath
        var filepath = $"{Application.persistentDataPath}/{DatabaseName.md5()}.bytes";

        if (!File.Exists(filepath)) {
            Debug.Log("Database not in Persistent path");

        #region #old way

            // // if it doesn't ->
            // // open StreamingAssets directory and load the knex_db ->
            //
            // // #if UNITY_ANDROID
            // //                 WWW loadDb =
            // //      new WWW ("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName); // this is the path to your StreamingAssets in android
            // //                 while (!loadDb.isDone) { } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // //                 // then save to Application.persistentDataPath
            // //                 File.WriteAllBytes (filepath, loadDb.bytes);
            // // #elif UNITY_IOS
            // //                 string loadDb =
            // //      Application.dataPath + "/Raw/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            // //                 // then save to Application.persistentDataPath
            // //                 File.Copy (loadDb, filepath);
            // // #elif UNITY_WP8
            // //                 string loadDb =
            // //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            // //                 // then save to Application.persistentDataPath
            // //                 File.Copy (loadDb, filepath);
            // //
            // // #elif UNITY_WINRT
            // //                 string loadDb =
            // //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            // //                 // then save to Application.persistentDataPath
            // //                 File.Copy (loadDb, filepath);
            // // #elif UNITY_STANDALONE_OSX
            // //                 string loadDb =
            // //      Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            // //                 // then save to Application.persistentDataPath
            // //                 File.Copy(loadDb, filepath);
            // // #else
            // //                 string loadDb =
            // //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            // //                 // then save to Application.persistentDataPath
            // //                 File.Copy(loadDb, filepath);
            // // #endif
            // var res = Resources.Load<TextAsset>(DatabaseName.Substring(0,
            //     DatabaseName.LastIndexOf(".", StringComparison.Ordinal)));
            //
            // if (res == null) {
            //     res = Resources.Load<TextAsset>(DatabaseName.Substring(0,
            //         DatabaseName.IndexOf(".", StringComparison.Ordinal)));
            // }
            //
            // if (res != null) {

        #endregion

            File.WriteAllBytes(filepath, asset.bytes);

            //}
            Debug.Log("Database written");
        }

        //var asset = new TextAsset(File.ReadAllText(filepath));

        if (asset != null && !Application.isEditor && File.ReadAllText(filepath) != asset.text) {
            File.WriteAllBytes(filepath, asset.bytes);
        }

        if (!Application.isEditor || asset == null) {
            dbPath = filepath;
        }

        var versionFile = Application.persistentDataPath + "/version.dat";

        if (!File.Exists(versionFile)) {
            // if (File.Exists(dbPath)) {
            //     File.Delete(dbPath);
            // }
            var timestamp = Core.TimeStamp().ToString();
            File.WriteAllText(versionFile, timestamp);
            Debug.Log($"recreate version file: {timestamp}");
        }

        //if (encrypt) {
        var t1 = new byte[][] {
            new byte[] { 0x40, 0xBD, 0xAD, 0x33 },
            new byte[] { 0x44, 0x70 },
            new byte[] { 0x46, 0x66 },
            new byte[] { 0xa5, 0xf0 },
            new byte[] {
                0x53, 0x80, 0xed, 0x41, 0xf3, 0x3f,
            },
        };
        var pwd = string.Join("-", t1.Select(t => ByteArrayToString(t)));
    #if UNITY_EDITOR
        Assert.IsTrue(pwd == "40BDAD33-4470-4666-A5F0-5380ED41F33F");
    #endif
        var pass = encrypt ? string.IsNullOrEmpty(password) ? pwd : (password + pwd.md5()).md5() : null;

        try {
            _connection = new DbConnection(dbPath, Application.isEditor || Debug.isDebugBuild ? null : pass);
        } catch (Exception e) {
            Debug.Log(e.Message);

            if (Application.isEditor && asset != null) {
            #if UNITY_EDITOR
                File.Delete(dbPath);
                AssetDatabase.CreateAsset(new TextAsset(), dbPath);
                AssetDatabase.SaveAssets();
                asset = AssetDatabase.LoadAssetAtPath<TextAsset>(dbPath);
            #endif
            } else {
                File.Delete(dbPath);
                File.WriteAllBytes(dbPath, (asset ?? new TextAsset()).bytes);
            }
            _connection = new DbConnection(dbPath, Application.isEditor || Debug.isDebugBuild ? null : pass);
        }

        // } else {
        //     _connection = new DbConnection(dbPath);
        // }

        //conn = _connection;
        if (!showed) {
            Debug.Log("Final PATH: " + dbPath);
            showed = true;
        }
    }

    public SQLiteConnection Connection() => _connection;

    // public void CreateDB() {
    //     _connection.DropTable<Person>();
    //     _connection.CreateTable<Person>();
    //
    //     _connection.InsertAll(new[] {
    //             new Person {
    //                 Id = 1,
    //                 Name = "Tom",
    //                 Surname = "Perez",
    //                 Age = 56
    //             },
    //             new Person {
    //                 Id = 2,
    //                 Name = "Fred",
    //                 Surname = "Arthurson",
    //                 Age = 16
    //             },
    //             new Person {
    //                 Id = 3,
    //                 Name = "John",
    //                 Surname = "Doe",
    //                 Age = 25
    //             },
    //             new Person {
    //                 Id = 4,
    //                 Name = "Roberto",
    //                 Surname = "Huertas",
    //                 Age = 37
    //             }
    //         }
    //     );
    // }

    // public IEnumerable<Person> GetPersons() {
    //     return _connection.Table<Person>();
    // }

    /// <summary>
    /// 字节数组转16进制字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string byteToHexStr(byte[] bytes)
    {
        var returnStr = "";

        if (bytes != null) {
            for (var i = 0; i < bytes.Length; i++) {
                returnStr += bytes[i].ToString("X2");
            }
        }

        return returnStr;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        // Concatenate the bytes into one long string
        return ba.Aggregate(new StringBuilder(32), (sb, b) => sb.Append(b.ToString("X2"))).ToString();
    }

    public IEnumerable<T> Table<T>(T Obj = null) where T : class, new()
    {
        var result = _connection.GetTableInfo(typeof(T).Name);

        if (result == null || result.Count == 0) {
            _connection.CreateTable<T>(CreateFlags.AllImplicit);
        }

        return _connection.Table<T>();
    }

    // public IEnumerable<Person> GetPersonsNamedRoberto() {
    //     return _connection.Table<Person>().Where(x => x.Name == "Roberto");
    // }
    //
    // public Person GetJohnny() {
    //     return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
    // }
    //
    // public Person CreatePerson() {
    //     var p = new Person {
    //         Name = "Johnny",
    //         Surname = "Mnemonic",
    //         Age = 21
    //     };
    //     _connection.Insert(p);
    //
    //     return p;
    // }

}

}