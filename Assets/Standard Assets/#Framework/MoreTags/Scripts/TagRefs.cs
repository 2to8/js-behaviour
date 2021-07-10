using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreTags {

[Serializable]
public class TagRefs {

    public int Id;
    public Color color = Color.white;

    public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

    // public HashSet<Node> nodes = new HashSet<Node>();

}

}