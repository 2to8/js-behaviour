using UnityEngine;

namespace GameEngine.Updater.Tests {

class MyScript : MonoBehaviour {
    IUpdater updater;
    IPlayer player;
    DamageOverTime dot;

    void Awake()
    {
        // Make the updater
        updater = new CoroutineUpdater();

        // Give the updater a MonoBehaviour. This starts it.
        ((CoroutineUpdater)updater).MonoBehaviour = this;

        // Give a non-MonoBehaviour the updater
        dot = new DamageOverTime(updater, player);

        // Give the updater a new MonoBehaviour, perhaps because this one is
        // being destroyed due to a scene being loaded
        //updater.MonoBehaviour = someOtherMonoBehaviour;
    }
}

}