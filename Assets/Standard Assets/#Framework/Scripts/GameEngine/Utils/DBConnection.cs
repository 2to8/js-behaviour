using SqlCipher4Unity3D;

namespace GameEngine.Utils {

public class DbConnection : SQLiteConnection {

    public static DbConnection Instance;

    public DbConnection(string databasePath, string password = null, bool storeDateTimeAsTicks = false) : base(
        databasePath, password, storeDateTimeAsTicks) { }

    public DbConnection(string databasePath, string password, SQLiteOpenFlags openFlags,
        bool storeDateTimeAsTicks = false) : base(databasePath, password, openFlags, storeDateTimeAsTicks) { }

    public new TableQuery<T> Table<T>() where T : new()
    {
        var result = GetTableInfo(typeof(T).Name);

        if (result == null || result.Count == 0) {
            CreateTable<T>(CreateFlags.AllImplicit);
        }

        return base.Table<T>();
    }

}

}