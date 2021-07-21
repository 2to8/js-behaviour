using GameEngine.Extensions;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags
{
    public class TagQuery
    {
        List<GameObject> m_Result = null;
        List<TagQueryItem> data = new List<TagQueryItem>();
        HashSet<Transform> parents = new HashSet<Transform>();
        public TagQueryItem tags(params string[] tags) => add.withTags(tags);
        public TagQueryItem tags<T>(params string[] tags) where T : Component => add.withTags(tags).with<T>();
        public TagQueryItem tags(Type type, params string[] tags) => add.withTags(tags).withTypes(type);
        public T Find<T>(params string[] tags) where T : Component => add.withTags(tags).with<T>().FirstOrDefault<T>();
        public Component Find(Type type, params string[] tags) => add.withTags(tags).withTypes(type).FirstOrDefault();
        public TagQueryItem types(params Type[] types) => add.withTypes(types);

        public TagQueryItem add =>
            new TagQueryItem(this).Of(t => {
                data.Add(t);
                m_Result = null;
            });

        public TagQuery Parent(params Component[] component) => Parent(component.Select(t => t?.gameObject).ToArray());

        public TagQuery Parent(params GameObject[] component) =>
            component == null || component.Length == 0
                ? this
                : this.Of(t => component.Where(t => t?.transform != null).ForEach(t => parents.Add(t.transform)));

        public List<GameObject> newResult => this.Of(t => t.m_Result = null).result;

        public List<GameObject> result =>
            m_Result ??= TagSystem.Query(parents.ToArray(), data.Select(t => t.ToQuery).ToArray()).ToList();
    }
}