using UnityEngine;
using System.Collections;
using PowerInject;

namespace Uhyre1
{
    [Insert]
    public class Monster : MonoBehaviour
    {
        private float speed = 8f;

        [Inject]
        private Player player;

        [Inject]
        private GameState gameState;

        private Rigidbody actor;

        private void Start()
        {
            actor = GetComponent<Rigidbody>();
        }

        private void move()
        {
            var pos = transform.position;
            var force = new Vector3();
            var playerPos = player.transform.position;
            if(playerPos.z < pos.z) {
                force.z = -1;
            }
            else {
                force.z = 1;
            }
            if(playerPos.x < pos.x) {
                force.x = -1;
            }
            else {
                force.x = 1;
            }
            actor.AddForce(force * speed);
        }

        private bool canMove()
        {
            return !gameState.PlayerIsDead & !gameState.PlayerWon;
        }

        private void FixedUpdate()
        {
            if(canMove()) {
                move();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<Player>()) {
                gameState.PlayerIsDead = true;
            }
        }
    }
}