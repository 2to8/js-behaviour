import { GridLayout, Text, TextButton } from '@/Components/UI';
import { UnityEngine } from 'csharp';
import React from 'react';

export class UIRoot extends React.Component<{}, { list: number }> {
    constructor(props) {
        super(props);
        this.state = {
            list: 2,
        };
    }
    
    render() {
        let id = 0;
        return (<GridLayout cellSize={ new UnityEngine.Vector2(50, 50) }>
            <Text text={ this.state.list.toString() }/>
            { this.state.list > 0 && Array.from(Array(this.state.list), (i => {
                console.log(1)
                return <Text key={ 'id_' + (id += 1) + '_' + i } text="111"/>
            })) }
            <TextButton text={ '+1' } onClick={ () => {
                this.setState((s, p) => {
                    return { list: s.list + 1 }
                })
            } }/>
            <TextButton text={ '+5' } onClick={ () => {
                this.setState((s, p) => {
                    return { list: s.list + 5 }
                })
            } }/>
            <TextButton text={ '-2' } onClick={ () => {
                this.setState((s, p) => {
                    return { list: s.list - 2 }
                })
            } }/>
        </GridLayout>);
    }
}