import { Autoload } from '@/Codegen/Autoload';
import { Dump } from '@/Components/Dump';
import { Items } from '@/Widget/Items';
import { NodeCanvas, UnityEngine } from 'csharp';
import React from 'react';
import GameObject = UnityEngine.GameObject;
import ActionTask = NodeCanvas.Framework.ActionTask;

type _props = {
    task: ActionTask
}

type _state = {
    booted: boolean
    helloWorld: string,
    target: JSX.Element[]
}

export class NewApp extends React.Component<_props, _state> {
    
    //reconciler: typeof Reconciler
    all: JSX.Element[] = []
    
    constructor(props) {
        super(props);
        //this.reconciler = props.reconciler;
        
        //console.log(JSTools.getDecimal(3.1415926));
        
        this.state = {
            target: null, booted: true, helloWorld: 'Components Started',
        };
        
        globalThis.render = (target: JSX.Element) => {
            if (!this.all.contains(target)) {
                this.all.push(target)
            }
            this.setState({ booted: false, helloWorld: '', target: this.all });
        }
    }
    
    //<Dump log={ this.state.helloWorld }/>
    
    public render(): React.ReactNode {
        return (<>
            { ...this.state['target'] }
        </>);
    }
}