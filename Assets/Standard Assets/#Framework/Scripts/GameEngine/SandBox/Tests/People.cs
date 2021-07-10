using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// using UnityEditor.Events;

namespace GameEngine.SandBox.Tests {

public class People : MonoBehaviour {

    static People man;
    public UnityAction<int> action;
    Button Button;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (man == null) {
            man = this;
        }

        //UnityEventTools.AddIntPersistentListener(Button.onClick,new UnityAction<int>(MyFunction), 10 );
        man.Eat();
    }

    public void MyFunction(int i)
    {
        print(i);
    }

    void Eat()
    {
        Debug.Log("i need more.");
    }

}

}