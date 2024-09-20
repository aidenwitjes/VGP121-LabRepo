using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.5f;
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        //Reference to the main camera's transform
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        //Calculate the difference in camera movement
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        //Move the background in proportion to the camera's movement, but slower based on parallaxFactor
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);

        //Update the previousCameraPosition to the new position for the next frame
        previousCameraPosition = cameraTransform.position;
    }
}