using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawString : MonoBehaviour
{
    public LineRenderer stringRenderer;
    private List<Transform> transformList;
    // Start is called before the first frame update
    void Start()
    {
        transformList = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++){
            transformList.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform[] transforms = transformList.ToArray();
        Vector3[] points = new Vector3[transforms.Length];
        for(int i = 0; i < transforms.Length; i ++)
        {
            points[i] = transforms[i].position;
        }
        stringRenderer.positionCount = points.Length;
        stringRenderer.SetPositions(points);
    }
}
