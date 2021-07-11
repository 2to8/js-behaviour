import { Item } from '@/Widget/Item';
import { UnityEngine } from 'csharp';

export namespace Dots {
    import Component = UnityEngine.Component;
    
    export class Entity {
        add(...c: (typeof Item | Item | typeof Component | Component)[]) {
        
        }
        
        remove(...c: (typeof Item | Item | typeof Component | Component)[]) {
        
        }
    }
}
