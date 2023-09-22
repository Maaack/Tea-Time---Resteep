using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenePhaseController : MonoBehaviour
{
    private Animator animator;
    private PlayableDirector playableDirector;
    public enum Phases{
        Intro,
        Waiting,
        Steeping,
        Return,
        Finished
    }
    public Phases currentPhase = Phases.Intro;


    void Start()
    {
        animator = GetComponent<Animator>();
        playableDirector = GetComponent<PlayableDirector>();
    }
    public void TimelineFinished()
    {
        switch(currentPhase)
        {
            case Phases.Intro:
                animator.SetTrigger("startWaiting");
                break;
            // case Phases.Steeping:
            //     animator.SetTrigger("startReturn");
            //     break;
            case Phases.Return:
                animator.SetTrigger("startFinished");
                break;
            default:
                break;
        }
    }

    public void SkipAhead()
    {
        if (playableDirector.state == PlayState.Playing)
        {
            playableDirector.Stop();
        }
        switch(currentPhase)
        {
            case Phases.Waiting:
                animator.SetTrigger("startSteeping");
                break;
            case Phases.Steeping:
                animator.SetTrigger("startReturn");
                break;
            case Phases.Return:
                animator.SetTrigger("startFinished");
                break;
            default:
                break;
        }
    }
}
