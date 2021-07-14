import { GameEngine, NodeCanvas } from 'csharp';
import ConcurrentState = NodeCanvas.StateMachines.ConcurrentState;
import Strings = GameEngine.Extensions.Strings;

export class TConcurrentState extends ConcurrentState {
    public OnValidate($assignedGraph: NodeCanvas.Framework.Graph): void {
        //super.OnValidate($assignedGraph);
        console.log(Strings.ToYellow("state validate"))
    }
    
    public OnGraphStarted(): void {
        //super.OnGraphStarted();
    }
    
    public OnGraphStoped(): void {
        //super.OnGraphStoped();
    }
}