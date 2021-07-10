import { Sandbox, UnityEngine } from 'csharp';
import TestCs2Ts = Sandbox.TestCs2Ts;
import Debug = UnityEngine.Debug;

export default class TestBind extends TestCs2Ts {
    public test2() {
        Debug.Log(this.num + " = test7..")
        this.num = 100;
        //
    }
}