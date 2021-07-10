using UnityEngine;

namespace __Mask3D {

public class Mask3D_Object : MonoBehaviour {

    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

}

}