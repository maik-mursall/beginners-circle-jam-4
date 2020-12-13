using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinn : MonoBehaviour
{
    public List<GameObject> wheels;
    [SerializeField] private float wheelRadius = 0.5f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0);
    [SerializeField] private Vector3 forwardVector = new Vector3(0, -1, 0);

    [SerializeField] private float speed = 0;
    private Vector3 lastPosition = Vector3.zero;
    void Update()
    {
        float movedDistance = (transform.position - lastPosition).magnitude;
        speed = Mathf.Clamp(movedDistance / Time.deltaTime, 0, 100);
        lastPosition = transform.position;

        float circumference = wheelRadius * 2 * Mathf.PI;
        float radRotation = circumference / (movedDistance * Mathf.PI *2);
        float degRotation = radRotation * Mathf.Rad2Deg;

        // forwards and backwards needs to be implemented
        // Debug.Log(Vector3.Dot(transform.position - lastPosition, forwardVector)); 
        int rotationDirection = Mathf.FloorToInt(Vector3.Dot(transform.position - lastPosition, forwardVector));

        foreach (GameObject go in wheels)
        {
            go.transform.Rotate(new Vector3(radRotation, 0, 0));
        }
    }
}
