using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TeaFlavor
{
    Blueberry,
    Mint,
    Chamomile,
    Rose,
    Elderberry
}

[Serializable]
public class flavorSpritePair {
    public TeaFlavor key;
    public Sprite value;
}

public class TeaTagFront : MonoBehaviour
{
    private TeaFlavor _currentFlavor;
    public TeaFlavor currentFlavor
    {
        get { return _currentFlavor;}
        set 
        {
            _currentFlavor = value;
            teaTagSprite.sprite = flavorTeaTagsDict[_currentFlavor]; 
        }
    }
    public List<flavorSpritePair> flavorTeaTags = new List<flavorSpritePair>();
    public SpriteRenderer teaTagSprite;
    private Dictionary<TeaFlavor, Sprite> flavorTeaTagsDict = new Dictionary<TeaFlavor, Sprite>();
    void Awake()
    {
        foreach(var pair in flavorTeaTags){
            flavorTeaTagsDict[pair.key] = pair.value;
        }
    }

    void Start()
    {
        teaTagSprite = GetComponent<SpriteRenderer>();
    }
}
