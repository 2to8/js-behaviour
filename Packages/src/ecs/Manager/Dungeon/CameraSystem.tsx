import { $ } from '@/types';
import { ReactSystem } from '@/Manager/ReactSystem';
import { el } from '@/Widget/el';
import { id } from '@/Widget/id';
import { MoreTags, ReactECS } from 'csharp';
import React from 'react';

export namespace CameraSystem {
    
    import SystemBase = ReactSystem.SystemBase;
    import ReactSystemEntry = MoreTags.Types.ReactSystemEntry;
    import SystemAdapter = ReactECS.SystemAdapter;
    
    interface P extends ReactSystem.baseProps {
    
    }
    
    interface S extends ReactSystem.baseState {
        enabled: boolean
    }
    
    export class CameraSystem extends SystemBase<P, S> {
        state: S
        debugQuery: boolean = false
        
        constructor(props) {
            super(props);
            if (this.debugQuery) {
                $(id.btn.pause, el.text, (e, c) => {
                    e.add(el.button);
                    c.text = 'test'
                })
                $([ [ id.btn.pause, id.caption ] ], el.text).text = 'test btn';
                $([
                    [ id.btn.pause ], [ id.btn.pause, id.test ],
                ], el.button)?.onClick.AddListener(() => {
                
                })
            }
            
            this.state = {
                enabled: true,
                ready: true,
                entities: null,
            }
        }
    
        public onStart(entry: ReactSystemEntry, adapter: SystemAdapter): boolean {
            console.log(this.className ," ===== onStart invoked ====")
            return true;
        }
        
    }
}
