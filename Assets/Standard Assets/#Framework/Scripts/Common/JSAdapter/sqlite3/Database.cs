using SqlCipher4Unity3D;

namespace Common.JSAdapter.sqlite3 {

public class Database : SQLiteConnection {

    public Database(string databasePath, string password = null, bool storeDateTimeAsTicks = true) : base(databasePath,
        password, storeDateTimeAsTicks) { }

    public Database(string databasePath, bool storeDateTimeAsTicks) : base(databasePath, storeDateTimeAsTicks) { }

    public Database(string connectionString, string databasePath, SQLiteOpenFlags openFlags,
        bool storeDateTimeAsTicks = true) : base(connectionString, databasePath, openFlags, storeDateTimeAsTicks) { }

    public Database(SQLiteConnectionString connectionString) : base(connectionString) { }

    public void prepare(string statement, params string[] args) { }

    public void run(string statement, params string[] args) { }

    public void get(string statement, params string[] args) { }

    public void all(string statement, params string[] args) { }

    public void each(string statement, params string[] args) { }

    public void map(string statement, params string[] args) { }

    public void backup() { }

    public void addListener(string type) { }

    public void on(string type) { }

    public void removeListener(string type) { }

    public void removeAllListeners(string type) { }

}

}