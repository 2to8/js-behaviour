using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Utils.Helpers {

public static class Components {
    // [MenuItem("Components/TestFindAssembly")]
    public static IEnumerable<Type> FindTypes(Expression<Func<Type, bool>> predicate)
    {
        return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !(a.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                    && (a.GetName().Name == "Assembly-CSharp"
                        || a.GetName().Name == "Assembly-CSharp-firstpass"))
            from type in assembly.GetExportedTypes().Where(predicate.Compile())
            select type;
    }
}

}