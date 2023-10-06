using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFluidSimulation : MonoBehaviour
{
    public enum Output {Velocity, Vorticity, Divergence, Pressure, Final};
    public Material velocityMaterial;
    public Output showOutput = Output.Velocity;
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
        RenderTexture velocityTexture = RenderTexture.GetTemporary(source.width, source.height);
        if (runVelocity){
            Graphics.Blit(persistentRenderTexture, velocityTexture, velocityMaterial, 0);
        } else {
            Graphics.Blit(persistentRenderTexture, velocityTexture);
        }

        if (runViscosity)
        {
            // Step 2: Run viscosity passes
            for (int i = 0; i < numViscosityPasses; i++)
            {
                RenderTexture destinationTexture = RenderTexture.GetTemporary(source.width, source.height);
                destinationTexture.filterMode = FilterMode.Bilinear;

                Graphics.Blit(velocityTexture, destinationTexture, velocityMaterial, 1);

                RenderTexture.ReleaseTemporary(velocityTexture);
                velocityTexture = destinationTexture;
            }
        }

        RenderTexture vorticityTexture = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(velocityTexture, vorticityTexture, velocityMaterial, 2);
        RenderTexture divergenceTexture = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(vorticityTexture, divergenceTexture, velocityMaterial, 3);

        switch(showOutput)
        {
            case Output.Velocity:
                Graphics.Blit(velocityTexture, persistentRenderTexture);
                Graphics.Blit(velocityTexture, destination);
                break;
            case Output.Vorticity:
                Graphics.Blit(vorticityTexture, persistentRenderTexture);
                Graphics.Blit(vorticityTexture, destination);
                break;
            case Output.Divergence:
                Graphics.Blit(vorticityTexture, persistentRenderTexture);
                Graphics.Blit(divergenceTexture, destination);
                break;
            case Output.Pressure:
                Graphics.Blit(velocityTexture, persistentRenderTexture);
                Graphics.Blit(velocityTexture, destination);
                break;
            default:
                break;


        }
        RenderTexture.ReleaseTemporary(velocityTexture);
        RenderTexture.ReleaseTemporary(vorticityTexture);
        RenderTexture.ReleaseTemporary(divergenceTexture);

    }
    
}
