using System;
using System.Collections.Generic;

namespace Common.JSAdapter.sqlite3 {

public class Helpers {

    public static object MakeDictionary(params Type[] types) =>
        Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(types));

    public static object MakeList(params Type[] types) =>
        Activator.CreateInstance(typeof(List<>).MakeGenericType(types));

}

}