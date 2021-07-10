using Puerts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlCipher4Unity3D {

[Configure]
public static class _PuertsConfig {

    [Binding]
    static IEnumerable<Type> SQLiteCommandBindTypes => SQLiteCommand.BindTypes;

    [Binding]
    static IEnumerable<Type> SQLite3BindTypes => new List<Type> {
        typeof(SQLite3)
    };

}

}