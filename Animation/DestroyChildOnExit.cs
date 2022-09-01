using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.transform.gameObject, stateInfo.length);
    }
}