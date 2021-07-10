using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Kernel.Pool {

public class SubPool {

    //集合
    readonly List<GameObject> m_objects = new List<GameObject>();

    //相对父物体
    readonly Transform m_parent;

    //预设
    readonly GameObject m_prefab;

    public SubPool(Transform parent, GameObject prefab)
    {
        m_parent = parent;
        m_prefab = prefab;
    }

    //名字标识
    public string Name => m_prefab.name;

    //取对象
    public GameObject Spawn()
    {
        GameObject go = null;

        foreach (var obj in m_objects) {
            if (!obj.activeSelf) {
                go = obj;

                break;
            }
        }

        if (go == null) {
            go = Object.Instantiate(m_prefab);
            go.transform.SetParent(m_parent);
            m_objects.Add(go);
        }

        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

        return go;
    }

    //回收对象
    public void Unspawn(GameObject go)
    {
        if (Contains(go)) {
            go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    //回收该池子的所有对象
    public void UnspawnAll()
    {
        foreach (var go in m_objects) {
            if (go.activeSelf) {
                Unspawn(go);
            }
        }
    }

    //是否包含子池子
    public bool Contains(GameObject go) => m_objects.Contains(go);

}

}