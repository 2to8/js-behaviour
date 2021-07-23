using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    public class ParticlePool : Manager<ParticlePool>
    {
        public GameObject effectPrefab;
        public int amount = 10;
        private ParticleSystem[] pool;
        private int currentSystem = 0;
        void OnEnable() { }

        public override void Start()
        {
            base.Start();
            pool = new ParticleSystem[amount];
            for (int i = 0; i < amount; i++) {
                pool[i] = GameObject.Instantiate<GameObject>(effectPrefab, this.transform)
                    .GetComponent<ParticleSystem>();
            }
        }

        public void UseParticles(Vector3 pos)
        {
            currentSystem = (currentSystem + 1 >= pool.Length) ? 0 : currentSystem + 1;
            pool[currentSystem].transform.position = pos;
            pool[currentSystem].Play();
        }
    }
}