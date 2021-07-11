import { ReactSystem } from '@/Manager/ReactSystem';
import { MoreTags, ReactECS } from 'csharp';
import ReactSystemEntry = MoreTags.Types.ReactSystemEntry;

export namespace LevelSystem {
    import SystemAdapter = ReactECS.SystemAdapter;
    type P = {} & ReactSystem.baseProps
    type S = {} & ReactSystem.baseState
    
    export class LevelSystem extends ReactSystem.SystemBase<P, S> {
        constructor(props) {
            super(props);
        }
    
        public onStart(entry: ReactSystemEntry, adapter: SystemAdapter): boolean {
            console.log(this.className ," ===== onStart invoked ====")
            return true;
        }
        
    }
}

