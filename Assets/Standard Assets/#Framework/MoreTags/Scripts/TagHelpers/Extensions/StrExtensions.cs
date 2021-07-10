using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreTags.TagHelpers.Extensions {

public static class StrExtensions {

    public static IList<T> RemoveUnique<T>(this IList<T> list, params T[] target)
    {
        if (target.Length == 0 || list == null || list.Count == 0) {
            return list;
        }
        if (typeof(T) == typeof(string)) {
            new List<T>(list).Where(s => target.Any(t =>
                    string.Equals($"{t}".Trim(), $"{s}".Trim(),
                        StringComparison.OrdinalIgnoreCase)))
                .ForEach(s => {
                    list.Remove(s);
                });
        } else {
            target.Where(t => list.Contains(t)).ForEach(t => list.Remove(t));
        }
        return list;
    }

    public static IList<T> AddUnique<T>(this IList<T> list, params T[] target)
    {
        if (target.Length == 0) {
            return list;
        }
        if (list == null) {
            list = new List<T>();
        }
        if (typeof(T) == typeof(string)) {
            target.Where(s =>
                    !string.IsNullOrEmpty(s as string)
                    && list.All(t =>
                        !string.Equals($"{t}".Trim(), $"{s}".Trim(),
                            StringComparison.OrdinalIgnoreCase)))
                .ForEach(s => {
                    list.Add(s);
                });
        } else {
            target.Where(t => (t != null) & !list.Contains(t)).ForEach(t => list.Add(t));
        }
        return list;
    }

}

}