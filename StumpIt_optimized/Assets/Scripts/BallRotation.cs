 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotation : MonoBehaviour
{
    public float rotationSpeed = 50f;
    SwipeBallManager swipeBallManager;
    public Vector3 rotationAxis = new Vector3(0.5f, 0.5f, 0.5f);
    private void Start()
    {
        swipeBallManager = GetComponent<SwipeBallManager>();
    }
    void Update()
    {
        if(swipeBallManager.ballInAir==false)
        {
            transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime);
        }
        // Rotate the sphere continuously around its y-axis
    }
}
