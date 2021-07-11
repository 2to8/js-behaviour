
// export namespace UnityEngine.UI.Button {
//     class ButtonClickedEvent extends UnityEngine.Events.UnityEvent {
//         RemoveAllListener();
//         public constructor();
//
//     }
//
// }


// export namespace System {
//     interface Array {
//         toArray(): any[]
//     }
//    
//     interface Array$1<T> extends System.Array {
//         get_Item(index: number):T;
//         set_Item(index: number, value: T):void;
//         toArray():T[];
//         [Symbol.iterator](): {
//             next(): {
//                 value: { key: number, value: T }, done: boolean
//             };
//         };
//     }
// }

//@ts-nocheck

import { T } from 'csharp';

export namespace System {
    interface Array {
        toArray(): any[]
        [Symbol.iterator](): Generator<any>
    }
    
    interface Array$1<T> extends System.Array {
        get_Item(index: number):T;
        set_Item(index: number, value: T):void;
        toArray():T[];
        [Symbol.iterator](): Generator<T>
        //     {
        //     next(): {
        //         value: { key: number, value: T }, done: boolean
        //     };
        // };
    }
}

export namespace UnityEngine {
    interface GameObject {
        GetInChild<T extends UnityEngine.Component>($type: new (...args :any[]) =>T , $includeInactive?: boolean):System.Array$1<T>;
    }
}

export namespace System.Collections.Generic {
    
    interface List$1<T>{
        
        forEach(fn: (v: T, k?: number) => boolean | void): void;
        
        toArray: T[];
        
        [Symbol.iterator](): {
            next(): {
                value: { key: number, value: T }, done: boolean
            };
        };
        
    }

    interface Dictionary$2<TKey, TValue> {
        /**
         * 遍历Dictionary$2字典
         * @param fn
         */
        forEach(fn: (v: TValue, k?: TKey) => boolean | void): void;

        /**
         * Key集合
         */
        keys: TKey[];

        /**
         * Value集合
         */
        values: TValue[];

        map: Map<TKey, TValue>;

        /**
         * 实现for迭代器接口
         */
        [Symbol.iterator](): {
            next(): {
                value: { key: TKey, value: TValue }, done: boolean
            };
        };
    }
}

export namespace UnityEngine {
    interface Transform {
        [Symbol.iterator](): Generator<Transform, void, unknown>;
    }
    interface GameObject {
        [Symbol.iterator](): Generator<Transform, void, unknown>; 
    }
}


