using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolOperator2 : StateMachineBehaviour
{

    public string boolName;
        
    public bool status;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, !status);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, !status);

    }
    /*
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
        
	}
    */
    


}
