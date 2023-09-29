using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TeaSteepingControl : MonoBehaviour
{
    FluidSimulator fluidSimulator;
    public void SteepTea()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vector = new(0.5f, 0.5f);
        Vector2 mousePosition = (Vector2)worldPosition;
        fluidSimulator.ApplyVelocityForce(mousePosition, vector);
    }
    // Start is called before the first frame update
    void Start()
    {
        fluidSimulator = GetComponentInChildren<FluidSimulator>();
    }

    // Update is called once per frame
    void Update()
    {
        SteepTea();
    }
}
