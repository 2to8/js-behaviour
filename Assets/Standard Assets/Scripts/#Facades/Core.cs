using System;
using System.Linq;
using System.Reflection;

public static partial class Core {

    public static Assembly MainAssembly => GetAssemblyByName("Assembly-CSharp");

    public static Assembly GetAssemblyByName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SingleOrDefault(assembly => assembly.GetName().Name == name);
    }
}