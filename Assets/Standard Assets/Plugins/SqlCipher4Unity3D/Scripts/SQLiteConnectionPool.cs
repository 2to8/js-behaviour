using System.Collections.Generic;

namespace SqlCipher4Unity3D {

class SQLiteConnectionPool
{
    class Entry
    {
        public SQLiteConnectionWithLock Connection { get; private set; }

        public SQLiteConnectionString ConnectionString { get; }

        public object TransactionLock { get; } = new object ();

        public Entry (SQLiteConnectionString connectionString)
        {
            ConnectionString = connectionString;
            Connection = new SQLiteConnectionWithLock (ConnectionString);

            // If the database is FullMutex, then we don't need to bother locking
            if (ConnectionString.OpenFlags.HasFlag (SQLiteOpenFlags.FullMutex)) {
                Connection.SkipLock = true;
            }
        }

        public void Close ()
        {
            var wc = Connection;
            Connection = null;
            if (wc != null) {
                wc.Close ();
            }
        }
    }

    readonly Dictionary<string, Entry> _entries = new Dictionary<string, Entry> ();
    readonly object _entriesLock = new object ();

    static readonly SQLiteConnectionPool _shared = new SQLiteConnectionPool ();

    /// <summary>
    /// Gets the singleton instance of the connection tool.
    /// </summary>
    public static SQLiteConnectionPool Shared {
        get {
            return _shared;
        }
    }

    public SQLiteConnectionWithLock GetConnection (SQLiteConnectionString connectionString)
    {
        return GetConnectionAndTransactionLock (connectionString, out var _);
    }

    public SQLiteConnectionWithLock GetConnectionAndTransactionLock (SQLiteConnectionString connectionString, out object transactionLock)
    {
        var key = connectionString.UniqueKey;
        Entry entry;
        lock (_entriesLock) {
            if (!_entries.TryGetValue (key, out entry)) {
                // The opens the database while we're locked
                // This is to ensure another thread doesn't get an unopened database
                entry = new Entry (connectionString);
                _entries[key] = entry;
            }
            transactionLock = entry.TransactionLock;
            return entry.Connection;
        }
    }

    public void CloseConnection (SQLiteConnectionString connectionString)
    {
        var key = connectionString.UniqueKey;
        Entry entry;
        lock (_entriesLock) {
            if (_entries.TryGetValue (key, out entry)) {
                _entries.Remove (key);
            }
        }
        entry?.Close ();
    }

    /// <summary>
    /// Closes all connections managed by this pool.
    /// </summary>
    public void Reset ()
    {
        List<Entry> entries;
        lock (_entriesLock) {
            entries = new List<Entry> (_entries.Values);
            _entries.Clear ();
        }

        foreach (var e in entries) {
            e.Close ();
        }
    }
}

}