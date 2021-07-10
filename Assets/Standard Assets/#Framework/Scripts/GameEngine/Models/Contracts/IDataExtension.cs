using GameEngine.Kernel;
using System;
using System.Linq.Expressions;

namespace GameEngine.Models.Contracts {

public static class IDataExtension {

    public static DbData<T> data<T>(this T data) where T : DbData<T>, new() => Core.Data(data);

    public static DbData<T> find<T>(this T data, Expression<Func<T, bool>> query) where T : DbData<T>, new() =>
        Core.Data(data);

}

}