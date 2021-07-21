// declare module 'puerts' {
//     import { $Ref, $Task, System } from 'csharp'
//    
//     function $ref<T>(x?: T): $Ref<T>;
//    
//     function $unref<T>(x: $Ref<T>): T;
//    
//     function $set<T>(x: $Ref<T>, val: T): void;
//    
//     function $promise<T>(x: $Task<T>): Promise<T>;
//    
//     function $generic<T extends new (...args: any[]) => any>(genericType: T,
// ...genericArguments: (new (...args: any[]) => any)[]): T;  function $typeof(x: new (...args:
// any[]) => any): System.Type;  function $extension(c: Function, e: Function): void;  function
// on(eventType: string, listener: Function, prepend?: boolean): void;  function off(eventType:
// string, listener: Function): void;  function emit(eventType: string, ...args: any[]): boolean; }
//  declare function require(name: string): any; 

// ==========================================
// 注意!注意!注意!
// *.d.ts 顶层不能出现 import, 否则会出问题
// ==========================================

declare module 'puerts' {
    import { $Ref, $Task, System } from 'csharp';
    //const React: React;
    // const Component: React.Component;
    // type ReactNode = React.ReactNode;
    
    function registerBuildinModule(name: string, module: any)
    
    function $ref<T>(x?: T): $Ref<T>;
    
    function $unref<T>(x: $Ref<T>): T;
    
    function $set<T>(x: $Ref<T>, val: T): void;
    
    function $promise<T>(x: $Task<T>): Promise<T>;
    
    function $generic<T extends new (...args: any[]) => any>(genericType: T, ...genericArguments: (new (...args: any[]) => any)[]): T;
    
    function $typeof(x: new (...args: any[]) => any): System.Type;
    
    function $extension(c: Function, e: Function): void;
    
    function on(eventType: string, listener: Function, prepend?: boolean): void;
    
    function off(eventType: string, listener: Function): void;
    
    function emit(eventType: string, ...args: any[]): boolean;
    
    /**
     * 将Uint8Array类型转为C# byte[]类型
     * @param data
     */
    export function Uint8ArrayToBytes(data: Uint8Array): System.Array$1<number>;
    
    /**
     * 将C# byte[]类型转为Uint8Array类型
     * @param data
     */
    export function BytesToUint8Array(data: System.Array$1<number>): Uint8Array;
    
}

// declare var require: {
//     <T>(path: string): T; (paths: string[], callback: (...modules: any[]) => void): void;
// ensure: (paths: string[], callback: (require: <T>(path: string) => T) => void) => void; };

//declare function require(name: string): any;

//declare var t:any;
declare interface Object {
    tap<T, U>(_: (fn: T) => U | any): T | U;
    
    tap<T>(_: (fn: T) => any): T;
    
    //tap<T extends any>(intercept: T): T;
    
    //tap: (intercept: any) => any;
}

//declare global {
// import { System } from 'csharp';

// declare interface Array {
//     contains: (obj: any) => boolean;
//    
//     toArray<T1 extends System.Object>(type: new (...args: any[]) => T1): System.Array$1<T1>
// }

declare interface Array<T> {
    contains: (obj: any) => boolean;
    
    toArray<T1 extends csharp.System.Object>(type: new (...args: any[]) => T1): csharp.System.Array$1<T1>
}

//}

//
// declare namespace UnityEngine {
//     //import {  System } from 'csharp'
//     // import ValueType = System.ValueType;
//
//     export class Vector3 {
//         public constructor();
//     }
// }

//declare function require(name: string): any;

declare var Chart: any;

declare function I<T>(c: { new(...args: any): T; }, ...args: any): T

//declare function require(name: string): any;

// declare var require: {
//   <T>(path: string): T;
//   (paths: string[], callback: (...modules: any[]) => void): void;
//   ensure: (
//     paths: string[],
//     callback: (require: <T>(path: string) => T) => void
//   ) => void;
// };

// declare module "*.json" {
//   const value: any;
//   export default value;
// }
// declare module "*.png";
// declare module "*.jpg";

// ==========================================
// 注意!注意!注意!
// *.d.ts 顶层不能出现 import, 否则会出问题
// ==========================================
declare module '*.prefab' {
    import PrefabRes from 'Prefabs/Resource/PrefabRes';
    const res: PrefabRes;
    export default res;
}

declare module '*!prefab' {
    import PrefabRes from 'Prefabs/Resource/PrefabRes';
    const res: PrefabRes;
    export default res;
}

declare module 'prefab!*' {
    import PrefabRes from 'Prefabs/Resource/PrefabRes';
    const res: PrefabRes;
    export default res;
}

// ==========================================
// 注意!注意!注意!
// *.d.ts 顶层不能出现 import, 否则会出问题
// ==========================================
declare module '*.asset' {
    import AssetRes from 'Prefabs/Resource/AssetRes';
    const assetRes: AssetRes;
    export default assetRes;
}
declare module '*.svg';
declare module '*.png' {
    const content: any;
    export default content;
}

declare type Type<T> = { new(...args: any[]): T };

declare module '*.jpg';
declare module '*.jpeg';
declare module '*.gif';
declare module '*.bmp';
declare module '*.tiff';

declare module '*.unity' {
    import SceneRes from 'Prefabs/Resource/SceneRes';
    const assetRes: SceneRes;
    export default assetRes;
}

interface DBConnection {
    open(force: boolean): DBConnection;
    
    instance<T>(c: Type<T>): T;
    
    open(): DBConnection;
    
    table<T extends object>(type: Type<T>)
}

interface GlobalData {
    lastUsername: string;
}

interface UserData {
    money: number;
    coin: number;
    diamond: number;
    health: number;
    healthTotal: number;
    levelPoint: number;
    levelPointTotal: number;
    userLevel: number;
}

interface LevelItem {
    
}

interface LevelData {
    data: LevelItem[][];
    width: number;
    height: number;
}

interface StageData {
    
}

declare var config: GlobalData;
declare var db: DBConnection;
declare var app: DBConnection;
declare var user: UserData;
declare var tables: Map<any, any>;
declare var gKey: number;
declare var level: LevelData;
declare var stage: StageData;

// declare module 'csharp' {
//     export namespace System.Collections.Generic {
//
//         interface Dictionary$2<TKey, TValue> {
//             /**
//              * 遍历Dictionary$2字典
//              * @param fn
//              */
//             forEach(fn: (v: TValue, k?: TKey) => boolean | void): void;
//
//             /**
//              * Key集合
//              */
//             keys: TKey[];
//
//             /**
//              * Value集合
//              */
//             values: TValue[];
//
//             map: Map<TKey, TValue>;
//
//             /**
//              * 实现for迭代器接口
//              */
//             [Symbol.iterator](): {
//                 next(): {
//                     value: { key: TKey, value: TValue }, done: boolean
//                 };
//             };
//         }
//     }
// }















