import { NodeCanvas } from 'csharp';
import Node = NodeCanvas.Framework.Node;

export class TNode extends Node {
    public Validate($assignedGraph: NodeCanvas.Framework.Graph): void {
        //super.Validate($assignedGraph);
        console.log('[node validate]', this.tag, this.name)
    }
}