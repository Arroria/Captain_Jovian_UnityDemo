using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class asm_bboss_bike_out : StateMachineBehaviour
{
    //public UnityEvent stateEndEvent;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { animator.GetComponent<BBoss>().BikeOutEndEvent(); } //{    stateEndEvent.Invoke(); }
}
