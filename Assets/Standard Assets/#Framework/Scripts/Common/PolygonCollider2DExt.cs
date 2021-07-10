using System.Collections.Generic;
using UnityEngine;

namespace Common {

public static class PolygonCollider2DExt {

    public static void ResetCollider(this PolygonCollider2D polygonCollider)
    {
        //polygonCollider = GetComponent<PolygonCollider2D>();
        var sprite = polygonCollider.GetComponent<SpriteRenderer>().sprite;

        for (var i = 0; i < polygonCollider.pathCount; i++) {
            polygonCollider.SetPath(i, (Vector2[])null);
        }

        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();
        var path = new List<Vector2>();

        for (var i = 0; i < polygonCollider.pathCount; i++) {
            path.Clear();
            sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path.ToArray());
        }
    }

}

}