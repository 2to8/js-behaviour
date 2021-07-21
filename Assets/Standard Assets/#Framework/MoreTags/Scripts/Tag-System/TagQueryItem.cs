using GameEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags
{
    public class TagQueryItem
    {
        List<string> m_Tags = new List<string>();
        List<Type> m_Types = new List<Type>();
        TagQuery m_Query;

        public TagQueryItem(TagQuery query)
        {
            m_Query = query;
        }

        public List<GameObject> result => m_Query.result;
        public List<GameObject> newResult => m_Query.newResult;

        public T FirstOrDefault<T>()
        {
            return result.Select(go => go.GetComponent<T>()).FirstOrDefault(t => t != null);
        }

        public Component FirstOrDefault()
        {
            return result.Select(go => go.GetComponent(m_Types.FirstOrDefault())).FirstOrDefault(t => t != null);
        }

        public TagQueryItem withTags(params string[] tags)
        {
            m_Tags.AddRange(tags);
            m_Tags = m_Tags.Distinct().ToList();
            return this;
        }

        public TagQueryItem with<T>() where T : Component
        {
            m_Types.AddOnce(typeof(T));
            return this;
        }

        public TagQueryItem withTypes(params Type[] types)
        {
            m_Types.AddRange(types.Where(t => t != null && typeof(Component).IsAssignableFrom(t)));
            m_Types = m_Types.Distinct().ToList();
            return this;
        }

        public TagQueryItem tags(params string[] tags) => m_Query.add.withTags(tags);
        public TagQueryItem types(params Type[] types) => m_Query.add.withTypes(types);
        public (string[] tags, Type[] types) ToQuery => (m_Tags.ToArray(), m_Types.ToArray());
    }
}