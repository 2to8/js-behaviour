namespace PuertsStaticWrap
{
    public static class AutoStaticCodeRegister
    {
        public static void Register(Puerts.JsEnv jsEnv)
        {
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Vector2), UnityEngine_Vector2_Wrap.GetRegisterInfo);
            UnityEngine_Vector2_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Vector3), UnityEngine_Vector3_Wrap.GetRegisterInfo);
            UnityEngine_Vector3_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Vector4), UnityEngine_Vector4_Wrap.GetRegisterInfo);
            UnityEngine_Vector4_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Quaternion), UnityEngine_Quaternion_Wrap.GetRegisterInfo);
            UnityEngine_Quaternion_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Color), UnityEngine_Color_Wrap.GetRegisterInfo);
            UnityEngine_Color_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Rect), UnityEngine_Rect_Wrap.GetRegisterInfo);
            UnityEngine_Rect_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Bounds), UnityEngine_Bounds_Wrap.GetRegisterInfo);
            UnityEngine_Bounds_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Ray), UnityEngine_Ray_Wrap.GetRegisterInfo);
            UnityEngine_Ray_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.RaycastHit), UnityEngine_RaycastHit_Wrap.GetRegisterInfo);
            UnityEngine_RaycastHit_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Matrix4x4), UnityEngine_Matrix4x4_Wrap.GetRegisterInfo);
            UnityEngine_Matrix4x4_Wrap.InitBlittableCopy(jsEnv);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Events.UnityEvent), UnityEngine_Events_UnityEvent_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Time), UnityEngine_Time_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Transform), UnityEngine_Transform_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Object), UnityEngine_Object_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.GameObject), UnityEngine_GameObject_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Component), UnityEngine_Component_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Behaviour), UnityEngine_Behaviour_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.MonoBehaviour), UnityEngine_MonoBehaviour_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AudioClip), UnityEngine_AudioClip_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.ParticleSystem.MainModule), UnityEngine_ParticleSystem_MainModule_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AnimationClip), UnityEngine_AnimationClip_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Animator), UnityEngine_Animator_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AnimationCurve), UnityEngine_AnimationCurve_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AndroidJNI), UnityEngine_AndroidJNI_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AndroidJNIHelper), UnityEngine_AndroidJNIHelper_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Collider), UnityEngine_Collider_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Collision), UnityEngine_Collision_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Rigidbody), UnityEngine_Rigidbody_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Screen), UnityEngine_Screen_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Texture), UnityEngine_Texture_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.TextAsset), UnityEngine_TextAsset_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.SystemInfo), UnityEngine_SystemInfo_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Input), UnityEngine_Input_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Mathf), UnityEngine_Mathf_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Camera), UnityEngine_Camera_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.ParticleSystem), UnityEngine_ParticleSystem_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AudioSource), UnityEngine_AudioSource_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.AudioListener), UnityEngine_AudioListener_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Physics), UnityEngine_Physics_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.SceneManagement.Scene), UnityEngine_SceneManagement_Scene_Wrap.GetRegisterInfo);
            jsEnv.AddLazyStaticWrapLoader(typeof(UnityEngine.Networking.UnityWebRequest), UnityEngine_Networking_UnityWebRequest_Wrap.GetRegisterInfo);
            
        }
    }
}