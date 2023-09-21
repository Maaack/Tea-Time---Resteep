using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{

    public void DelayedSelfDestruct(){
        Invoke(nameof(SelfDestruct), 5.0f);
    }

    void SelfDestruct(){
        Destroy(gameObject);
    }
}
