using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public void ApplyVelocityForce(Vector2 position, Vector2 vector) 
    {
        spriteRenderer.material.SetVector("_BrushScale", new Vector2(0.5f, 0.5f));
        spriteRenderer.material.SetVector("_BrushCenterUV", position);
        spriteRenderer.material.SetVector("_BrushColor", new Vector4(vector.x, vector.y, 0, 1));
        spriteRenderer.material.SetInt("_BrushOn", 1);
    
    }
    public void RemoveVelocityForce()
    {
        spriteRenderer.material.SetVector("_BrushColor", Color.black);
        spriteRenderer.material.SetInt("_BrushOn", 0);       
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
