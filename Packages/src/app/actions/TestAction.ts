import { App, GameEngine, UnityEngine } from 'csharp';
import { keys } from 'mobx';
import { $ } from 'types';
import { id } from 'Widget/id';
import TsActionTest = App.Actions.TsActionTest;
import Debug = UnityEngine.Debug;
import GameObject = UnityEngine.GameObject;
import Strings = GameEngine.Extensions.Strings;

export class TestAction extends TsActionTest {
    m_testProp: number = 10;
    static testStatic: number = 100;
    
    static staticFunc() {
        console.log('[test static]')
    }
    
    // $Init() {
    //     Debug.Log(Strings.ToBlue('init TestAction'))
    //     this.m_testProp = 100;
    // }
    
    //OnExecute(t:number);
    
    OnExecute() {
        Debug.Log('test ts');
        //this.testProp = 20;
        console.log('testProp=', this.m_testProp)
        console.log('testStatic=', TestAction.testStatic)
        $(id.test, tag => {
            Debug.Log(tag.name);
            //tag.gameObject.SetActive(false);
        })
    }
}

// let TA: any = TestAction;
// let N = new TA();
// for (const name of Object.keys(N)) {
//     console.log(name, N[name])
//     TsActionTest.prototype[name] = N[name];
// }

//
// class Describer {
//     private static FRegEx = new RegExp(/(?:this\.)(.+?(?= ))/g);
//     static describe(val: Function, parent = false): string[] {
//         var result = [];
//         if (parent) {
//             var proto = Object.getPrototypeOf(val.prototype);
//             if (proto) {
//                 result = result.concat(this.describe(proto.constructor, parent));
//             }
//         }
//         result = result.concat(val.toString().match(this.FRegEx) || []);
//         return result;
//     }
// }
//
// console.log(Describer.describe(TestAction)); // ["this.a1", "this.a2"]
//
// //Object.setPrototypeOf(TsActionTest, TestAction.prototype)
//
let ks = Object.getOwnPropertyNames(TestAction.prototype);
console.log(Strings.ToRed('test prototype'), ks)
console.log(Object.getOwnPropertyNames(TestAction))
let a: any = TestAction;
a['staticFunc']();

(function <T>(a: new (...args: any[]) => T) {
    a['staticFunc']();
})(TestAction);

// // let pr = Object.getPrototypeOf(TsActionTest.prototype)
// // for (const name in ks) {
// //     pr[name] = TsActionTest.prototype[name];
// // }