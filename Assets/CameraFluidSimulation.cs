using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFluidSimulation : MonoBehaviour
{
    public Material velocityMaterial;
    public bool runVelocity = true;
    public bool runViscosity = true;
    public int numViscosityPasses = 50;
    private RenderTexture persistentRenderTexture;
    public void ApplyVelocityForce(Vector2 position, Vector2 vector) 
    {
        velocityMaterial.SetVector("_BrushScale", new Vector2(0.5f, 0.5f));
        velocityMaterial.SetVector("_BrushCenterUV", position);
        velocityMaterial.SetVector("_BrushColor", new Vector4(vector.x, vector.y, 0, 1));
        velocityMaterial.SetInt("_BrushOn", 1);
    
    }
    public void RemoveVelocityForce()
    {
        velocityMaterial.SetVector("_BrushColor", Color.black);
        velocityMaterial.SetInt("_BrushOn", 0);  
    }

    void Start()
    {
        persistentRenderTexture = new RenderTexture(Screen.width, Screen.height, 0);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture sourceTexture = RenderTexture.GetTemporary(source.width, source.height);
        if (runVelocity){
            Graphics.Blit(persistentRenderTexture, sourceTexture, velocityMaterial, 0);
        } else {
            Graphics.Blit(persistentRenderTexture, sourceTexture);
        }

        if (runViscosity)
        {
            // Step 2: Run viscosity passes
            for (int i = 0; i < numViscosityPasses; i++)
            {
                RenderTexture destinationTexture = RenderTexture.GetTemporary(source.width, source.height);
                destinationTexture.filterMode = FilterMode.Bilinear;

                Graphics.Blit(sourceTexture, destinationTexture, velocityMaterial, 1);

                RenderTexture.ReleaseTemporary(sourceTexture);
                sourceTexture = destinationTexture;
            }
        }

        // Display the result
        Graphics.Blit(sourceTexture, persistentRenderTexture);
        Graphics.Blit(sourceTexture, destination);
        RenderTexture.ReleaseTemporary(sourceTexture);
    }
    
}
