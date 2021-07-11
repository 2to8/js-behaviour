import { TMPro } from 'csharp';
import { UnityEngine } from 'csharp';

export module el {
    
    import UI = UnityEngine.UI;
    import TMP_Text = TMPro.TMP_Text;
    import ScrollRect = UnityEngine.UI.ScrollRect;
    import RectTransform = UnityEngine.RectTransform;
    import Transform = UnityEngine.Transform;
    import Canvas = UnityEngine.Canvas;
    
    export class button extends UI.Button {
    
    }
    
    export class transform extends Transform {}
    
    export class rt extends RectTransform {}
    
    export class scrollView extends ScrollRect {}
    
    export class canvas extends Canvas {}
    
    export class text extends UI.Text {
    
    }
    
    export class tm_text extends TMP_Text {}
}