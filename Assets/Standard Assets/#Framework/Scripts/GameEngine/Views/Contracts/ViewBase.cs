using Sirenix.OdinInspector;
using System;
using UnityEngine; //using Unity.Entities;

namespace GameEngine.Views.Contracts {

//[ RequiresEntityConversion]
[Serializable]
public abstract class ViewBase : SerializedMonoBehaviour /*, IConvertGameObjectToEntity*/ {

#if ECS
    IConvertGameObjectToEntity convertGameObjectToEntityImplementation;
    GameObjectConversionSystem m_conversionSystem;
    Entity m_entity;
    EntityManager? m_entityManager;

    public Entity Entity {
        get =>

            // if( m_entity == Entity.Null ) {
            //     var entity = DoConvent();
            //
            //     if( m_entity == Entity.Null ) {
            //         m_entity = entity;
            //     }
            // }
            m_entity;
        set => m_entity = value;
    }

    public EntityManager? Em => m_entityManager ??
        (m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager);

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        m_entity = entity;
        m_entityManager = dstManager;
        m_conversionSystem = conversionSystem;
    }
#endif

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        // if( Entity == Entity.Null ) {
        //     DoConvent();
        // }

        //EM.AddComponentData(Entity,new Translation { Value = new float3(1,1,1)});
    }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    protected static void RuntimeInitializeOnLoadMethod() { }

    //
    //protected Entity DoConvent() => default;
    // if( m_entity != Entity.Null ) {
    //     return m_entity;
    // }
    //
    // if( !Application.isPlaying ) {
    //     DefaultWorldInitialization.DefaultLazyEditModeInitialize();
    // }
    //
    // // gameObject.RequireComponent<ConvertToEntity>().ConversionMode =
    // //     ConvertToEntity.Mode.ConvertAndInjectGameObject;
    // // var settings =
    // //     GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
    // //
    //  return GameObjectConversionUtility.ConversionFlags(gameObject, settings);

}

}