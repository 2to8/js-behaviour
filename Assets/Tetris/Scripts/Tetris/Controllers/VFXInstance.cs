using Common;
using Saro;
using UnityEngine;

namespace Tetris
{
    public class VFXInstance : View<VFXInstance>, IPoolable<VFXInstance>
    {
        public bool InPooled { get; set; }
        public Pool<VFXInstance> Pool { get; set; }
        new ParticleSystem particleSystem;
        Timer timer;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            particleSystem.Play();
            if (timer == null) timer = Timer.Register(particleSystem.main.duration, Free, null, false);
            timer.Restart();
        }

        void Free()
        {
            particleSystem.Stop();
            Pool.Free(this);
        }
    }
}