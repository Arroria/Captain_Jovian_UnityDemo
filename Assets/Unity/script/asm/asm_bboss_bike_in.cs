using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class asm_bboss_bike_in : StateMachineBehaviour
{
    //public UnityEvent stateEndEvent;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)    { animator.GetComponent<BBoss>().BikeInEndEvent(); } //{    stateEndEvent.Invoke(); }
}
