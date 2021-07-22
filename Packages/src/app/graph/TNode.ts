import { NodeCanvas } from 'csharp';
import Node = NodeCanvas.Framework.Node;

export class TNode extends Node {
    public Validate($assignedGraph: NodeCanvas.Framework.Graph): void {
        //super.Validate($assignedGraph);
        if (this.tag != null) {
            console.log('[node validate]', this.tag, this.name)
            this.tag.split(',').forEach(tag => this.AddTag(tag));
        }
        
    }
    
    onTest(){
        this.Validate.bind(this)
    }
}