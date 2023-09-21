using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

[System.Serializable]
public class DirectorStoppedEvent : UnityEvent {}
    
public class PlayableDirectorCallback : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public DirectorStoppedEvent directorStoppedEvent;
    void OnPlayableDirectorStopped(PlayableDirector stoppedDirector){
        if (stoppedDirector == playableDirector)
        {
            directorStoppedEvent.Invoke();
        }
    }

    void OnEnable()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnDisable()
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }

}
