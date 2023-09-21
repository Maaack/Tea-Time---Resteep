using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeabagWrapper : MonoBehaviour
{
    public TeaFlavor currentFlavor;
    public List<flavorSpritePair> flavorTeaTags = new List<flavorSpritePair>();
    public SpriteRenderer teaBagWrapperSprite;
    public Animator teaBagWrapperAnimator;
    private Dictionary<TeaFlavor, Sprite> flavorTeaTagsDict = new Dictionary<TeaFlavor, Sprite>();
    public TeaFlavorEvent onTeaBagOpened;


    public void OpenTeabag(){
        onTeaBagOpened.Invoke(currentFlavor);
    }

    void Start()
    {
        foreach(var pair in flavorTeaTags){
            flavorTeaTagsDict[pair.key] = pair.value;
        }
        teaBagWrapperSprite = GetComponentInChildren<SpriteRenderer>();
        teaBagWrapperAnimator = GetComponentInChildren<Animator>();
        teaBagWrapperSprite.sprite = flavorTeaTagsDict[currentFlavor];
    }

    public void PointerHoverEnter()
    {
        teaBagWrapperAnimator.SetBool("pointerHovering", true);
    }
    
    public void PointerHoverExit()
    {
        teaBagWrapperAnimator.SetBool("pointerHovering", false);
    }
}