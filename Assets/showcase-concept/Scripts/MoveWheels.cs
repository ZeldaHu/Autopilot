using System.Collections.Generic;
using UnityEngine;

public class MoveWheels : MonoBehaviour
{
    public List<GameObject> gameObjectsToRotate = new List<GameObject>();
    public Vector3 rotationAxis = Vector3.up; // Default rotation axis is up (Y axis)
    public float rotationSpeed = 10f; // Default rotation speed

    void Update()
    {
        foreach (var gameObject in gameObjectsToRotate)
        {
            if (gameObject != null)
            {
                gameObject.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
