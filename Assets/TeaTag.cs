using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTag : MonoBehaviour
{
    public Rigidbody2D baseRigidBody2D;
    [Range(0,1000000)]
    public float forceMod = 1000.0f;
    [Range(0,10000)]
    public float maxForce = 25.0f;
    public bool negateCurrentVelocity = true;
    public bool followingPointer = true;
    private Vector3 moveToTarget;
    // Start is called before the first frame update
    void Start()
    {
        baseRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!followingPointer){
            return;
        }
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = -10;
        moveToTarget = worldPosition;
    }
    void FixedUpdate()
    {
        if (!followingPointer){
            return;
        }
        Vector3 diffVector = moveToTarget - gameObject.transform.position;
        diffVector *= forceMod;
        diffVector *= Math.Min(diffVector.magnitude, maxForce) / diffVector.magnitude;
        if (negateCurrentVelocity)
        {
            diffVector -= new Vector3(baseRigidBody2D.velocity.x, baseRigidBody2D.velocity.y, 0);
        }
        baseRigidBody2D.AddForce(diffVector, ForceMode2D.Impulse);

    }

}
