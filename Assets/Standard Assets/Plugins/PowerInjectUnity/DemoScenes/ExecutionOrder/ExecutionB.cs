using UnityEngine;
using System.Collections;
using PowerInject;

[Power]
public class ExecutionB : MonoBehaviour
{
    private void Awake()
    {
        print("AwakeB");
    }

    private void Start()
    {
        print("StartB");
    }

    [Produce]
    private string produceB_1()
    {
        print("produceB_1");
        return "";
    }

    [Produce]
    private float produceB_2()
    {
        print("produceB_2");
        return 0;
    }

    [OnInjected]
    public void injectedA()
    {
        print("Injected_B");
    }

    private bool ranupdate = false;
    private bool ranFixedUpdate = false;

    private void Update()
    {
        if(!ranupdate) {
            print("Update_B");
            ranupdate = true;
        }
    }

    private void FixedUpdate()
    {
        if(!ranFixedUpdate) {
            print("FixedUpdate_B");
            ranFixedUpdate = true;
        }
    }
}