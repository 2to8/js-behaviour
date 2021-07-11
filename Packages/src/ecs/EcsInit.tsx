//import availableTypedArrays from "available-typed-arrays"
import { Tag } from '@/Codegen/Autoload';
import { UnityWidgetRoot } from '@/Components/UnityWidgetRoot';
import { HostConfig } from '@/config/HostConfig';
import { $extension, $typeof } from 'puerts';
import { TagMaps } from 'Tags/TagMap';
import { dict_install } from 'libs/Dictionary';
import { id } from '@/Widget/id';
import { GameEngine, MoreTags, NodeCanvas, System,  UnityEngine } from 'csharp';
import { prefabs } from 'Generated/Prefabs_Settings';
import { DatabaseInit } from 'DBInit';
import { NewApp } from 'NewApp';
import React from 'react';
import Reconciler from 'react-reconciler';
import { BootUI_Main } from 'Scenes/BootScene/BootUI/BootUI_Main';

//import app from 'libs/orm/app';
//import filter from 'array-filter';
import GameObject = UnityEngine.GameObject;
import ActionTask = NodeCanvas.Framework.ActionTask;
import BindScript = NodeCanvas.Tasks.Actions.BindScript;
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import Transforms = GameEngine.Extensions.Transforms;
import Tags = MoreTags.Tags;
import IP = GameEngine.IP;
//import TetrisUtil = Tetris.TetrisUtil;
import Transform = UnityEngine.Transform;
import UI = UnityEngine.UI;

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

console.log(`hello, typescript start IP: ${ IP.GetIP() }`)

DatabaseInit();

// ;(async ()=> {
//       await orm.then(()=> {
//
//       });
// } )().catch((err) => {
//     console.error('error:', err.response.status, err.response.data)
// })

//app()

global.Execute = (task: BindScript) => {
    globalThis.render(prefabs.get('SandBox').BootUI_Main(task));
    // Debug.Log(Strings.ToBlue(`${ task.gameObject.name } ${
    // ActionTask.CurrentFlow.eventNode?.GetType().Name}`) );
}



global.OnTagStart = (context: Tags) => {
    if (context == null) return;
    TagMaps.forEach((value, i) => {
        let list = []
        value.tags.forEach(t => list.push(t.name))
        if (context.Match(...list)) {
            Debug.Log(Strings.ToGreen(list.join(', ')))
            globalThis.render(value.call(context));
        }
    })
    
    
    
        //let t=[...GameObject.Find("test").GetInChild(UI.Image)]
    //t[0].doF
    
    
    //Debug.Log(Strings.ToGreen('Tags Loaded: ' + Transforms.GetPath(context.gameObject)))
}

global.OnTagAwake = (context: Tags) => {
    
}

export default function(task: ActionTask) {
    
    const reconciler = Reconciler(HostConfig as any)
    let root = new UnityWidgetRoot(null);
    const container = reconciler.createContainer(root, null, false, null);
    // reconciler.updateContainer(<UIRoot/>, container, null, () => {
    //
    // });
    
    reconciler.updateContainer(<NewApp task={ task }/>, container, null, () => {
        
    });
    
    globalThis['reconciler'] = reconciler;
}

