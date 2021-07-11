using App.Support;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace App.Actions
{
    [Name("TsAction Test"), Category("✫ Typescript"), Description("测试ts绑定")]
    public class TsActionTest : ActionTask<Transform>
    {
        public BBParameter<string> module;
        public BBParameter<TextAsset> asset;

        protected override void OnExecute()
        {
            this.JsCall(nameof(OnExecute));
        }
    }
}