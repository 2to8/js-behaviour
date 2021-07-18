import { App } from 'csharp';
import TestFindNode = App.Actions.TestFindNode;

export class TTestFindNode extends TestFindNode {
    OnExecute() {
        console.log('TTestFindNode Execute')
    }
}