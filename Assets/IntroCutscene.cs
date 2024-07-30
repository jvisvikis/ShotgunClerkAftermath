using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void StartIntro(GameObject obj)
    {
        obj.SetActive(false);
        director.Play();
    }
    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            ScenesManager.instance.LoadScene(1);
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
