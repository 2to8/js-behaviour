import { Dots } from '@/Components/Entity';
//import { ReactSystem } from '@/Manager/ReactSystem';
import { el } from '@/Widget/el';
import { Item } from '@/Widget/Item';
import { GameEngine, MoreTags, System, Unity, UnityEngine } from 'csharp';
import { fixList } from 'libs/Dictionary';
import { ReactSystem } from 'Manager/ReactSystem';
import { val } from 'objection';
//import { type } from 'os';
import { $typeof } from 'puerts';
import React from 'react';
//import { debug } from 'webpack';
import { id } from 'Widget/id';
import { st } from 'Widget/st';
import Entity = Dots.Entity;
//import ItemFilter = ReactSystem.ItemFilter;
import MonoBehaviour = UnityEngine.MonoBehaviour;
import Component = UnityEngine.Component;
import GameObject = UnityEngine.GameObject;
import ItemQuery = MoreTags.TagHelpers.ItemQuery;
//import SystemBase = ReactSystem.SystemBase;
import FilterType = MoreTags.TagHelpers.FilterType;
import TagSystem = MoreTags.TagSystem;
import TagQuery = MoreTags.TagQuery;
import TagQueryItem = MoreTags.TagQueryItem;
import Tags = MoreTags.Tags;
import Button = UnityEngine.UI.Button;
import Transform = UnityEngine.Transform;
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import Transforms = GameEngine.Extensions.Transforms;
import ItemFilter = ReactSystem.ItemFilter;

export type ALL = Tags | Button | Component /*| IComponentData */ | Item | GameObject
    | React.Component;

// type QLink = typeof  $(p: (typeof Widget)[] | filter, _: (e: any,...params: any[]) => any): any
// {}

const patch = (ns: object, path?: string) => {
    Object.keys(ns).forEach((key) => {
        const value = ns[key];
        const currentPath = path ? `${ path }.${ key }` : key;
        if (typeof value === 'object') {
            patch(value, currentPath);
        }
        if (typeof value === 'function') {
            Object.defineProperty(value, 'name', {
                value: currentPath, configurable: true,
            });
        }
    })
}

patch(id, 'Id')
patch(st, 'St')

export class FindCheck {
    type: System.Type
    
    // public isItemFilter(value: any): value is ItemFilter {
    //     if (value instanceof Array) {
    //         return false;
    //         // value.forEach(function(item) { // maybe only check first value?
    //         //     if (typeof item !== 'string') {
    //         //         return false
    //         //     }
    //         // })
    //         // return true
    //     }
    //     if (value.all || value.any || value.none) {
    //         return true;
    //     }
    //     return false
    // }
    
    public isItemType(value: any): value is Item {
        if (value instanceof Item || typeof (value as Item)?.name == 'string') {
            return true;
            // value.forEach(function(item) { // maybe only check first value?
            //     if (typeof item !== 'string') {
            //         return false
            //     }
            // })
            // return true
        }
        return false
    }
    
    public isStringArray(value: any): value is string[] {
        let res = value instanceof Array;
        if (res) {
            // @ts-ignore
            value.forEach(item => { // maybe only check first value?
                if (typeof item !== 'string') {
                    res = false
                }
            })
            //return true
        }
        return res
    }
    
    public isItemArray(value: any): value is Item[] {
        let res = value instanceof Array
        if (res) {
            // @ts-ignore
            value.forEach(item => { // maybe only check first value?
                if (item instanceof Item || typeof (item as Item)?.name == 'string') {
                    //
                } else {
                    res = false
                }
            })
        }
        return res
    }
    
    public isItemArrayArray(value: any): value is (Item[])[] {
        let res = value instanceof Array
        if (res) {
            value.forEach(function(item) { // maybe only check first value?
                if (item instanceof Array) {
                    // @ts-ignore
                    item.forEach(sub => { // maybe only check first value?
                        
                        if (sub instanceof Item || typeof (sub as Item)?.name == 'string') {
                            //return true
                        } else {
                            res = false
                        }
                    })
                } else {
                    res = false
                }
            })
        }
        return res
    }
    
    public isComponent(value: any): value is System.Type {
        if (value != null && $typeof(value)?.IsAssignableFrom($typeof(Component))) {
            return true
        }
        return false
    }
    
    public isGameObject(value: any): value is GameObject {
        if (value != null && $typeof(value)?.IsAssignableFrom($typeof(GameObject))) {
            return true
        }
        return false
    }
    
    public isDelegate(value: any): value is Function {
        if (typeof value == 'function') {
            return true
        }
        return false
    }
    
    getReturnType<R>(fn: (...args: any[]) => R): R {
        return {} as R;
    }
    
    /**
     * 返回 false 代表 ()=>any 是回调, 返回 true 代表是 id.Pause 这样的类名
     * @param value
     * @returns {{} | boolean}
     */
    isClassType(value) {
        if (value == null) return false;
        try {
            // @ts-ignore
            new new Proxy(value, {
                construct() {
                    return {};
                },
            });
            return true;
        } catch (err) {
            return false;
        }
    }
    
    isFn(value) {
        return value != null && !this.isClassType(value);
    }
    
    // addToQuery(query: ItemQuery, type: FilterType, items: (typeof Item)[][]) {
    //     if (items == null) {
    //         return;
    //     }
    //     //$generic调用性能不会太好，同样泛型参数建议整个工程，至少一个文件内只做一次
    //    
    //     let all = ItemQuery.getList();
    //     items.every((t) => {
    //         let sn = new SystemBase.List<string>();
    //         t.every(s => {
    //             console.log('[JS-ID]', s.name)
    //             sn.Add(s.name)
    //         })
    //         all.Add(sn)
    //     })
    //     query.Add(type, all)
    // }
    
    makeQuery<A extends ALL>(query: TagQueryItem, p: typeof Item | (typeof Item | typeof Component | (typeof Item | typeof Component)[])[] | ItemFilter | (new(...args: any[]) => A)): TagQueryItem {
        
        // if(p instanceof  Item || this.isItemType(p) || typeof  p['name'] == 'string'){
        //     console.log(`it's item: ${(p as Item).name}`)
        // } else {
        //     console.log("not item")
        // }
        
        if (this.isItemType(p)) {
            console.log(`[tag] ${ p.name }`)
            query.withTags(p.name)
        } else if (this.isComponent(p)) {
            this.type = $typeof(p as any);
            console.log(`[type] ${ this.type.FullName }`)
            query.withTypes(this.type)
        } else if (this.isItemArray(p)) {
            let tags = [];
            p.forEach(cb => tags.push(cb.name))
            query.tags(...tags)
        } else if (this.isItemArrayArray(p)) {
            p.forEach(cb => {
                let tags = [];
                cb.forEach(c => tags.push(c.name))
                query.tags(...tags)
            })
        } else if (Array.isArray(p)) {
            p.forEach(cb => this.makeQuery(query.tags(), cb))
        }
        return query;
    }
    
}

export function $<A extends ALL>(p: React.Component | typeof Item | (typeof Item | typeof Component | (typeof Item | typeof Component)[])[] | ItemFilter | (new(...args: any[]) => A), _?: ((t: Tags, go: GameObject) => any) /*| ((e: A, ...params: any[]) => any)*/ | (new(...args: any[]) => A) | (typeof Item | typeof Component | (typeof Item | typeof Component)[])[], callback?: ((c: A, e?: Entity | GameObject) => any) | (() => any)): A {
    
    let f = new FindCheck();
    console.log(`Test $() ${ f.isClassType(() => null) } ${ f.isClassType(id.Pause) }`)
    // 第一个参数不是回调
    
    if (p instanceof React.Component) {
        console.log(`check component ${ $typeof(_ as any)?.FullName }`)
        let tags = p.props['tags'] as Tags;
        let type = $typeof(_ as any)
        if (tags != null) {
            console.log('is tags')
            if (type != null) {
                if (type.IsAssignableFrom($typeof(GameObject))) {
                    callback?.call(tags, tags.gameObject)
                } else if (type.IsAssignableFrom($typeof(Button))) {
                    let button = tags.GetComponent($typeof(Button)) as Button;
                    if (button != null) {
                        globalThis.ButtonEvents = globalThis.ButtonEvents || new Map<string, (...args: any) => any>();
                        let cb = globalThis.ButtonEvents.get(p.constructor.name);
                        if (globalThis.ButtonEvents.has(p.constructor.name) && cb != null) {
                            button.onClick.RemoveListener(cb)
                        }
                        if (callback != null && !f.isClassType(callback)) {
                            globalThis.ButtonEvents.set(p.constructor.name, callback);
                            button.onClick.AddListener(callback as any)
                        }
                        return button as A;
                        
                    }
                    
                } else if (type.IsAssignableFrom($typeof(Component))) {
                    callback?.call(tags, tags.GetComponent(type))
                }
                return tags as A;
                
            } else if (f.isFn(_)) {
                (_ as Function)?.call(tags, tags.gameObject)
                return tags?.gameObject as A;
            }
            
        }
        return null;
    }
    
    if (f.isClassType(p)) {
        
        // console.log('is query etc: id.btn.pause')
        let query = f.makeQuery(TagSystem.query.tags(), p);
        
        if (f.isClassType(_) && $typeof(_ as any)?.IsAssignableFrom($typeof(Component))) {
            console.log(`found type: ${ $typeof(_ as any).FullName }`)
            query.withTypes($typeof(_ as any))
        }
        
        let list = fixList(query.result);
        console.log(`found num: ${ list?.Count }`)
        
        // 第二个参数是回调
        if (f.isFn(_)) {
            console.log(`press callback2 = ${ list?.Count }`)
            list?.forEach(t => (_ as Function).call(t.GetComponent($typeof(Transform)), t.GetComponent($typeof(Tags)), t))
        }
        
        // 第三个参数是回调
        if (f.isFn(callback)) {
            console.log(`press callback3 = ${ list?.Count }`)
            
            list?.forEach(t => {
                // 如果第二个参数是button
                let isButton = f.isClassType(_) && $typeof(_ as any).IsAssignableFrom($typeof(Button))
                if (isButton) {
                    Debug.Log(Strings.ToYellow(`setup button ${ Transforms.GetPath(t) }`))
                    let button = t.GetComponent($typeof(Button)) as Button;
                    if (button != null) {
                        Debug.Log(Strings.ToYellow(`Check Button ${ Transforms.GetPath(t) } => ${ isButton }`))
                        globalThis.ButtonEvents = globalThis.ButtonEvents || new Map<string, (...args: any) => any>();
                        let cb = globalThis.ButtonEvents.get(t.GetHashCode());
                        if (cb != null) {
                            button.onClick.RemoveListener(cb)
                        }
                        //if (callback != null && !f.isConstructor(callback)) {
                        globalThis.ButtonEvents.set(t.GetHashCode(), callback);
                        button.onClick.AddListener(callback as any)
                        // }
                        //return button as A;
                        
                    }
                } else {
                    if (f.isComponent(_)) {
                        callback(t.GetComponent($typeof(_ as any)) as A, t)
                    } else if (f.type != null) {
                        callback(t.GetComponent(f.type instanceof System.Type ? f.type : $typeof(f.type as any)) as A, t)
                    }
                    
                }
            })
        }
        if (list?.Count > 0) {
            return list.get_Item(0) as A;
        }
        return null;
        
    } else {
        console.log('is callback: ()=>any')
    }
    
    return null;
    
}

export function $P(p: (typeof Item)[] | ItemFilter, _: (e: any, ...params: any[]) => any): any {
    return null;
    
}