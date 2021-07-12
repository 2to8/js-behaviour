import { TestAction } from 'app/actions/TestAction';
import { DebugText } from 'app/graph/DebugText';
import extensions from 'extensions';
import { dict_install } from 'libs/Dictionary';
import TestBind from 'sandbox/TestBind';
import { Puerts, PuertsStaticWrap, Sandbox, System, UnityEngine } from 'csharp';
import { Component } from 'component/component-base';
import { component, property } from 'component/component-decoration';
import { uses } from 'support/utils';
import { SystemBehaviour1, SystemBehaviour2, SystemBehaviour3, TestBehaviour, UserBehaviour1, UserBehaviour2, UserBehaviour3 } from 'views/TestBehaviour';
import TestScript = Sandbox.TestScript;
import Debug = UnityEngine.Debug;
import TestCs2Ts = Sandbox.TestCs2Ts;

export * from './component/component-info-mgr';
export * from './component/component-inst-mgr';

import EcsInit from 'EcsInit';
import JsEnv = Puerts.JsEnv;
import PuertsHelper = PuertsStaticWrap.PuertsHelper;


global.$hello = (s: string) => {
    Debug.Log(`hello, ${ s }`)
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

dict_install();

global.$InitEnv = (env: JsEnv) => {
    PuertsHelper.UsingActions(env);
}

global.$providers = new Map<string, any>();
global.$require = (obj: System.Object, fn: string, ...args: any[]) => {
    if (!global.$providers.has(obj.GetType().FullName)) {
        Debug.LogError(`${ obj.GetType().FullName } 没有添加到 module 列表`)
        return;
    }
    obj[fn].call(obj, ...args);
}

//TestCs2Ts.prototype['test2'] = TestBind.prototype['test2'];

global.$testBind = (obj: TestBind) => {
    Debug.Log('test bind 5');
    Debug.Log(obj.num);
    obj.num = 5;
    obj.test2();
}

uses(   //
    TestBind,//
    TestAction,//
    DebugText,//
    TestBehaviour, //
    UserBehaviour1, UserBehaviour2, UserBehaviour3,  //
    SystemBehaviour1, SystemBehaviour2, SystemBehaviour3, //
);

