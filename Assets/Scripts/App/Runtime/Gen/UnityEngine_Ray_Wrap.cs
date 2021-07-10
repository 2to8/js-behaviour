using System;

namespace PuertsStaticWrap
{
    public static class UnityEngine_Ray_Wrap
    {
        static UnityEngine.Ray HeapValue;
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8ConstructorCallback))]
        unsafe private static IntPtr Constructor(IntPtr isolate, IntPtr info, int paramLen, long data)
        {
            try
            {
                
                if (paramLen == 2)
                {
                    
                    var argHelper0 = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                    var argHelper1 = new Puerts.ArgumentHelper((int)data, isolate, info, 1);
                    
                    
                    if (argHelper0.IsMatch(Puerts.JsValueType.NativeObject, typeof(UnityEngine.Vector3), false, false)
                        && argHelper1.IsMatch(Puerts.JsValueType.NativeObject, typeof(UnityEngine.Vector3), false, false))
                    {
                        
                        var Arg0 = argHelper0.Get<UnityEngine.Vector3>(false);
                        var Arg1 = argHelper1.Get<UnityEngine.Vector3>(false);
                        HeapValue = new UnityEngine.Ray(Arg0,Arg1);
                        
                        
                        fixed (UnityEngine.Ray* result = &HeapValue)
                        {
                            return new IntPtr(result);
                        }
                    }
                }
                
                if (paramLen == 0)
                {
                    
                    
                    
                    
                    {
                        
                        HeapValue = new UnityEngine.Ray();
                        
                        
                        fixed (UnityEngine.Ray* result = &HeapValue)
                        {
                            return new IntPtr(result);
                        }
                    }
                }
                
                Puerts.PuertsDLL.ThrowException(isolate, "invalid arguments to UnityEngine.Ray constructor");
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
            return IntPtr.Zero;
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void M_GetPoint(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                
                
                {
                    
                    var argHelper0 = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                    
                    
                    
                    {
                        
                        var Arg0 = argHelper0.GetFloat(false);
                        var result = (*obj).GetPoint(Arg0);
                        
                        Puerts.ResultHelper.Set((int)data, isolate, info, result);
                        
                        
                    }
                }
                
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void M_ToString(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                
                if (paramLen == 0)
                {
                    
                    
                    
                    
                    {
                        
                        var result = (*obj).ToString();
                        
                        Puerts.PuertsDLL.ReturnString(isolate, info, result);
                        
                        return;
                    }
                }
                
                if (paramLen == 1)
                {
                    
                    var argHelper0 = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                    
                    
                    if (argHelper0.IsMatch(Puerts.JsValueType.NullOrUndefined | Puerts.JsValueType.String, null, false, false))
                    {
                        
                        var Arg0 = argHelper0.GetString(false);
                        var result = (*obj).ToString(Arg0);
                        
                        Puerts.PuertsDLL.ReturnString(isolate, info, result);
                        
                        return;
                    }
                }
                
                if (paramLen == 2)
                {
                    
                    var argHelper0 = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                    var argHelper1 = new Puerts.ArgumentHelper((int)data, isolate, info, 1);
                    
                    
                    if (argHelper0.IsMatch(Puerts.JsValueType.NullOrUndefined | Puerts.JsValueType.String, null, false, false)
                        && argHelper1.IsMatch(Puerts.JsValueType.NullOrUndefined | Puerts.JsValueType.NativeObject, typeof(System.IFormatProvider), false, false))
                    {
                        
                        var Arg0 = argHelper0.GetString(false);
                        var Arg1 = argHelper1.Get<System.IFormatProvider>(false);
                        var result = (*obj).ToString(Arg0,Arg1);
                        
                        Puerts.PuertsDLL.ReturnString(isolate, info, result);
                        
                        return;
                    }
                }
                
                Puerts.PuertsDLL.ThrowException(isolate, "invalid arguments to ToString");
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_origin(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                var result = (*obj).origin;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_origin(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).origin = argHelper.Get<UnityEngine.Vector3>(false);
                
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void G_direction(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                var result = (*obj).direction;
                Puerts.ResultHelper.Set((int)data, isolate, info, result);
            }
            catch (Exception e)
            {
                Puerts.PuertsDLL.ThrowException(isolate, "c# exception:" + e.Message + ",stack:" + e.StackTrace);
            }
        }
        
        [Puerts.MonoPInvokeCallback(typeof(Puerts.V8FunctionCallback))]
        unsafe private static void S_direction(IntPtr isolate, IntPtr info, IntPtr self, int paramLen, long data)
        {
            try
            {
                var obj = (UnityEngine.Ray*)self;
                var argHelper = new Puerts.ArgumentHelper((int)data, isolate, info, 0);
                (*obj).direction = argHelper.Get<UnityEngine.Vector3>(false);
                
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
                    { new Puerts.MethodKey {Name = "GetPoint", IsStatic = false},  M_GetPoint },
                    { new Puerts.MethodKey {Name = "ToString", IsStatic = false},  M_ToString },
                    
                },
                Properties = new System.Collections.Generic.Dictionary<string, Puerts.PropertyRegisterInfo>()
                {
                    {"origin", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_origin, Setter = S_origin} },
                    {"direction", new Puerts.PropertyRegisterInfo(){ IsStatic = false, Getter = G_direction, Setter = S_direction} },
                    
                }
            };
        }
        
        unsafe private static UnityEngine.Ray StaticGetter(int jsEnvIdx, IntPtr isolate, Puerts.IGetValueFromJs getValueApi, IntPtr value, bool isByRef)
        {
            UnityEngine.Ray* result = (UnityEngine.Ray*)getValueApi.GetNativeObject(isolate, value, isByRef);
            return result == null ? default(UnityEngine.Ray) : *result;
        }

        unsafe private static void StaticSetter(int jsEnvIdx, IntPtr isolate, Puerts.ISetValueToJs setValueApi, IntPtr value, UnityEngine.Ray val)
        {
            HeapValue = val;
            fixed (UnityEngine.Ray* result = &HeapValue)
            {
                var typeId = Puerts.JsEnv.jsEnvs[jsEnvIdx].GetTypeId(typeof(UnityEngine.Ray));
                setValueApi.SetNativeObject(isolate, value, typeId, new IntPtr(result));
            }
        }
        
        public static void InitBlittableCopy(Puerts.JsEnv jsEnv)
        {
            Puerts.StaticTranslate<UnityEngine.Ray>.ReplaceDefault(StaticSetter, StaticGetter);
            int jsEnvIdx = jsEnv.Index;
            jsEnv.RegisterGeneralGetSet(typeof(UnityEngine.Ray), (IntPtr isolate, Puerts.IGetValueFromJs getValueApi, IntPtr value, bool isByRef) =>
            {
                return StaticGetter(jsEnvIdx, isolate, getValueApi, value, isByRef);
            }, (IntPtr isolate, Puerts.ISetValueToJs setValueApi, IntPtr value, object obj) => 
            {
                StaticSetter(jsEnvIdx, isolate, setValueApi, value, (UnityEngine.Ray)obj);
            });
        }
        
    }
}