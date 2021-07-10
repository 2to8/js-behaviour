using UnityEngine;

namespace GameEngine.Extensions {

public static class UnityEngineObjectExtention {

    public static bool IsNull(this Object o) // 或者名字叫IsDestroyed等等
        => o == null;

}

}