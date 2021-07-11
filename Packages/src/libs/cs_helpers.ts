import { Common, NodeCanvas, System, UnityEngine, Utils } from 'csharp';
import { $generic, $typeof } from 'puerts';
import Dictionary$2 = System.Collections.Generic.Dictionary$2;
import String = System.String;
import JSAdapter = Common.JSAdapter;
import Helpers = Common.JSAdapter.sqlite3.Helpers;

export function isDefined<T>(value: T | undefined | null): value is T {
    return <T> value !== undefined && <T> value !== null && (!(<T> value instanceof UnityEngine.Object) || !(<T> value as unknown as UnityEngine.Object)?.Equals(null));
}

export function Dictionary<TKey, TValue>(k: new() => TKey, v: new() => TValue): Dictionary$2<TKey, TValue> {
    return Helpers.MakeDictionary($typeof(k), $typeof(v));
}

/**
 * https://allknowboy.com/posts/576eb6bf/#TS%E4%B8%AD%E7%9A%84%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F
 * @returns {SingletonTemp}
 * @constructor
 */
export function Singleton<T>() {
    class SingletonTemp {
        private static _instance: any;
        protected constructor() {}
        static getInstance() : T {
            if (!SingletonTemp._instance) {
                SingletonTemp._instance = new this();
            }
            return SingletonTemp._instance as T;
        }
    }
    return SingletonTemp;
}

