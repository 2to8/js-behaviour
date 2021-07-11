import { ReactSystem } from '@/Manager/ReactSystem';
import { id } from '@/Widget/id';
import { speed } from '@/Widget/speed';
import { MoreTags, ReactECS, UnityEngine } from 'csharp';
import React from 'react';
import baseProps = ReactSystem.baseProps;
import ReactSystemEntry = MoreTags.Types.ReactSystemEntry;

export namespace HelloWorldSystem {
    
    import UI = UnityEngine.UI;
    
    export interface HelloWorldProps extends baseProps {
        //times: number
    }
    
    export interface HelloWorldState extends ReactSystem.baseState {
        //
    }
    
    export class HelloWorldSystem extends ReactSystem.SystemBase<HelloWorldProps, HelloWorldState> {
        times: number = 0
        
        constructor(props) {
            super(props);
            if (globalThis[HelloWorldSystem.name] == null) {
                globalThis[HelloWorldSystem.name] = this;
            }
        }
        
        public onStart(entry: MoreTags.Types.ReactSystemEntry, adapter: ReactECS.SystemAdapter): boolean {
            super.onStart(entry, adapter);
            console.log('hello world, systems!')
            return true;
        }
        
        render() {
            let self = globalThis[HelloWorldSystem.name]
            this.forEach(it => {
                it.has<speed>(data => data.speedValue += 1)
            })
            if (self.times < 2) {
                this.with({ all: [ [ id.btn.start ], [ id.test ] ] }, UI.Button, (e, c) => {
                    console.log('test filter: ', c.name)
                }).with(UI.Button, (e, c) => {
                
                })
                self.times += 1;
                console.log(`update times: ${ HelloWorldSystem.name } = ${ self.times }`)
            }
            
            return this.success();
        }
    }
}

