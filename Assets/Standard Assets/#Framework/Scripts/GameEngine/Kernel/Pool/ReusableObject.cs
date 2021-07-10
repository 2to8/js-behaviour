using UnityEngine;

namespace GameEngine.Kernel.Pool {

public abstract class ReusableObject : MonoBehaviour, IReusable {

    public abstract void OnSpawn();

    public abstract void OnUnspawn();

}

}