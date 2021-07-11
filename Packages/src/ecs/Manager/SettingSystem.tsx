import { ReactSystem } from '@/Manager/ReactSystem';

export namespace SettingSystem {
    
    type P = {} & ReactSystem.baseProps
    type S = {} & ReactSystem.baseState
    
    export class SettingSystem extends ReactSystem.SystemBase<P, S> {
        constructor(props) {
            super(props);
        }
        
    }
}
