using SqlCipher4Unity3D;
using System.Collections.Generic;
using UnityEngine;

// using example;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
namespace GameEngine.Kernel {

public class DataService {

    readonly SQLiteConnection _connection;
    public string dbPath;
    public bool encrypt = false;

    public DataService(string DatabaseName)
    {
    #if UNITY_EDITOR
        dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
    #else
            // check if file exists in Application.persistentDataPath
            string filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the knex_db ->

#if UNITY_ANDROID
                WWW loadDb =
     new WWW ("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName); // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes (filepath, loadDb.bytes);
#elif UNITY_IOS
                string loadDb =
     Application.dataPath + "/Raw/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_WP8
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);

#elif UNITY_WINRT
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_STANDALONE_OSX
                string loadDb =
     Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#else
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif

                Debug.Log("Database written");
            }

            dbPath = filepath;
    #endif
        if (encrypt) {
            _connection = new DbConnection(dbPath, "40BDAD33-4470-4666-A5F0-5380ED41F33F");
        } else {
            _connection = new DbConnection(dbPath);
        }

        //conn = _connection;
        Debug.Log("Final PATH: " + dbPath);
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

    public IEnumerable<T> Table<T>() where T : class, new()
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