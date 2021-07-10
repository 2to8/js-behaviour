using System;

namespace PuertsStaticWrap
{
    public static class UnityEngine_RaycastHit_Wrap
    {
        static UnityEngine.RaycastHit HeapValue;
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8ConstructorCallback))]
        unsafe private static IntPtr Constructor(IntPtr isolate, IntPtr info, int paramLen, long data)
        {
            try
            {
                
                Puerts.PuertsDLL.ThrowException(isolate, "invalid arguments to UnityEngine.RaycastHit constructor");
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
            return IntPtr.Zero;
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_collider(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).collider;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_point(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).point;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_point(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).point = argHelper.Get<UnityEngine.Vector3>(false);
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_normal(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).normal;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_normal(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).normal = argHelper.Get<UnityEngine.Vector3>(false);
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_barycentricCoordinate(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).barycentricCoordinate;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_barycentricCoordinate(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).barycentricCoordinate = argHelper.Get<UnityEngine.Vector3>(false);
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_distance(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).distance;
                Puerts.PuertsDLL.ReturnNumber(isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_distance(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).distance = argHelper.GetFloat(false);
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_triangleIndex(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).triangleIndex;
                Puerts.PuertsDLL.ReturnNumber(isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_textureCoord(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).textureCoord;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_textureCoord2(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).textureCoord2;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_transform(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).transform;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_rigidbody(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).rigidbody;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_articulationBody(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).articulationBody;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_lightmapCoord(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.RaycastHit*)self;
                var result = (*obj).lightmapCoord;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        
        
        public static Puerts.TypeRegisterInfo GetRegisterInfo()
        {
            return new Puerts.TypeRegisterInfo()
            {
                BlittableCopy = true,
                Constructor = Constructor,
                Methods = new System.Collections.Generic.Dictionary<Puerts.MethodKey, Puerts.V8FunctionCallback>()
                {
                    
                },
                Properties = new System.Collections.Generic.Dictionary<string, Puerts.PropertyRegisterInfo>()
                {
                    {"collider", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_collider, Setter = null} },
                    {"point", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_point, Setter = S_point} },
                    {"normal", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_normal, Setter = S_normal} },
                    {"barycentricCoordinate", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_barycentricCoordinate, Setter = S_barycentricCoordinate} },
                    {"distance", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_distance, Setter = S_distance} },
                    {"triangleIndex", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_triangleIndex, Setter = null} },
                    {"textureCoord", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_textureCoord, Setter = null} },
                    {"textureCoord2", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_textureCoord2, Setter = null} },
                    {"transform", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_transform, Setter = null} },
                    {"rigidbody", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_rigidbody, Setter = null} },
                    {"articulationBody", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_articulationBody, Setter = null} },
                    {"lightmapCoord", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_lightmapCoord, Setter = null} },
                    
                }
            };
        }
        
        unsafe private static UnityEngine.RaycastHit StaticGetter(int jsEnvIdx, IntPtr isolate, Puerts.IGetValueFromJs getValueApi, IntPtr value, bool isByRef)
        {
            UnityEngine.RaycastHit* result = (UnityEngine.RaycastHit*)getValueApi.GetNativeObject(isolate, value, isByRef);
            return result == null ? default(UnityEngine.RaycastHit) : *result;
        }

        unsafe private static void StaticSetter(int jsEnvIdx, IntPtr isolate, Puerts.ISetValueToJs setValueApi, IntPtr value, UnityEngine.RaycastHit val)
        {
            HeapValue = val;
            fixed (UnityEngine.RaycastHit* result = &HeapValue)
            {
                var typeId = Puerts.JsEnv.jsEnvs[jsEnvIdx].GetTypeId(typeof(UnityEngine.RaycastHit));
                setValueApi.SetNativeObject(isolate, value, typeId, new IntPtr(result));
            }
        }
        
        public static void InitBlittableCopy(Puerts.JsEnv jsEnv)
        {
            Puerts.StaticTranslate<UnityEngine.RaycastHit>.ReplaceDefault(StaticSetter, StaticGetter);
            int jsEnvIdx = jsEnv.Index;
            jsEnv.RegisterGeneralGetSet(typeof(UnityEngine.RaycastHit), (IntPtr isolate, Puerts.IGetValueFromJs getValueApi, IntPtr value, bool isByRef) =>
            {
                return StaticGetter(jsEnvIdx, isolate, getValueApi, value, isByRef);
            }, (IntPtr isolate, Puerts.ISetValueToJs setValueApi, IntPtr value, object obj) => 
            {
                StaticSetter(jsEnvIdx, isolate, setValueApi, value, (UnityEngine.RaycastHit)obj);
            });
        }
        
    }
}