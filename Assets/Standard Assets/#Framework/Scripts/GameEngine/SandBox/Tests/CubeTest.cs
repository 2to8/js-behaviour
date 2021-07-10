using UnityEngine;

namespace GameEngine.SandBox.Tests {

public class CubeTest : MonoBehaviour {

    float timer;
    public GameObject wall;

    void Update()
    {
        timer += Time.deltaTime;

        if (wall.activeInHierarchy) {
            if (timer > 5f) {
                timer = 0f;
                wall.SetActive(false);
            }
        } else {
            if (timer > 10f) {
                timer = 0f;
                wall.SetActive(true);
            }
        }
    }

}

}