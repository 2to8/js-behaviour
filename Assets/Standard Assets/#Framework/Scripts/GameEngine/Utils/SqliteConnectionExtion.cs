using SqlCipher4Unity3D;

namespace GameEngine.Utils {

public static class SqliteConnectionExtion {

    public static TableQuery<T> Table<T>(this SQLiteConnection connection) where T : new()
    {
        var result = connection.GetTableInfo(typeof(T).Name);

        if (result == null || result.Count == 0) {
            connection.CreateTable<T>(CreateFlags.AllImplicit);
        }

        return new TableQuery<T>(connection);
    }

}

}