using UnityEngine;
using System.Collections;
using PowerInject;

namespace Uhyre1
{
    public class GameState
    {
        [Inject]
        private Labyrinth labyrinth;

        public bool PlayerIsDead { get; set; }
        public bool PlayerWon => labyrinth.NumberOfDots == 0;
    }
}