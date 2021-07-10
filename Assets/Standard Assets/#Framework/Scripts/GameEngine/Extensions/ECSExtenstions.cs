#if ECS
using System;
using Sirenix.Utilities;
using Unity.Entities;

namespace Engine.Extensions {

public static class ECSExtenstions {

    public static void AddName(this EntityManager entityManager, Entity entity, string name)
    {
#if UNITY_EDITOR
        entityManager.SetName(entity, name);
#endif
    }

    public static string Name(this EntityManager entityManager, Entity entity)
    {
#if UNITY_EDITOR
        return entityManager.GetName(entity);
#endif
        return string.Empty;
    }

    public static void AddName(this EntityManager entityManager, Entity entity, Type type)
    {
#if UNITY_EDITOR
        entityManager.SetName(entity, type.GetNiceName());
#endif
    }

}

}
#endif