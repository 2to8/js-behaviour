// import { App } from '@/App';
// import { HostConfig } from '@/config/HostConfig';
// import { id } from '@/Widget/id';
// import { PuertsTest,  UnityEngine } from 'csharp';
// import 'libs/cs_presets'
// import { dict_install } from 'libs/Dictionary'
// import { $typeof } from 'puerts';
// import React from 'react';
// import ComponentMap from 'Components/ComponentMap'
// import Reconciler from 'react-reconciler';
// import { UnityWidgetRoot } from 'Components/UnityWidgetRoot';
// import Camera = UnityEngine.Camera;
//
// dict_install();
//
//
//
// class JsAction extends Set {
//   set Add(action:(...args:any) => void){
//      this.add(action)
//   }
//   Invoke(){
//     this.forEach(c => (c as Function)?.call(this));
//   }
// }
// // I(PuertsTest.DerivedClass).
//
// let act = new JsAction();
// act.Add = () => {}
// act.Invoke();
//
// // var t = Camera.main.GetComponent($typeof(Client)) as Client;
// //
// //
// // var callback = new Set();
// //  callback.add(()=> {
// //
// //  })
// // callback.forEach(c => (c as Function)?.call(null))
//
// const patch = (ns: object, path?: string) => {
//     Object.keys(ns).forEach((key) => {
//         const value = ns[key];
//         const currentPath = path ? `${path}.${key}` : key;
//         if (typeof value === 'object') {
//             patch(value, currentPath);
//         }
//         if (typeof value === 'function') {
//             Object.defineProperty(value, 'name', {
//                 value: currentPath,
//                 configurable: true,
//             });
//         }
//     })
// }
//
// patch(id, "id")
//
// export default function(router: RouteProxy) {
//     console.log(`[components size]: ${ ComponentMap.size }`);
//     ComponentMap.forEach((value, key) => {
//         //patch(key)
//         console.log(`${key.name}`, value)
//         router.setComponentMap(`${key.name}`, value);
//     })
//    
//     if (router.init()) {
//         const reconciler = Reconciler(HostConfig as any)
//         let root = new UnityWidgetRoot(router.root);
//         const container = reconciler.createContainer(root, false, false, null);
//         // reconciler.updateContainer(<UIRoot/>, container, null, () => {
//         //
//         // });
//        
//         reconciler.updateContainer(<App router={ router }/>, container, null, () => {
//        
//         });
//        
//         globalThis['reconciler'] = reconciler;
//         globalThis['router'] = router;
//     }
//    
// }
