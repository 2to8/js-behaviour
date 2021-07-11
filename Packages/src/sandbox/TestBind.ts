import { Sandbox, UnityEngine, UnityRoyale } from 'csharp';
import { $ } from 'types';
import { id } from 'Widget/id';
import TestCs2Ts = Sandbox.TestCs2Ts;
import Debug = UnityEngine.Debug;
import InputManager = UnityRoyale.InputManager;
import CardManager = UnityRoyale.CardManager;

export default class TestBind extends TestCs2Ts {
    public test2() {
        Debug.Log(this.num + " = test7..")
        this.num = 100;
        //$(CardManager).cardPrefab = 
        //
    }
}