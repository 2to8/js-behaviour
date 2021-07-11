import { Success } from '@/Components/Success';
import { UnityEngine } from 'csharp';
import React from 'react';

type props = {
    log: string
}
type state = {
    text: string
}

export class Dump extends React.Component<props, state> {
    constructor(props) {
        super(props);
        this.state = {
            text: props.log,
        }
    }
    
    public render(): React.ReactNode {
        UnityEngine.Debug.Log(this.state.text)
        return (<Success/>);
    }
    
}