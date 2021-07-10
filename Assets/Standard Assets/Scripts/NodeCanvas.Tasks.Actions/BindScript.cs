using Common.JSRuntime;
using GameEngine.Extensions;
// using MoreTags.Query;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NodeCanvas.Tasks.Actions {

[Category("# Puerts")]
public class BindScript : ActionTask {
    // public BBParameter<string> objectName;
    // public BBParameter<Vector3> position;
    // public BBParameter<Vector3> rotation;
    //
    // [BlackboardOnly]
    // public BBParameter<GameObject> saveAs;

    public GameObject[] FindAll()
    {
        return null;
    }

    //public ItemQuery NewQuery() => new ItemQuery();

    public List<List<string>> getList() => new List<List<string>>();

    [Button]
    void testButton()
    {
        Debug.Log(transform.GetPath());
    }

    protected override void OnExecute()
    {
        Debug.Log(CurrentFlow.eventNode?.GetType().Name);

        Core.js.Call("globalThis.Execute", this);

        // var newGO = new GameObject(objectName.value);
        // newGO.transform.position = position.value;
        // newGO.transform.eulerAngles = rotation.value;
        // saveAs.value = newGO;
        EndAction();
    }
}

}