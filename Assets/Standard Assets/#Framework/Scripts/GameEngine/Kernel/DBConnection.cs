using SqlCipher4Unity3D;

namespace GameEngine.Kernel {

public static class SqliteConnectionExtion {

    public static TableQuery<T> Table<T>(this SQLiteConnection connection) where T : class, new()
    {
        var result = connection.GetTableInfo(typeof(T).Name);

        if (result == null || result.Count == 0) {
            connection.CreateTable<T>(CreateFlags.AllImplicit);
        }

        return new TableQuery<T>(connection);
    }

}

public class DbConnection : SQLiteConnection {

    public static DbConnection Instance;

    public DbConnection(string databasePath, string password = null, bool storeDateTimeAsTicks = false) : base(
        databasePath, password, storeDateTimeAsTicks) { }

    public DbConnection(string databasePath, string password, SQLiteOpenFlags openFlags,
        bool storeDateTimeAsTicks = false) : base(databasePath, password, openFlags, storeDateTimeAsTicks) { }

    public TableQuery<T> Table<T>() where T : new()
    {
        var result = GetTableInfo(typeof(T).Name);

        if (result == null || result.Count == 0) {
            CreateTable<T>(CreateFlags.AllImplicit);
        }

        return base.Table<T>();
    }

}

}