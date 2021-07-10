using System;

namespace SqlCipher4Unity3D {

/// <summary>
/// Represents a parsed connection string.
/// </summary>
public class SQLiteConnectionString {

    const string DateTimeSqliteDefaultFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";
    public string UniqueKey { get; }
    public string DatabasePath { get; }
    public bool StoreDateTimeAsTicks { get; }
    public bool StoreTimeSpanAsTicks { get; }
    public string DateTimeStringFormat { get; }
    public System.Globalization.DateTimeStyles DateTimeStyle { get; }
    public object Key { get; }
    public SQLiteOpenFlags OpenFlags { get; }
    public Action<SQLiteConnection> PreKeyAction { get; }
    public Action<SQLiteConnection> PostKeyAction { get; }
    public string VfsName { get; }
#if NETFX_CORE
		static readonly string MetroStyleDataPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        public static readonly string[] InMemoryDbPaths = new[]
        {
            ":memory:",
            "file::memory:"
        };

        public static bool IsInMemoryPath(string databasePath)
        {
            return InMemoryDbPaths.Any(i => i.Equals(databasePath, StringComparison.OrdinalIgnoreCase));
        }
#endif

    /// <summary>
    /// Constructs a new SQLiteConnectionString with all the data needed to open an SQLiteConnection.
    /// </summary>
    /// <param name="databasePath">
    /// Specifies the path to the database file.
    /// </param>
    /// <param name="storeDateTimeAsTicks">
    /// Specifies whether to store DateTime properties as ticks (true) or strings (false). You
    /// absolutely do want to store them as Ticks in all new projects. The value of false is
    /// only here for backwards compatibility. There is a *significant* speed advantage, with no
    /// down sides, when setting storeDateTimeAsTicks = true.
    /// If you use DateTimeOffset properties, it will be always stored as ticks regardingless
    /// the storeDateTimeAsTicks parameter.
    /// </param>
    public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks = true) : this(databasePath,
        SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite, storeDateTimeAsTicks) { }

    /// <summary>
    /// Constructs a new SQLiteConnectionString with all the data needed to open an SQLiteConnection.
    /// </summary>
    /// <param name="databasePath">
    /// Specifies the path to the database file.
    /// </param>
    /// <param name="storeDateTimeAsTicks">
    /// Specifies whether to store DateTime properties as ticks (true) or strings (false). You
    /// absolutely do want to store them as Ticks in all new projects. The value of false is
    /// only here for backwards compatibility. There is a *significant* speed advantage, with no
    /// down sides, when setting storeDateTimeAsTicks = true.
    /// If you use DateTimeOffset properties, it will be always stored as ticks regardingless
    /// the storeDateTimeAsTicks parameter.
    /// </param>
    /// <param name="key">
    /// Specifies the encryption key to use on the database. Should be a string or a byte[].
    /// </param>
    /// <param name="preKeyAction">
    /// Executes prior to setting key for SQLCipher databases
    /// </param>
    /// <param name="postKeyAction">
    /// Executes after setting key for SQLCipher databases
    /// </param>
    /// <param name="vfsName">
    /// Specifies the Virtual File System to use on the database.
    /// </param>
    public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks, object key = null,
        Action<SQLiteConnection> preKeyAction = null, Action<SQLiteConnection> postKeyAction = null,
        string vfsName = null) : this(databasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite,
        storeDateTimeAsTicks, key, preKeyAction, postKeyAction, vfsName) { }

    /// <summary>
    /// Constructs a new SQLiteConnectionString with all the data needed to open an SQLiteConnection.
    /// </summary>
    /// <param name="databasePath">
    /// Specifies the path to the database file.
    /// </param>
    /// <param name="openFlags">
    /// Flags controlling how the connection should be opened.
    /// </param>
    /// <param name="storeDateTimeAsTicks">
    /// Specifies whether to store DateTime properties as ticks (true) or strings (false). You
    /// absolutely do want to store them as Ticks in all new projects. The value of false is
    /// only here for backwards compatibility. There is a *significant* speed advantage, with no
    /// down sides, when setting storeDateTimeAsTicks = true.
    /// If you use DateTimeOffset properties, it will be always stored as ticks regardingless
    /// the storeDateTimeAsTicks parameter.
    /// </param>
    /// <param name="key">
    /// Specifies the encryption key to use on the database. Should be a string or a byte[].
    /// </param>
    /// <param name="preKeyAction">
    /// Executes prior to setting key for SQLCipher databases
    /// </param>
    /// <param name="postKeyAction">
    /// Executes after setting key for SQLCipher databases
    /// </param>
    /// <param name="vfsName">
    /// Specifies the Virtual File System to use on the database.
    /// </param>
    /// <param name="dateTimeStringFormat">
    /// Specifies the format to use when storing DateTime properties as strings.
    /// </param>
    /// <param name="storeTimeSpanAsTicks">
    /// Specifies whether to store TimeSpan properties as ticks (true) or strings (false). You
    /// absolutely do want to store them as Ticks in all new projects. The value of false is
    /// only here for backwards compatibility. There is a *significant* speed advantage, with no
    /// down sides, when setting storeTimeSpanAsTicks = true.
    /// </param>
    public SQLiteConnectionString(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks,
        object key = null, Action<SQLiteConnection> preKeyAction = null, Action<SQLiteConnection> postKeyAction = null,
        string vfsName = null, string dateTimeStringFormat = DateTimeSqliteDefaultFormat,
        bool storeTimeSpanAsTicks = true)
    {
        if(key != null && !((key is byte[]) || (key is string)))
            throw new ArgumentException("Encryption keys must be strings or byte arrays", nameof(key));

        UniqueKey = string.Format("{0}_{1:X8}", databasePath, (uint)openFlags);
        StoreDateTimeAsTicks = storeDateTimeAsTicks;
        StoreTimeSpanAsTicks = storeTimeSpanAsTicks;
        DateTimeStringFormat = dateTimeStringFormat;

        DateTimeStyle =
            "o".Equals(DateTimeStringFormat, StringComparison.OrdinalIgnoreCase)
            || "r".Equals(DateTimeStringFormat, StringComparison.OrdinalIgnoreCase)
                ? System.Globalization.DateTimeStyles.RoundtripKind
                : System.Globalization.DateTimeStyles.None;

        Key = key;
        PreKeyAction = preKeyAction;
        PostKeyAction = postKeyAction;
        OpenFlags = openFlags;
        VfsName = vfsName;

    #if NETFX_CORE
			DatabasePath = IsInMemoryPath(databasePath)
                ? databasePath
                : System.IO.Path.Combine(MetroStyleDataPath, databasePath);

    #else
        DatabasePath = databasePath;
    #endif
    }

}

}