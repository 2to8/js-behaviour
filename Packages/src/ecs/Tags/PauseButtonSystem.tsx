import { GameEngine, MoreTags, UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import React from 'react';
import { $ } from 'types';
import { debug } from 'webpack';
import { id } from 'Widget/id';
import Tags = MoreTags.Tags;
import Button = UnityEngine.UI.Button;
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import Transform = UnityEngine.Transform;
import GameObject = UnityEngine.GameObject;
import Transforms = GameEngine.Extensions.Transforms;

export class PauseButtonSystem extends React.Component<{ tags: Tags }> {
    
    componentWillUnmount() {
        console.log(`PauseButtonSystem unmount`)
        // this.off('authChange', this.authChange);
        // this.authChange = null;
    }
    
    public render(): React.ReactNode {
        if (this.props.tags == null) return null;
        
        console.log('pause button started')
        
        $(id.PauseWin, t => {
            t.gameObject.SetActive(false)
        })
        //
        $(id.Close, Button, () => {
            Debug.Log(Strings.ToYellow("check id.close"))
            $(id.PauseWin, t => {
                t.gameObject.SetActive(false)
            })
        })
        
        $(this, Button, () => {
            Debug.Log(Strings.ToGreen('test pause button'))
            $(id.PauseWin, t => {
                console.log(`PauseWin found ${ Transforms.GetPath(t.gameObject) }`)
                t.gameObject.SetActive(!t.gameObject.activeInHierarchy);
                //globalThis.pauseWin = globalThis.pauseWin || go;
                //globalThis.pauseWin.SetActive(true)
                
            });
            //Debug.Log(Strings.ToGreen(globalThis.pauseWin?.name))
        })
        // let button = this.props.tags.GetComponent($typeof(Button)) as Button;
        // if (globalThis.OnPauseButton != null) {
        //     button.onClick.RemoveListener(globalThis.OnPauseButton)
        // }
        // globalThis.OnPauseButton = () => {
        //     Debug.Log(Strings.ToGreen('test pause'))
        //     $(id.PauseWin, GameObject)?.SetActive(true);
        // }
        // button.onClick.AddListener(globalThis.OnPauseButton)
        return null;
    }
}