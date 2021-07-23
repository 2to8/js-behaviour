import { TestAction } from 'app/actions/TestAction';
import { TTestFindNode } from 'app/actions/TTestFindNode';
import { DebugText } from 'app/graph/DebugText';
import { TConcurrentState } from 'app/graph/TConcurrentState';
import { TNode } from 'app/graph/TNode';
import extensions from 'extensions';
import { dict_install } from 'libs/Dictionary';
import { $extension, $typeof } from 'puerts';
import TestBind from 'sandbox/TestBind';
import { GameEngine, MoreTags, NodeCanvas, Puerts, PuertsStaticWrap, Sandbox, System, UnityEditor, UnityEngine } from 'csharp';
import { Component } from 'component/component-base';
import { component, property } from 'component/component-decoration';
import { uses } from 'support/utils';
import { $ } from 'types';
import { SystemBehaviour1, SystemBehaviour2, SystemBehaviour3, TestBehaviour, UserBehaviour1, UserBehaviour2, UserBehaviour3 } from 'views/TestBehaviour';
import TestScript = Sandbox.TestScript;
import Debug = UnityEngine.Debug;
import TestCs2Ts = Sandbox.TestCs2Ts;

export * from './component/component-info-mgr';
export * from './component/component-inst-mgr';

import EcsInit from 'EcsInit';
import { id } from 'Widget/id';
import { st } from 'Widget/st';
import JsEnv = Puerts.JsEnv;
import Strings = GameEngine.Extensions.Strings;
import UI = UnityEngine.UI;
import GameObject = UnityEngine.GameObject;
import TagSystem = MoreTags.TagSystem;
import Node = NodeCanvas.Framework.Node;
import Process = System.Diagnostics.Process;
import Tags = MoreTags.Tags;
import TestJs = Sandbox.TestJs;
import Int32 = System.Int32;
import Convert = System.Convert;

global.$hello = (s: string) => {
    Debug.Log(`hello, ${ s }`)
}

globalThis.OnTagStart = (tags: Tags) => {
    
}

// TestCs2Ts.prototype.test = function() {
//     Debug.Log(this.num + ' = test')
//     this.num = 123;
// }

global.$testPrototype = function() {
    const p = Object.getOwnPropertyNames(TestBind.prototype);
    console.log(p); // [ 'constructor', 'echo', 'info' ]
    p.forEach(name => {
        Debug.Log(name);
        TestCs2Ts.prototype[name] = TestBind.prototype[name];
    });
}

// for (let i in p) {
//     let name = p[i];
//   
// }

extensions();
//$extension(Node, TagSystem)

dict_install();

export function Singleton<E>() {
    class SingletonE {
        protected constructor() {
        }
        
        private static _inst: SingletonE = null;
        public static get inst(): E {
            if (SingletonE._inst == null) {
                SingletonE._inst = new this();
            }
            return SingletonE._inst as E;
        }
    }
    
    return SingletonE;
}

global.$InitEnv = (env: JsEnv) => {
    PuertsStaticWrap.AutoStaticUsing.AutoUsing(env);
    //PuertsHelper.UsingActions(env);
}

global.$providers = new Map<string, any>();
global.$require = (obj: System.Object, fn: string, ...args: any[]) => {
    if (!global.$providers.has(obj.GetType().FullName)) {
        Debug.Log(Strings.ToBlue(`${ obj.GetType().FullName } 没有添加到 module 列表`))
        // 这个不检查父类是否已经添加, 不需要return
        //return;
    }
    obj['$Init']?.call(obj);
    obj[fn]?.call(obj, ...args);
    
}

//TestCs2Ts.prototype['test2'] = TestBind.prototype['test2'];

// GameObject.Find("test").transform.do

global.$testBind = (obj: TestBind) => {
    Debug.Log('test bind 5');
    Debug.Log(obj.num);
    obj.num = 5;
    obj.test2();
}

Array.prototype['toArray'] = function <T1 extends System.Object | any>(type: new (...args: any[]) => T1): System.Array$1<T1> {
    let ret = System.Array.CreateInstance($typeof(type), this.length) as System.Array$1<T1>;
    for (let i = 0; i < this.length; i++) {
        ret.set_Item(i, this[i]);
    }
    return ret;
}

TestJs.TestArray([ 1, 2, 3 ].toArray(Int32));

$([ id.Pause ], [ st.game.GameOver, [ st.game.Pause ] ]);

uses(   //
    TNode,//
    TTestFindNode,//
    TConcurrentState,//
    TestBind,//
    TestAction,//
    DebugText,//
    TestBehaviour, //
    UserBehaviour1, UserBehaviour2, UserBehaviour3,  //
    SystemBehaviour1, SystemBehaviour2, SystemBehaviour3, //
);

