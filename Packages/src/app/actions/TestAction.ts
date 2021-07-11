import { App, UnityEngine } from 'csharp';
import { $ } from 'types';
import { id } from 'Widget/id';
import TsActionTest = App.Actions.TsActionTest;
import Debug = UnityEngine.Debug;
import GameObject = UnityEngine.GameObject;

export class TestAction extends TsActionTest {
    OnExecute() {
        Debug.Log('test ts');
        $(id.test, tag => {
            Debug.Log(tag.name);
            tag.gameObject.SetActive(false);
        })
    }
}