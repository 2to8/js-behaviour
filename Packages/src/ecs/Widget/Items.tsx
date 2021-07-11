import { Success } from '@/Components/Success';
import { IdButtonItem } from '@/Widget/IdButtonItem';
import { Item } from '@/Widget/Item';
import { MoreTags, UnityEngine } from 'csharp';
import Enumerable = require('linq');
import React, { Children } from 'react';
import Tags = MoreTags.Tags;
import Scene = UnityEngine.SceneManagement.Scene;

export class Items extends React.Component<{ scene: Scene }, { items: Map<number, Map<string, Item>> }> {
    tags: Tags
    
    static instance: Items
    items = new Map()
    
    public filter(children, filterFn) {
        return Children
        .toArray(children)
        .filter(filterFn);
    };
    
    constructor(props) {
        super(props);
        Items.instance = this;
        this.state = {
            items: new Map<number, Map<string, Item>>(),
        }
    }
    
    public getItem<T extends Item>(c: new() => T, tag: Tags): Item {
        let tmp = this.state.items;
        if (tag == null || !tmp.has(tag.id) || !tmp[tag.id].has(c)) return null;
        return tmp[tag.id][c];
    }
    
    public addItem<T extends Item>(c: new() => T, obj: T, tag: Tags): T {
        let tmp = this.state.items;
        if (!tmp.has(tag.id)) {
            tmp[tag.id] = new Map<string, Item>();
        }
        tmp[tag.id][c] = obj;
        
        this.setState({ items: tmp });
        return obj;
    }
    
    public removeItem<T extends Item>(c: new() => T, tag: Tags): T {
        
        if (this.state.items.has(tag.id) && this.state.items[tag.id].has(c)) {
            let ret = this.state.items[tag.id][c];
            this.state.items[tag.id].delete(c);
            this.setState(this.state);
            return ret;
        }
        
        return null;
    }
    
    public render(): React.ReactNode {
        return (<>
            { this.items.forEach(item => {
                item.render()
            }) }
        </>);
    }
    
}