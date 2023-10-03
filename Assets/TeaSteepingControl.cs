using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TeaSteepingControl : MonoBehaviour
{
    CameraFluidSimulation fluidSimulator;
    public Color brushColor = new Color(0.5f, 0.5f, 0, 1);
    public void SteepTea()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = (Vector2)worldPosition;
        fluidSimulator.ApplyVelocityForce(mousePosition, new Vector2(brushColor.r, brushColor.g));
    }
    // Start is called before the first frame update
    void Start()
    {
        fluidSimulator = GetComponentInChildren<CameraFluidSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        SteepTea();
    }
}
