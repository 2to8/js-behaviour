using GameEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Kernel.Pool {

public class ObjectPool : SingletonBaseView<ObjectPool> {

    readonly Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();
    public string ResourceDir = "";

    //取对象
    public GameObject Spawn(string name)
    {
        if (!m_pools.ContainsKey(name)) {
            CreateNewPool(name);
        }

        var pool = m_pools[name];

        return pool.Spawn();
    }

    //回收对象
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;

        foreach (var p in m_pools.Values) {
            if (p.Contains(go)) {
                pool = p;

                break;
            }
        }

        pool.Unspawn(go);
    }

    //回收所有对象
    public void UnspawnAll()
    {
        foreach (var p in m_pools.Values) {
            p.UnspawnAll();
        }
    }

    //创建新子池子
    void CreateNewPool(string name)
    {
        //预设的位置
        var path = "";

        if (string.IsNullOrEmpty(ResourceDir)) {
            path = name;
        } else {
            path = ResourceDir + "/" + name;
        }

        //加载预设
        var prefab = Resources.Load<GameObject>(path);

        //创建子对象池
        var pool = new SubPool(transform, prefab);
        m_pools.Add(pool.Name, pool);
    }

}

}