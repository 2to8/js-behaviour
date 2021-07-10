using System;

namespace SqlCipher4Unity3D
{

public class SQLiteException : Exception
    {
        protected SQLiteException(SQLite3.Result r, string message) : base(message)
        {
            this.Result = r;
        }

        public SQLite3.Result Result { get; private set; }

        public static SQLiteException New(SQLite3.Result r, string message)
        {
            return new SQLiteException(r, message);
        }
    }

}