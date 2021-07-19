using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{

    public class AnimatorMatcher : MonoBehaviour
    {
        Animator anim;
        ControlManager control;
        Vector3 dPosition;
        Vector3 vPosition;


        void Start()
        {
            anim = GetComponent<Animator>();
            control = GetComponentInParent<ControlManager>();
        }

        
        private void OnAnimatorMove()   //It updates every frame when animator's animations in play.
        {
            if (control.canMove)
                return;

            if (!control.onGround)
                return;

            control.rigid.drag = 0;
            float multiplier = 3f;

            dPosition = anim.deltaPosition;   //storing delta positin of active model's position.         
            
            dPosition.y = 0f;   //flatten the Y (height) value of root animations.
            
            vPosition = (dPosition * multiplier) / Time.fixedDeltaTime;     //defines the vector 3 value for the velocity.      

            
            control.rigid.velocity = vPosition; //This will move the root gameObject for matching active model's position.
            

        }

        
    }
}