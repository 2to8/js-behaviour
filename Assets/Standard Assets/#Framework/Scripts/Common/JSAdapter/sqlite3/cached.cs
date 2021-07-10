using System.Collections.Generic;

namespace Common.JSAdapter.sqlite3 {

public class cached {

    public static Dictionary<string, Database> objects = new Dictionary<string, Database>();

    public Database Database(string path, string password = "", bool DateAsTimestamp = true) =>
        new Database(path, password, DateAsTimestamp);

}

}