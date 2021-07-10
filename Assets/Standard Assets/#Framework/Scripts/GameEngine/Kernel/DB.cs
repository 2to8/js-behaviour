using GameEngine.Models.Contracts;
using SqlCipher4Unity3D;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq.Expressions;
using Core_1 = Core;

namespace GameEngine.Kernel {

public static class DB {

    public static SQLiteConnection Connection => Core_1.Connection;
    static SQLiteConnection m_Memory;

    public static SQLiteConnection memory =>
        m_Memory ?? (m_Memory = new SQLiteConnection(":memory:", "DC2F9B09-679C-4E27-ADE1-03BBB9231B3D"));

    public static SQLiteConnection storage => Connection;

    public static void CreateTable<T>() where T : IData => storage.CreateTable<T>();

    public static T Insert<T>(T obj) where T : IData
    {
        Connection.CreateTable<T>();
        //Connection.InsertWithChildren(obj, true);
        Connection.InsertOrReplace(obj);
       // Connection.InsertOrReplaceWithChildren(obj, true);

        return obj;
    }

    public static T Load<T>(Expression<Func<T, bool>> predExpr = null) where T : IData, new() =>
        Table<T>().FirstOrDefault(predExpr);

    public static TableQuery<T> Table<T>(T obj = default) => Connection.Table<T>();

    public static T Save<T>(T obj) where T : IData => Update(obj);

    public static T Update<T>(T obj) where T : IData
    {
        Connection.CreateTable<T>();
        obj.Updated = Core_1.TimeStamp();
        Connection.InsertOrReplaceWithChildren(obj, true);

        return obj;
    }

    public static int Delete(object obj) => Connection.Delete(obj);

}

}