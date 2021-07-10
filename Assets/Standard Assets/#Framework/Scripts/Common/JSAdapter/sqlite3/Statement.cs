using SqlCipher4Unity3D;
using System;

namespace Common.JSAdapter.sqlite3 {

public class Statement : SQLiteCommand {

    public Statement(Statement _this, string sql, Action action) : base(null) { }

    public Statement(SQLiteConnection conn) : base(conn) { }

    public void map(params string[] args) { }

}

}