#if ECS
using Engine.Contracts;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
#endif

namespace Engine.Providers {

public abstract class View<T, TS> : Manager<View<T, TS>, View<T, TS>.Service> where T : IComponentData
    where TS : View<T, TS>.Service {

    public class Service : Controller<View<T, TS>, Service> { }

}

public abstract class UI<T, TS, TC> : Manager<UI<T, TS, TC>, UI<T, TS, TC>.Service> where T : IComponentData
    where TS : UI<T, TS, TC>.Service where TC : UIBehaviour {

    [ SerializeField, HideInInspector ]
    TC m_ui;

    [ ShowInInspector ]
    public TC ui { get => m_ui ?? (m_ui = GetComponentInParent<TC>()); set => m_ui = value; }

    public class Service : Controller<UI<T, TS, TC>, Service> { }

}

// public class Button<T, TS> : Manager<Button<T, TS>, Button<T, TS>.Service> where T : IComponentData
//     where TS : Button<T, TS>.Service {
//
//     public class Service : Controller<Button<T, TS>, Service> { }
//
// }
//
// public class Text<T, TS> : Manager<Text<T, TS>, Text<T, TS>.Service> where T : IComponentData
//     where TS : Text<T, TS>.Service {
//
//     public class Service : Controller<Text<T, TS>, Service> { }
//
// }
//
// public class Toggle<T, TS> : Manager<Toggle<T, TS>, Toggle<T, TS>.Service> where T : IComponentData
//     where TS : Toggle<T, TS>.Service {
//
//     public class Service : Controller<Toggle<T, TS>, Service> { }
//
// }
//
// public class ScrollView<T, TS> : Manager<ScrollView<T, TS>, ScrollView<T, TS>.Service>
//     where T : IComponentData where TS : ScrollView<T, TS>.Service {
//
//     public class Service : Controller<ScrollView<T, TS>, Service> { }
//
// }

}
#endif