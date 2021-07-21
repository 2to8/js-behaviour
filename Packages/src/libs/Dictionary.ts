import { System } from 'csharp';
import { $generic, $typeof } from 'puerts';
import './cs_presets'
import { Item } from 'Widget/Item';
import Dictionary$2 = System.Collections.Generic.Dictionary$2;
import List$1 = System.Collections.Generic.List$1;
// #region Dictionary<TKey,TValue> 扩展, 替换 $generic 方法
const srcGeneric = $generic;
// cache types
let list = new Array<System.Type>();

globalThis.instances = globalThis.instances || new Set();

globalThis.I = globalThis.I || function <T>(c: { new(...args: any[]): T; }, ...args: any[]): T {
    if (!globalThis.instances.has(c)) {
        globalThis.instances.set(c, args[0] instanceof c ? args[0] : new c(...args));
    }
    return globalThis.instances.get(c);
}

function cache(type: System.Type) {
    if (!type) return false;
    for (let i = 0; i < list.length; i++) {
        if (type.Equals(list[i])) return false;
    }
    list.push(type);
    return true;
}

// overwrite generic method
export function generic<T extends new (...args: any[]) => any>(genericType: T, ...genericArguments: (new (...args: any[]) => any)[]): T {
    let Class = srcGeneric(genericType, ...genericArguments);
    let type = $typeof(Class);
    if (genericArguments.length == 2 && cache(type)) {
        try {
            let dictType = $typeof(srcGeneric(System.Collections.Generic.Dictionary$2, ...genericArguments));
            if (dictType && dictType.IsAssignableFrom(type)) {
                Class.prototype['forEach'] = function(callbackfn: (v: any, k: any) => void | boolean) {
                    let iterator = this.Keys.GetEnumerator();
                    while (iterator.MoveNext()) {
                        let key = iterator.Current;
                        let ret = callbackfn(this.get_Item(key), key);
                        if (ret !== void 0 && !ret) break;
                    }
                }
                Class.prototype['getKeys'] = function() {
                    let result = [];
                    let iterator = this.Keys.GetEnumerator();
                    while (iterator.MoveNext()) {
                        result.push(iterator.Current);
                    }
                    return result;
                }
                Class.prototype['getValues'] = function() {
                    let result = [];
                    let iterator = this.Values.GetEnumerator();
                    while (iterator.MoveNext()) {
                        result.push(iterator.Current);
                    }
                    return result;
                }
                Class.prototype[Symbol.iterator] = function() {
                    let values = [];
                    let iterator = this.Keys.GetEnumerator();
                    while (iterator.MoveNext()) {
                        let key = iterator.Current;
                        values.push({ key: key, value: this.get_Item(key) });
                    }
                    return {
                        index: 0, values: values, next() {
                            let index = this.index++;
                            return {
                                value: this.values[index], done: index >= this.values.length,
                            };
                        },
                    };
                }
            }
        } catch (e) {
            console.warn(e);
        }
    }
    return Class as T;
}

// replace method
(function() {
    let puerts = globalThis['puerts'];
    if (puerts && puerts['$generic'].toString() !== generic.toString()) {
        puerts['$generic'] = generic;
        puerts['$srcGeneric'] = srcGeneric;
    } else {
        console.log("puerts reloaded")
        //throw new Error('puerts is undefined or redefinition \'$generic\'');
    }
})();
// #endregion

// #region Uint8Array 与 byte[] 类型互转
export function Uint8ArrayToBytes(data: Uint8Array): System.Array$1<number> {
    let result = System.Array.CreateInstance($typeof(System.Byte), data.length) as System.Array$1<number>;
    for (let i = 0; i < result.Length; i++) {
        result.set_Item(i, data[i]);
    }
    return result;
}

export function BytesToUint8Array(data: System.Array$1<number>): Uint8Array {
    let result = new Uint8Array(data.Length);
    for (let i = 0; i < data.Length; i++) {
        result[i] = data.get_Item(i);
    }
    return result;
}

export function fixList<T extends System.Object>(Class: List$1<T>, c?: new(...args: any[]) => T): List$1<T> {
    if (Class == null && c != null) {
        let List = $generic(List$1, c);
        Class = new List<T>() as any
    }
    if (!Class.hasOwnProperty('forEach')) {
        Object.defineProperty(Class, 'forEach', {
            value: function(fn: (value: T, k?: number) => void | boolean) {
                let iterator = this.GetEnumerator();
                let i = -1;
                while (iterator.MoveNext()) {
                    let key = iterator.Current;
                    let ret = fn(key/*this.get_Item(i += 1)*/, i+=1);
                    if (ret !== void 0 && !ret) break;
                }
            },
        });
    }
    if (!Class.hasOwnProperty('array')) {
        Object.defineProperty(Class, 'array', {
            get: function() {
                let result = [];
                let iterator = this.GetEnumerator();
                while (iterator.MoveNext()) {
                    result.push(iterator.Current);
                }
                return result;
            },
        });
    }
    Object.defineProperty(Class, Symbol.iterator, {
        value: function() {
            let values = [];
            let iterator = this.GetEnumerator();
            while (iterator.MoveNext()) {
                let key = iterator.Current;
                values.push({ key: key, value: this.get_Item(key) });
            }
            return {
                index: 0, values: values, next() {
                    let index = this.index++;
                    return {
                        value: this.values[index], done: index >= this.values.length,
                    };
                },
            }
        },
    });
    return Class;
}

export function fixDict<TKey, TValue>(Class: Dictionary$2<TKey, TValue>, TK?: new(...args: any[]) => TKey, TV?: new(...args: any[]) => TValue): Dictionary$2<TKey, TValue> {
    if (Class == null && TK != null && TV != null) {
        while ($typeof(TK).IsGenericType) {
            TK = $generic(TK);
        }
        while ($typeof(TV).IsGenericType) {
            TV = $generic(TV);
        }
        let Dict = $generic(Dictionary$2, TK, TV);
        Class = new Dict<TKey, TValue>() as any
    }
    if (!Class.hasOwnProperty('forEach')) {
        Object.defineProperty(Class, 'forEach', {
            value: function(fn: (v: any, k: any) => void | boolean) {
                let iterator = this.Keys.GetEnumerator();
                while (iterator.MoveNext()) {
                    let key = iterator.Current;
                    let ret = fn(this.get_Item(key), key);
                    if (ret !== void 0 && !ret) break;
                }
            },
        });
    }
    
    if (!Class.hasOwnProperty('keys')) {
        Object.defineProperty(Class, 'keys', {
            get: function() {
                let result = [];
                let iterator = this.Keys.GetEnumerator();
                while (iterator.MoveNext()) {
                    result.push(iterator.Current);
                }
                return result;
            },
        });
    }
    
    if (!Class.hasOwnProperty('values')) {
        Object.defineProperty(Class, 'values', {
            get: function() {
                let result = [];
                let iterator = this.Values.GetEnumerator();
                while (iterator.MoveNext()) {
                    result.push(iterator.Current);
                }
                return result;
            },
        });
    }
    
    if (!Class.hasOwnProperty('map')) {
        Object.defineProperty(Class, 'map', {
            get: function(): Map<TKey, TValue> {
                let result = new Map<TKey, TValue>();
                let iterator = this.Keys.GetEnumerator();
                let v = this.Values.GetEnumerator();
                while (iterator.MoveNext() && v.MoveNext()) {
                    result.set(iterator.Current, v.Current);
                }
                return result;
            },
        });
    }
    
    Object.defineProperty(Class, Symbol.iterator, {
        value: function() {
            let values = [];
            let iterator = this.Keys.GetEnumerator();
            while (iterator.MoveNext()) {
                let key = iterator.Current;
                values.push({ key: key, value: this.get_Item(key) });
            }
            return {
                index: 0, values: values, next() {
                    let index = this.index++;
                    return {
                        value: this.values[index], done: index >= this.values.length,
                    };
                },
            }
        },
    });
    return Class;
}

//   fixDict(Dictionary$2.prototype)

/**
 * 属性装饰器
 * @param {Item | (typeof Item | typeof Item[])[]} p
 * @returns {any}
 */
export function find(...p: (typeof Item | (typeof Item | (typeof Item)[])[])[]) {
    
    /**
     *   属性装饰器固定形式, 返回void, 第一个参数是属性的对象, 第二个是属性名
     *   target[propertyName] 设置属性值
     *   https://blog.csdn.net/ZZB_Bin/article/details/103168609
     */
    return (target: any, propertyName: string) => {
        let value = null;
        target[propertyName] = value;
        // 在类的原型属性 'someMethod' 上定义元数据，key 为 `methodMetaData`，value 为 `b`
        // Reflect.defineMetadata('methodMetaData', 'b', target, key);
    };
}

export function dict_install() {
    //(function() {
    let csharp = globalThis['puerts'];
    csharp['Uint8ArrayToBytes'] = Uint8ArrayToBytes;
    csharp['BytesToUint8Array'] = BytesToUint8Array;
    //})()
}

//#endregion