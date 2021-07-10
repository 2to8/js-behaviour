using System;

namespace GameEngine.Extensions {

public static class FloatExtensions {

    // [SecuritySafeCritical] public static bool operator ==(float a, float b) {
    //      return a.eq(b);
    //  }
    //
    // [SecuritySafeCritical] public static bool operator !=(float a, float b) {
    //      return !(a == b);
    //  }

    public static bool eq(this float a, float b, float epsilon = float.Epsilon)
    {
        var absA = Math.Abs(a);
        var absB = Math.Abs(b);
        var diff = Math.Abs(a - b);

        if (a == b) {
            return true;
        }

        if (a == 0 || b == 0 || diff < float.Epsilon) {
            // a or b is zero or both are extremely close to it
            // relative error is less meaningful here
            return diff < epsilon;
        }

        // use relative error
        return diff / (absA + absB) < epsilon;
    }

}

}