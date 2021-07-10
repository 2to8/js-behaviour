using GameEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags.Types {

[Serializable]
public class TagReference : Dictionary<string, TagRefs> {

    public new void Add(string tag, TagRefs data = null)
    {
        if (ContainsKey(tag = tag._TagKey())) {
            return;
        }
        data ??= new TagRefs();
        var row = TagData.FirstOrInsert(t => t.name == tag) ?? new TagData();

        if (row.Id == 0) {
            row.color = data.color;
            row.name = tag;
            TagData.Insert(row);
        }
        data.Id = row.Id;
        data.color = row.color;
        base.Add(tag, data);
    }

    public new void Remove(string tag)
    {
        if (base.ContainsKey(tag = tag._TagKey())) {
            base.Remove(tag);
        }
    }

    public new bool ContainsKey(string tag) => base.ContainsKey($"{tag}"._TagKey());

    public TagReference FetchAll()
    {
        TagManager.Data.ForEach(t => {
            if (!ContainsKey(t.name)) {
                this[t.name._TagKey()] = new TagRefs() {
                    color = t.color, Id = t.Id, gameObjects = new HashSet<GameObject>(),
                };
            }
        });

        return this;
    }

    public new TagRefs this[string tag] {
        get => string.IsNullOrEmpty(tag) ? new TagRefs() : base[tag._TagKey()] ?? new TagRefs();
        set {
            if (string.IsNullOrEmpty(tag = tag._TagKey())) {
                return;
            }

            if (value == null) {
                Remove(tag);

                return;
            }
            base[tag] = value;
        }
    }

}

}