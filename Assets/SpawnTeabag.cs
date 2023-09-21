using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnTeabag : MonoBehaviour
{
    public GameObject teabagPrefab;
    public bool inputEnabled = false;
    private GameObject currentTeaBag;
    public void OpenTeabag(TeaFlavor newTeaFlavor)
    {
        if (!inputEnabled)
        {
            return;
        }
        DiscardTeaBag();
        currentTeaBag = Instantiate(teabagPrefab);
        TeaTagFront teaTagFront = currentTeaBag.GetComponentInChildren<TeaTagFront>();
        teaTagFront.currentFlavor = newTeaFlavor;
    }

    private void DiscardTeaBag()
    {
        if (currentTeaBag is null)
        {
            return;
        }
        TeaTag currentTeaTag = currentTeaBag.GetComponentInChildren<TeaTag>();
        currentTeaTag.followingPointer = false;
        GameObject discardedTeaBag = currentTeaBag.gameObject;
        DelayedDestroy delayedDestroy = currentTeaBag.GetComponentInChildren<DelayedDestroy>();
        delayedDestroy.DelayedSelfDestruct();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
