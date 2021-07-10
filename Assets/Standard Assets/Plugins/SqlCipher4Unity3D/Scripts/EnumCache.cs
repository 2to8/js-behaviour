using System;
using System.Collections.Generic;

namespace SqlCipher4Unity3D {

static class EnumCache {

    static readonly Dictionary<Type, EnumCacheInfo> Cache = new Dictionary<Type, EnumCacheInfo>();

    public static EnumCacheInfo GetInfo<T>()
    {
        return GetInfo(typeof(T));
    }

    public static EnumCacheInfo GetInfo(Type type)
    {
        lock(Cache) {
            EnumCacheInfo info = null;

            if(!Cache.TryGetValue(type, out info)) {
                info = new EnumCacheInfo(type);
                Cache[type] = info;
            }

            return info;
        }
    }

}

}