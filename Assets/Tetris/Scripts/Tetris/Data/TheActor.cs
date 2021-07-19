using System;
using UnityEngine;

namespace Battle
{
    [Serializable]
    public class TheActor
    {
        public int col;
        public int row;
        public int actorId;

        [NonSerialized]
        public Transform transform;
        // public GameObject gameObject {
        //     get { return transform?.gameObject; }
        // }
    }
}