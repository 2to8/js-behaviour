import { Success } from '@/Components/Success';
import { ALL } from '@/types';
import { Item } from '@/Widget/Item';
import { Items } from '@/Widget/Items';
import { MoreTags, NodeCanvas, ReactECS, System } from 'csharp';
import { $generic, $typeof } from 'puerts';
import React, { Children } from 'react';
import ReactSystemEntry = MoreTags.Types.ReactSystemEntry;
import SystemAdapter = ReactECS.SystemAdapter;

export namespace ReactSystem {
    import Tags = MoreTags.Tags;
    import FilterType = ReactECS.FilterType;
    import ItemQuery = ReactECS.ItemQuery;
    import RouteProxy = ReactECS.RouteProxy;
    import List$1 = System.Collections.Generic.List$1;
    
    export interface ItemFilter {
        any?: ((typeof Item)[])[],
        all?: ((typeof Item)[])[],
        none?: ((typeof Item)[])[],
    }
    
    export interface baseProps {
        //className: string
        tid: number
        tag: string
    }
    
    export interface baseState {
        ready: boolean
        entities: SystemAdapter
    }
    
    export class SystemContainer {
        [key: string]: SystemBase<any, any> | any
        
        systems = new Map();
        instanceMap = new Map();
        triggers = new Map();
        
        private static instance: SystemContainer;
        private _temperature: number;
        
        private constructor() {
        }
        
        // 在TypeScript使用泛型创建工厂函数时，需要引用构造函数的类类型。比如：
        //
        // function create<T> (c: {new(): T;}): T {
        //     return new c();
        // }
        // 使用原型属性推断并约束构造函数与类实例的关系
        // function createInstance<A extends Keeper3> (c: new() => A): A {
        //     return new c();
        // }
        //
        // console.log(createInstance(ChildrenKeeper1));
        
        static getInstance() {
            if (!SystemContainer.instance) {
                SystemContainer.instance = new SystemContainer();
                SystemContainer.instance._temperature = 0;
                //SystemContainer.instance.systems = [];
            }
            return SystemContainer.instance;
        }
        
        /**
         * 根据类型返回单例
         * @param e
         * @param {{new(): T}} c
         * @param create
         * @returns {T}
         */
        static getItemInstance<T>(e: Tags, c: new(et?: Tags) => T, create?: (_e: Tags, ..._args: any[]) => T): T {
            if (!this.instance.instanceMap.has(e.id)) {
                this.instance.instanceMap[e.id] = new Map<typeof c, T>();
            }
            if (!this.instance.instanceMap[e.id].has(c)) {
                this.instance.instanceMap[e.id][c] = create ? create(e) : new c(e);
            }
            return this.instance.instanceMap[e.id][c];
            
        }
        
        public add<T extends SystemBase<any, any>>(name: string, system: T): this {
            
            this.systems[name] = {
                onUpdate: (adapter: SystemAdapter) => {
                    //console.log(adapter.name, "update")
                    system.setState({ entities: adapter })
                }, onStart: (entry: ReactSystemEntry, adapter: SystemAdapter): boolean => {
                    let ret = system.onStart(entry, adapter)
                    return ret === true || ret === false ? ret : true
                }, invoke: (node: NodeCanvas.Framework.Node, router: RouteProxy) => {
                    system.invoke(node, router)
                },
            };
            this.triggers[name] = system;
            globalThis['Triggers'] = this.triggers;
            globalThis['Systems'] = this.systems
            return this;
        }
    }
    
    export class BaseComponent<P, S> extends React.Component<P, S> {
    }
    
    export class SystemBase<T extends baseProps, TS extends baseState> extends BaseComponent<T, TS> {
        tid: number
        className: string
        ready: boolean
        state: TS & any
        
        public static List = $generic(List$1, System.String);
        
        // findByEntityAndTagId(): any {
        //     const subRows = (React.Children.toArray(this.props.children) as
        // HelloWorldSystem<any,any>[]).filter((child, i) => { return (child.props as
        // HelloWorldProps).times; });  // const subRows = React.Children.map(this.props.children,
        // (child, i) => { //     return child; // });  // let snapCount =
        // React.Children.toArray(this.props.children) // .filter((item) =>  item.props.className
        // === 'snap').length; // return snapCount; }
        
        componentDidMount() {
            //this.textInput.focusTextInput();
        }
        
        public filter(children, filterFn) {
            return Children
            .toArray(children)
            .filter(filterFn);
        };
        
        constructor(props) {
            super(props);
            this.tid = props.tid
            this.className = props.tag
            let container = SystemContainer.getInstance();
            container.add(props.tag, this)
            
            this.state = {
                ready: false, entities: null,
            }
        }
        
        success() {
            return (<Success/>);
        }
        
        public invoke(node: NodeCanvas.Framework.Node, router: RouteProxy) {
            console.log(node.name)
        }
        
        public onStart(entry: ReactSystemEntry, adapter: SystemAdapter): boolean {
            console.log(this.className, ' ===== onStart invoked ====')
            return true;
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
        
        public isItemFilter(value: any): value is ItemFilter {
            if (value instanceof Array) {
                return false;
                // value.forEach(function(item) { // maybe only check first value?
                //     if (typeof item !== 'string') {
                //         return false
                //     }
                // })
                // return true
            }
            if (value.all || value.any || value.none) {
                return true;
            }
            return false
        }
        
        public isItemType(value: any): value is Item {
            if (value instanceof Item) {
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
        
        public isItemArray(value: any): value is Item[] {
            if (value instanceof Array) {
                // @ts-ignore
                value.forEach(item => { // maybe only check first value?
                    if (item instanceof Item) {
                        return true
                    }
                })
            }
            return false
        }
        
        public isItemArrayArray(value: any): value is (Item[])[] {
            if (value instanceof Array) {
                value.forEach(function(item) { // maybe only check first value?
                    
                    if (item instanceof Array) {
                        // @ts-ignore
                        item.forEach(sub => { // maybe only check first value?
                            
                            if (sub instanceof Item) {
                                return true
                            }
                        })
                    }
                })
            }
            return false
        }
        
        public isMonoBehaviourType(value: any): value is System.Type {
            if (value instanceof System.Type) {
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
        
        isConstructor(value) {
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
        
        addToQuery(query: ItemQuery, type: FilterType, items: (typeof Item)[][]) {
            if (items == null) {
                return;
            }
            //$generic调用性能不会太好，同样泛型参数建议整个工程，至少一个文件内只做一次
            
            let all = ItemQuery.getList();
            items.every((t) => {
                let sn = new SystemBase.List<string>();
                t.every(s => {
                    console.log('[JS-ID]', s.name)
                    sn.Add(s.name)
                })
                all.Add(sn)
            })
            query.Add(type, all)
        }
        
        public with<A extends ALL>(p: typeof Item | (typeof Item | (typeof Item)[])[] | ItemFilter | (new() => A), _?: (new() => A) | ((e: Items, c: A) => any) | typeof Item, cb?: (e: Items, c: A) => any): this {
            if (this.isItemFilter(p)) {
                // 第一种方式, 参数1为筛选器, 参数2为type,参数3为回调, 此时需要保存query的最后查询结果
                console.log(p, 'is item filter')
                console.log(this.isConstructor(_), this.isConstructor(cb))
                let query = ItemQuery.NewQuery();
                //console.log(JSON.stringify(p))
                this.addToQuery(query, FilterType.All, p.all)
                this.addToQuery(query, FilterType.Any, p.any)
                this.addToQuery(query, FilterType.None, p.none)
                let result = query.FetchAll();
                for (let i = 0; i < result.Count; i++) {
                    let tags = result.get_Item(i);
                    
                    var param = $typeof(_ as any) ? tags.GetComponent($typeof(_ as any)) : Items.instance.getItem(_ as any, tags);
                    
                    console.log(tags.name, param)
                    
                    cb(Items.instance, param as any)
                }
                //query.Add(FilterType.All,...p.all)
                
            } else if (this.isConstructor(p) && !this.isConstructor(_)) {
                // 第二种方式, 参数1为type, 参数2为回调, 此时使用保存的最后查询结果并取type进行回调
            }
            return this;
        }
        
        // public  $(p: (typeof Item)[] | filter, _: (e: any, ...params: any[]) => any): any {
        //    return;
        // }
        
        public setReady(value: boolean) {
            this.setState({ entities: undefined, ready: value })
        }
        
        public forEach(_: (e: Item) => React.ReactNode | void): this | any {
            return this;
        }
        
        public render(): React.ReactNode {
            return this.success();
        }
    }
}


