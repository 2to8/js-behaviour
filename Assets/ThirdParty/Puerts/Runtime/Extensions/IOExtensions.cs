using System;
using System.IO;

namespace Puerts.Extensions {

public static class IOExtensions {

    public static void DeleteEmptyDirs(this DirectoryInfo dir)
    {
        foreach (DirectoryInfo d in dir.GetDirectories()) d.DeleteEmptyDirs();
        try {
            dir.Delete();
        } catch (IOException) { } catch (UnauthorizedAccessException) { }
    }

}

}