using GameEngine.Extensions;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Utils
{
    [Serializable]
    public class RndReferenceItem
    {
        [Range(0, 100)]
        public int ratio;

        public int index = -1;

        public AssetLabelReference label;
        public UnityEngine.Object[] objects = { };

        public Object target =>
            objects.ElementAtOrDefault(index < 0 ? (index = Random.Range(0, objects.Length)) : index);

        public Texture2D texture2D => target as Texture2D;

        public Sprite sprite => target is Texture2D texture
            ? Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero)
            : (target is Sprite sp ? sp : null);

        public GameObject gameObject => target as GameObject;
    }

    public class RndReference : SerializedMonoBehaviour
    {
        public AssetLabelReference mainLabel;

        public RndReferenceItem[] items = new RndReferenceItem[] { };
        public int totalRatio => items.Sum(t => t.ratio);

        public RndReferenceItem GetItem(params string[] labelName)
        {
            var all = new List<RndReferenceItem>();
            var weight = new List<int>();

            items.ForEach(t => {
                var res = t.label.RuntimeKeyIsValid()
                    && (labelName.Length == 0
                        || labelName.Contains(t.label.labelString, StringComparer.OrdinalIgnoreCase))
                        ? t.objects.ElementAtOrDefault(Random.Range(0, t.objects.Length))
                        : null;

                if (res != null) {
                    all.Add(t);
                    weight.Add(t.ratio);
                }
            });
            int rand = Random.Range(1, weight.Sum());
            int tmp = 0;

            for (int i = 0; i < all.Count; i++) {
                tmp += weight[i];

                if (rand < tmp) {
                    return all[i];
                }
            }

            return all.FirstOrDefault();
        }
    }
}
