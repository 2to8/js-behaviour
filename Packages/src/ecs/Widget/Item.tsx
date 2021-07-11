import { Success } from '@/Components/Success';
import { MoreTags, Unity } from 'csharp';
import React, { Children } from 'react';
import Tags = MoreTags.Tags;

type ParamType<T> = T extends (param: infer P) => any ? P : T;

export class Item extends React.Component<{ target: Tags },{}> {
    
    public static isItem: boolean = true
    
    public name:string = "";
    
    constructor(props) {
        super(props);
    }
    
    public filter(children, filterFn) {
        return Children
        .toArray(children)
        .filter(filterFn);
    };
    
    public add(...component: Item[]): this {
        return this;
    }
    
    public remove(...component: Item[]): this {
        
        return this;
    }
    
    public has<T>(_: (e: T) => any) {
    
    }
    
    public render(): React.ReactNode {
        return <Success/>;
    }
    
    
}