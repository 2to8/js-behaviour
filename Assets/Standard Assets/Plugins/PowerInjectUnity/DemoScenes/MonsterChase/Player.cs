using UnityEngine;
using System.Collections;
using PowerInject;

namespace Uhyre1
{
    [Insert]
    public class Player : MonoBehaviour
    {
        private float moveStep = 0.1f;

        [Inject]
        private ISelectedMoves controls;

        [Inject]
        private Labyrinth labyrinth;

        [Inject]
        private PointsKeeper pointsKeeper;

        private Rigidbody actor;
        private int moveForce = 100;

        private void move(float x, float z)
        {
            var pos = transform.position;
            var force = new Vector3(x, 0, z) * moveForce;
            actor.AddForce(force);
        }

        private void moveForward()
        {
            move(moveStep, 0);
        }

        private void moveBack()
        {
            move(-moveStep, 0);
        }

        private void moveLeft()
        {
            move(0, moveStep);
        }

        private void moveRight()
        {
            move(0, -moveStep);
        }

        public void FixedUpdate()
        {
            var move = controls.getControl();

            if(move.Contains("forward")) {
                moveForward();
            }
            if(move.Contains("back")) {
                moveBack();
            }
            if(move.Contains("left")) {
                moveLeft();
            }
            if(move.Contains("right")) {
                moveRight();
            }
        }

        [OnInjected]
        public void InitPlayer()
        {
            var x = labyrinth.transform.position.x + 1;
            var y = labyrinth.transform.position.y;
            var z = labyrinth.transform.position.z + 3;
            transform.position = new Vector3(x, y, z);
            actor = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.GetComponent<Dot>()) {
                collision.gameObject.active = false;
                pointsKeeper.addPoints(5);
                labyrinth.NumberOfDots -= 1;
            }
        }
    }
}