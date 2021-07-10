using System;
using System.Threading;

namespace SqlCipher4Unity3D {

/// <summary>
/// This is a normal connection except it contains a Lock method that
/// can be used to serialize access to the database
/// in lieu of using the sqlite's FullMutex support.
/// </summary>
public class SQLiteConnectionWithLock : SQLiteConnection
{
    readonly object _lockPoint = new object ();

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SQLite.SQLiteConnectionWithLock"/> class.
    /// </summary>
    /// <param name="connectionString">Connection string containing the DatabasePath.</param>
    public SQLiteConnectionWithLock (SQLiteConnectionString connectionString)
        : base (connectionString)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:SQLite.SQLiteConnectionWithLock"/> skip lock.
    /// </summary>
    /// <value><c>true</c> if skip lock; otherwise, <c>false</c>.</value>
    public bool SkipLock { get; set; }

    /// <summary>
    /// Lock the database to serialize access to it. To unlock it, call Dispose
    /// on the returned object.
    /// </summary>
    /// <returns>The lock.</returns>
    public IDisposable Lock ()
    {
        return SkipLock ? (IDisposable)new FakeLockWrapper() : new LockWrapper (_lockPoint);
    }

    class LockWrapper : IDisposable
    {
        object _lockPoint;

        public LockWrapper (object lockPoint)
        {
            _lockPoint = lockPoint;
            Monitor.Enter (_lockPoint);
        }

        public void Dispose ()
        {
            Monitor.Exit (_lockPoint);
        }
    }
    class FakeLockWrapper : IDisposable
    {
        public void Dispose ()
        {
        }
    }
}

}