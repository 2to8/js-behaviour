import { GameEngine, MoreTags, UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import React from 'react';
import Tags = MoreTags.Tags;
import Button = UnityEngine.UI.Button;
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;

export class BackButtonSystem extends React.Component<{ tags: Tags }> {
    
    componentWillUnmount() {
        console.log('BackButtonSystem unmount')
        // this.off('authChange', this.authChange);
        // this.authChange = null;
    }
    
    public render(): React.ReactNode {
        console.log('back button started')
        let button = this.props.tags.GetComponent($typeof(Button)) as Button;
        
        if (globalThis.OnBackClick != null) {
            button.onClick.RemoveListener(globalThis.OnBackClick)
        }
        globalThis.OnBackClick = () => {
            Debug.Log(Strings.ToGreen('test back 123'))
        }
        button.onClick.AddListener(globalThis.OnBackClick)
        //button.onClick.RemoveAllListener();
        
        return null;
    }
}