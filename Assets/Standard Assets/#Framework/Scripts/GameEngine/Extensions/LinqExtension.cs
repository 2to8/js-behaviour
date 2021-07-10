using System;

namespace GameEngine.Extensions {

public static class LinqExtension {

    // public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
    // {
    //     while(enumerator.MoveNext()) {
    //         yield return enumerator.Current;
    //     }
    // }
    //
    // public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> input, int start = 0)
    // {
    //     var i = start;
    //
    //     foreach(var t in input) {
    //         yield return (i++, t);
    //     }
    // }
    //
    // public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
    // {
    //     // argument null checking omitted
    //     var i = 0;
    //
    //     foreach(var item in sequence) {
    //         action(i, item);
    //         i++;
    //     }
    // }
    //
    // public static void ForEach<T>(this T[,] list, Action<int, int, T> action)
    // {
    //     for(var i = 0; i < list.GetLength(0); i++) {
    //         for(var j = 0; j < list.GetLength(1); j++) {
    //             action?.Invoke(i, j, list[i, j]);
    //         }
    //     }
    // }

}

}