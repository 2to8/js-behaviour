using GameEngine.Extensions;
using GameEngine.Kernel.Args;
using GameEngine.Views.Contracts;
using UnityEngine.UI;

namespace GameEngine.Views {

// public class UITitle : TManager<UITitle> {
//
//     Text _textHead;
//     public override object ViewName => Consts.V_UITitle;
//
//     protected override void Start()
//     {
//         base.Start();
//         _textHead = transform.Find("HeadText").GetComponent<Text>();
//     }
//
//     public override void RegisterViewEvents()
//     {
//         attentionEvents.Add(Consts.E_LevelChange);
//     }
//
//     public override void HandleEvent(object eventName, object data)
//     {
//         switch( eventName ) {
//             case Consts.E_LevelChange :
//                 var e = data as LevelArgs;
//                 _textHead.text = e.level.ToString();
//
//                 break;
//         }
//     }
//
// }

}