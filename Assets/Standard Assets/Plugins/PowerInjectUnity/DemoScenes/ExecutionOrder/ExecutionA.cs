using UnityEngine;
using System.Collections;
using PowerInject;

[Power]
public class ExecutionA : MonoBehaviour
{
    private void Awake()
    {
        print("AwakeA");
    }

    private void Start()
    {
        print("StartA");
    }

    [Produce]
    private float produceA_2()
    {
        print("produceA_2");
        return 0;
    }

    [Produce]
    private string produceA_1()
    {
        print("produceA_1");
        return "";
    }

    [OnInjected]
    public void injectedA()
    {
        print("Injected_A");
    }

    private bool ranupdate = false;
    private bool ranFixedUpdate = false;

    private void Update()
    {
        if(!ranupdate) {
            print("Update_A");
            ranupdate = true;
        }
    }

    private void FixedUpdate()
    {
        if(!ranFixedUpdate) {
            print("FixedUpdate_A");
            ranFixedUpdate = true;
        }
    }
}