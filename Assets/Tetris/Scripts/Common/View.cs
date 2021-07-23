using System.Linq;
using System.Reflection;
using GameEngine.Extensions;
using MoreTags.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common
{
    public partial class View : SerializedMonoBehaviour
    {
        protected bool BindingInited;
        protected virtual void OnEnable() { }

        [Button]
        public void InitBinding()
        {
            if (!BindingInited) {
                //BindingInited = true;
                Debug.Log($"binding tags: {GetType().FullName}".ToRed());
                GetType()
                    .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Static).Where(t => t.IsDefined(typeof(TagsAttribute))).ToList().ForEach(mi => {
                        mi.GetCustomAttribute<TagsAttribute>().Invoke(mi, this);
                    });
            }
        }

        public virtual void Start()
        {
            InitBinding();
        }
    }

    public partial class View<T> : View where T : View<T> { }
}