using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LoadTimeline : StateMachineBehaviour
{
    public TimelineAsset timelineAsset;
    public string uniqueString;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       PlayableDirector playableDirector = animator.GetComponent<PlayableDirector>();
       playableDirector.playableAsset = timelineAsset;
       playableDirector.Play();
    }

}
