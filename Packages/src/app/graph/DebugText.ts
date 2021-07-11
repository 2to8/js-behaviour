import { NodeCanvas, UnityEngine } from 'csharp';
import DebugLogText = NodeCanvas.Tasks.Actions.DebugLogText;
import Debug = UnityEngine.Debug;

export class DebugText extends DebugLogText {
    OnExecute() {
        Debug.Log(`[Js] ${this.log.value}`);
    }
}                 