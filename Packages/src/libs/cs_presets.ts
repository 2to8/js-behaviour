// https://stackoverflow.com/questions/42088007/is-there-source-map-support-for-typescript-in-node-nodemon
// import sourceMapSupport from 'source-map-support'

//import "node-window-polyfill/register";

import { System,  Unity, UnityEngine } from 'csharp';
import { $extension, $typeof, emit, off, on } from 'puerts';
import { isDefined } from './cs_helpers';
import List$1 = System.Collections.Generic.List$1;
import * as puerts from 'puerts';
import GameObject = UnityEngine.GameObject;
import Transform = UnityEngine.Transform;

global.process = global.process || {} as any
process.env = process.env || {} as any
if (!process.on) {
    Object.assign(process, {
        on: on, off: off, emit: emit,
    })
}

process.on('unhandledRejection', console.log);

UnityEngine.Object.prototype.valueOf = function() {
    return this.Equals(null) ? null : this;
}

UnityEngine.GameObject.prototype['GetInChild'] = 
    function <T extends UnityEngine.Component>($type: new (...args: any[]) => T, $includeInactive?: boolean):
        System.Array$1<T> {
    if (!($type instanceof System.Type)) {
        $type = $typeof($type) as any;
    }
    return (this as GameObject).GetComponentsInChildren($type as any) as any;
}

System.Array.prototype[Symbol.iterator] = function* () {
    let _this = this as System.Array;
    for (let i = 0; i < _this.Length; i++) {
        yield _this.GetValue(i)
    }
}

// Object.defineProperty(System.Array.prototype,Symbol.iterator,function() {
//     let values = [];
//     let iterator = this.GetEnumerator();
//     while (iterator.MoveNext()) {
//         let key = iterator.Current;
//         values.push({ key: key, value: this.get_Item(key) });
//     }
//     return {
//         index: 0, values: values, next() {
//             let index = this.index++;
//             return {
//                 value: this.values[index], done: index >= this.values.length,
//             };
//         },
//     }
// } )

UnityEngine.Transform.prototype[Symbol.iterator] = function() {
    let target = this;
    return (function* () {
        for (let i = 0; i < target.childCount; i++) {
            yield target.GetChild(i);
        }
        
    })();
}

// $extension(GameObject, TetrisUtil);
// $extension(Transform, TetrisUtil);

UnityEngine.GameObject.prototype[Symbol.iterator] = function() {
    let target = this.transform;
    return (function* () {
        for (let i = 0; i < target.childCount; i++) {
            yield target.GetChild(i);
        }
    })();
}

Object.defineProperty(System.Array.prototype, 'toArray', {
    enumerable: true, writable: true, value: function(): any[] {
        let arr = [];
        for (let i = 0n; i < this.Length; i++) {
            arr.push(this.GetValue(i))
        }
        return arr;
    },
})

if (List$1?.prototype != null) {
    Object.defineProperty(List$1.prototype, 'toArray', {
        enumerable: true, writable: true, value: function(): any[] {
            let arr = [];
            for (let i = 0; i < this.Count(); i++) {
                arr.push(this.get_Item(i))
            }
            return arr;
        },
    })
}

Object.defineProperty(Object.prototype, 'tap', {
    value: function(intercept) {
        var val = (this instanceof Number || this instanceof String || this instanceof Boolean || this instanceof UnityEngine.Object) ? this.valueOf() : this;
        intercept(val);
        return val;
    }, enumerable: true, configurable: true, writable: true,
});

// https://gist.github.com/getify/9104721
if (!(Symbol.iterator in Object.prototype)) {
    Object.defineProperty(Object.prototype, Symbol.iterator, {
        enumerable: true, writable: true, configurable: true, // thanks to:
                                                              // https://twitter.com/juandopazo/status/436298238464122880
        value: function* () {
            var o = this;
            var ks = Object.keys(o);
            for (var idx = 0, length = ks.length; idx < length; idx++) {
                yield o[ks[idx]];
            }
        },
    });
}

// Object.prototype.tap = function(intercept){
//     var val = (this instanceof Number || this instanceof String || this instanceof Boolean ||
// this instanceof UnityEngine.Object) ? this.valueOf() : this; intercept(val); return val; }

// See if an array contains an object
Array.prototype.contains = function(obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

// Global declaration
//declare var IsDefined: typeof isDefined;

// Global scope augmentation
var window = window || null;
const _global = (window || global) as any;
_global.IsDefined = isDefined;

puerts.registerBuildinModule('path', {
    dirname(path) {
        return System.IO.Path.GetDirectoryName(path);
    }, resolve(dir, url) {
        url = url.replace(/\\/g, '/');
        while (url.startsWith('../')) {
            dir = System.IO.Path.GetDirectoryName(dir);
            url = url.substr(3);
        }
        return System.IO.Path.Combine(dir, url);
    },
});
puerts.registerBuildinModule('fs', {
    existsSync(path) {
        return System.IO.File.Exists(path);
    }, readFileSync(path) {
        return System.IO.File.ReadAllText(path);
    },
});
(function() {
    let global = this ?? globalThis;
    global['Buffer'] = global['Buffer'] ?? {};
})();
//sourceMapSupport.install();

//import 'source-map-support/register'



