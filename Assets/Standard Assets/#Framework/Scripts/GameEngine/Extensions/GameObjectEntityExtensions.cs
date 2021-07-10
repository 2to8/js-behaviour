#if false
using System;
using Unity.Entities;
using UnityEngine;


namespace Game.Extensions
{


    public static class GameObjectEntityExtensions
    {
        private static Action<Entity, ComponentType, object> m_SetComponentObjectDelegate;

        /// <summary>
        /// Add a Component to a GameObjectEntity.
        /// </summary>
        /// <param name="gameObjectEntity"><see cref="GameObjectEntity"/>.</param>
        /// <typeparam name="T">Type of Component to add.</typeparam>
        /// <returns>Added Component.</returns>
        public static T AddComponent<T>(this GameObjectEntity gameObjectEntity)
            where T : Component {
            if(!Application.isPlaying) {
                return null;
            }

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = gameObjectEntity.Entity;
            T component = gameObjectEntity.gameObject.GetComponent<T>() ??
                          gameObjectEntity.gameObject.AddComponent<T>();
            entityManager.AddComponent(entity, typeof(T));
            entityManager.SetComponentObject(entity, ComponentType.ReadWrite<T>(), component);

            return component;
        }

        /// <summary>
        /// Remove a Component from a GameObjectEntity.
        /// </summary>
        /// <param name="gameObjectEntity"><see cref="GameObjectEntity"/>.</param>
        /// <param name="component">Component to remove.</param>
        /// <typeparam name="T">Type of Component to remove.</typeparam>
        public static void RemoveComponent<T>(this GameObjectEntity gameObjectEntity, T component)
            where T : Component {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = gameObjectEntity.Entity;
            entityManager.RemoveComponent(entity, typeof(T));
            GameObject.Destroy(component);
        }

        /// <summary>
        /// Cached reflection for the internal method SetComponentObject within the EntityManager.
        /// </summary>
        /// <param name="entityManager"><see cref="EntityManager"/>.</param>
        /// <param name="entity">The <see cref="Entity"/> to set the object to.</param>
        /// <param name="componentType">The <see cref="ComponentType"/> of the object to set.</param>
        /// <param name="componentObject">The object to set.</param>
        public static void SetComponentObject(
            this EntityManager entityManager,
            Entity entity,
            ComponentType componentType,
            object componentObject) {
            if(m_SetComponentObjectDelegate == null) {
                m_SetComponentObjectDelegate = Delegate.CreateDelegate(
                        type: typeof(Action<Entity, ComponentType, object>),
                        target: entityManager,
                        method: "SetComponentObject",
                        ignoreCase: false)
                    as Action<Entity, ComponentType, object>;
            }

            if(m_SetComponentObjectDelegate != null)
                m_SetComponentObjectDelegate(entity, componentType, componentObject);
            else
                throw new NullReferenceException("SetComponentObject method signature changed");
        }
    }
}
# endif