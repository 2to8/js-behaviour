﻿using System.Collections.Generic;
using SqlCipher4Unity3D;
using System;
using System.IO;
using UnityEngine;

#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
namespace example {

public class DataService {

    private readonly SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {
    #if UNITY_EDITOR

        //string dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
        string dbPath = string.Format(@"Assets/Resources/{0}", DatabaseName);
    #else
        // check if file exists in Application.persistentDataPath
        string filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
        if (!File.Exists(filepath)) {
            Debug.Log("Database not in Persistent path");

            // if it doesn't ->
            // open StreamingAssets directory and load the knex_db ->

            // #if UNITY_ANDROID
            //                 WWW loadDb =
            //      new WWW ("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName); // this is the path to your StreamingAssets in android
            //                 while (!loadDb.isDone) { } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            //                 // then save to Application.persistentDataPath
            //                 File.WriteAllBytes (filepath, loadDb.bytes);
            // #elif UNITY_IOS
            //                 string loadDb =
            //      Application.dataPath + "/Raw/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            //                 // then save to Application.persistentDataPath
            //                 File.Copy (loadDb, filepath);
            // #elif UNITY_WP8
            //                 string loadDb =
            //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            //                 // then save to Application.persistentDataPath
            //                 File.Copy (loadDb, filepath);
            //
            // #elif UNITY_WINRT
            //                 string loadDb =
            //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            //                 // then save to Application.persistentDataPath
            //                 File.Copy (loadDb, filepath);
            // #elif UNITY_STANDALONE_OSX
            //                 string loadDb =
            //      Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            //                 // then save to Application.persistentDataPath
            //                 File.Copy(loadDb, filepath);
            // #else
            //                 string loadDb =
            //      Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
            //                 // then save to Application.persistentDataPath
            //                 File.Copy(loadDb, filepath);
            // #endif
            //
            //                 Debug.Log("Database written");
            //             }
            //
            //             var dbPath = filepath;
            var res = Resources.Load<TextAsset>(DatabaseName.Substring(0,
                DatabaseName.LastIndexOf(".", StringComparison.Ordinal)));
            if (res == null) {
                res = Resources.Load<TextAsset>(DatabaseName.Substring(0,
                    DatabaseName.IndexOf(".", StringComparison.Ordinal)));
            }
            if (res != null) {
                File.WriteAllBytes(filepath, res.bytes);
            }
   }
        var dbPath = filepath;
    #endif
        _connection = new SQLiteConnection(dbPath, "password");
        Debug.Log("Final PATH: " + dbPath);
    }

    public void CreateDB()
    {
        _connection.DropTable<Person>();
        _connection.CreateTable<Person>();
        _connection.InsertAll(new[] {
            new Person {
                Id = 1,
                Name = "Tom",
                Surname = "Perez",
                Age = 56
            },
            new Person {
                Id = 2,
                Name = "Fred",
                Surname = "Arthurson",
                Age = 16
            },
            new Person {
                Id = 3,
                Name = "John",
                Surname = "Doe",
                Age = 25
            },
            new Person {
                Id = 4,
                Name = "Roberto",
                Surname = "Huertas",
                Age = 37
            }
        });
    }

    public IEnumerable<Person> GetPersons()
    {
        return _connection.Table<Person>();
    }

    public IEnumerable<Person> GetPersonsNamedRoberto()
    {
        return _connection.Table<Person>().Where(x => x.Name == "Roberto");
    }

    public Person GetJohnny()
    {
        return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
    }

    public Person CreatePerson()
    {
        Person p = new Person {
            Name = "Johnny",
            Surname = "Mnemonic",
            Age = 21
        };
        _connection.Insert(p);
        return p;
    }

}

}