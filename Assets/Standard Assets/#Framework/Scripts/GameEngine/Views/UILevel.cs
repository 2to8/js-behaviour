using GameEngine.Extensions;
using GameEngine.Kernel.Args;
using GameEngine.Views.Contracts;
using UnityEngine.UI;

namespace GameEngine.Views {

// public class UILevel : TManager<UILevel> {
//
//     public Button addLevelButton;
//     public Text numberText;
//     public Text prenumberText;
//     public override object ViewName => Consts.V_UILevel;
//
//     protected override void Start()
//     {
//         base.Start();
//         numberText.text = 0.ToString();
//         prenumberText.text = 0.ToString();
//         addLevelButton.onClick.AddListener(OnClickAddLevel);
//     }
//
//     public void OnClickAddLevel()
//     {
//         SendEvent(Consts.E_AddLevel);
//     }
//
//     //void OnMouseUpAsButton() { }
//
//     public override void RegisterViewEvents()
//     {
//         attentionEvents.Add(Consts.E_LevelChange);
//     }
//
//     public override void HandleEvent(object eventName, object data)
//     {
//         switch( eventName ) {
//             case Consts.E_LevelChange : {
//                 var e = data as LevelArgs;
//                 numberText.text = e.level.ToString();
//                 prenumberText.text = e.level + "%";
//             }
//
//                 break;
//         }
//     }
//
// }

}