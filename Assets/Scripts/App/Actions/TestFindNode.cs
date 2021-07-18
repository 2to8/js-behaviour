using App.Support;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Actions
{
    [Name(nameof(TestFindNode)), Category("✫ Typescript"), Description("查找节点")]
    public class TestFindNode : ActionTask<Transform>
    {
        [Button]
        protected override void OnExecute()
        {
            this.JsCall(nameof(OnExecute));
        }
    }
}