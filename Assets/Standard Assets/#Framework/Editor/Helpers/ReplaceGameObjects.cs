using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helpers {

// CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
// March 2010

//Modified by Kristian Helle Jespersen
//June 2011

public class ReplaceGameObjects : ScriptableWizard {

    public bool copyValues = true;
    public bool usePrefabScale = false;
    public GameObject NewType;
    public GameObject[] OldObjects;

    [MenuItem("Tools/*** Replace to Prefab ***")]
    static void CreateWizard()
    {
        var replaceGameObjects = DisplayWizard<ReplaceGameObjects>("Replace GameObjects", "Replace");

        if (ReplaceGameObjectsConfig.Instance.prefab != null) {
            replaceGameObjects.NewType = ReplaceGameObjectsConfig.Instance.prefab;
        }
        replaceGameObjects.OldObjects = Selection.gameObjects;
    }

    void OnWizardCreate()
    {
        //Transform[] Replaces;
        //Replaces = Replace.GetComponentsInChildren<Transform>();
        ReplaceGameObjectsConfig.Instance.prefab = NewType;

        foreach (var go in OldObjects) {
            GameObject newObject;

            //newObject = (GameObject)EditorUtility.InstantiatePrefab(NewType);

            //GameObject o = null;
            newObject = PrefabUtility.GetPrefabParent(NewType) as GameObject;

            if (PrefabUtility.GetPrefabType(NewType).ToString() == "PrefabInstance") {
                newObject = (GameObject)PrefabUtility.InstantiatePrefab(newObject);
                PrefabUtility.SetPropertyModifications(newObject, PrefabUtility.GetPropertyModifications(NewType));
            } else if (PrefabUtility.GetPrefabType(NewType).ToString() == "Prefab") {
                newObject = (GameObject)PrefabUtility.InstantiatePrefab(NewType);
            } else {
                newObject = Instantiate(NewType) as GameObject;
            }
            Undo.RegisterCreatedObjectUndo(newObject, "created prefab");
            newObject.transform.parent = go.transform.parent;
            newObject.transform.localPosition = go.transform.localPosition;
            newObject.transform.localRotation = go.transform.localRotation;

            if (!usePrefabScale) {
                newObject.transform.localScale = go.transform.localScale;
            }
            DestroyImmediate(go);
        }
        var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

        if (prefabStage != null) {
            Debug.Log("changed");
            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            Debug.Log("set dirty");
        } else {
            Debug.Log("changed");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("set dirty");
        }
    }

}

}