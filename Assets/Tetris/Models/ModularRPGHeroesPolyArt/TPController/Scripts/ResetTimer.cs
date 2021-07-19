using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimer : StateMachineBehaviour
{
    public string timerName;    //timer for combo timing.
    float time;
    
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        time += Time.deltaTime; //storing time passing.
        
        animator.SetFloat(timerName, time); //setting up "timerName"'s float value according to timer variable.
        
    }

    //resetting timer.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        time = 0;
        
        animator.SetFloat(timerName, time);        

    }

}
