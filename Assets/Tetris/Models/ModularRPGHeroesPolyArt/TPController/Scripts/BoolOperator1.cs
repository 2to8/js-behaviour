using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolOperator1 : StateMachineBehaviour
{

    public string boolName;        
    public bool status;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
            animator.SetBool(boolName, status);
            animator.ResetTrigger("combo");     //reseting combo trigger.
            animator.ResetTrigger("roll");      //resetng roll trigger.
    }

}
