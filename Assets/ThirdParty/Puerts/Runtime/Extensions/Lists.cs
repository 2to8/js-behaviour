using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Puerts.Extensions {

public static class Lists {

    public static bool ContainsList<T>(this IEnumerable<IEnumerable<T>> list, IEnumerable<T> other)
    {
        if (list == null || !list.Any()) return false;
        if (list.Contains(other)) return true;

        return list.Any(tl => {
            var ret = tl.Count() == other.Count();
            if (ret) {
                tl.ForEach((tp1, ii) => {
                    if (!Equals(tp1, other.ElementAt(ii))) {
                        ret = false;
                    }
                });
            }
            return ret;
        });
    }

}

}