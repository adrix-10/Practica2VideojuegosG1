using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroBehaviour : StateMachineBehaviour
{
    [SerializeField]
    string stateName;

    [SerializeField]
    float speed;

    [SerializeField]
    float maximumfollowTime;

    Transform target;
    AggroController controller;

    Vector3 originalPosition;

    float currentFollowTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<AggroController>();

        switch (stateName.ToLower())
        {
            case "flying":
                currentFollowTime = maximumfollowTime;
                target = GameObject.FindGameObjectWithTag("Player").transform;
                break;

            case "flying back":
                originalPosition = controller.GetOriginalPosition();
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (stateName.ToLower())
        {
            case "flying":
                animator.transform.position = Vector2.MoveTowards(animator.transform.position, target.position, speed * Time.deltaTime);
                controller.LookAt(target.position);
                currentFollowTime -= Time.deltaTime;
                if (currentFollowTime <= 0.0F)
                {
                    animator.SetTrigger("back");
                }
                break;

            case "flying back":
                animator.transform.position = Vector2.MoveTowards(animator.transform.position, originalPosition, speed * Time.deltaTime);
                controller.LookAt(originalPosition);
                if (animator.transform.position == originalPosition)
                {
                    animator.SetTrigger("stop");
                }
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
