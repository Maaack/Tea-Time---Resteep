using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshUpdater : MonoBehaviour
{
    public TMP_Text textObject;
    public void SetText(string newText)
    {
        textObject.text = newText;
    }
}
