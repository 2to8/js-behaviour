import { id } from '@/Widget/id';
import { Item } from '@/Widget/Item';
import { GameEngine, MoreTags, NodeCanvas, System, UnityEngine } from 'csharp';
import { find, fixDict } from 'libs/Dictionary';
import { any } from 'prop-types';
import { $generic, $typeof } from 'puerts';
import React from 'react';
import BindScript = NodeCanvas.Tasks.Actions.BindScript;
import Button = UnityEngine.UI.Button;
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import Transforms = GameEngine.Extensions.Transforms;
import TagSystem = MoreTags.TagSystem;
import Tags = MoreTags.Tags;
import Dictionary$2 = System.Collections.Generic.Dictionary$2;
import List$1 = System.Collections.Generic.List$1;



export class BootUI_Main extends React.Component<{ target: BindScript }> {
    
    @find([ id.btn.pause, [ id.test ] ], id.caption) //
    pauseButton: Button;
    
    constructor(props) {
        super(props);
        globalThis[BootUI_Main.name] = this;
    }
    
    static inited = 0;
    
    testFunc() {
        // if(globalThis[BootUI_Main.name] != null) return null;
        // globalThis[BootUI_Main.name] = this;
        let test = TagSystem.query;
        test.tags('Test6');
        test.tags('Test9');
        if (test.result.Count > 0) {
            let go = test.result.get_Item(0);
            let tags = go.GetComponent($typeof(Tags)) as Tags;
            if (tags.Injects?.Count > 0) {
                fixDict(tags.Injects);
                for (let { value, key } of tags.Injects) {
                    Debug.Log(Strings.ToGreen(key))
                }
                Debug.Log(Strings.ToBlue(tags.Injects.map?.get(tags.Injects.keys[0])))
            }
            
        }
          //"test".prependHello();
        
        Debug.Log(Strings.ToBlue(test.result.Count > 0 ? 'Found: ' + test.result.get_Item(0).name : 'not found'));
        Debug.Log(Strings.ToBlue(Transforms.GetPath(this.props.target.gameObject)))
    }
    
    public render(): React.ReactNode {
        BootUI_Main.inited += 1;
        console.log(BootUI_Main.inited);
        this.testFunc();
        
        return null;
    }
    
}
