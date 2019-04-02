using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInBounds : MonoBehaviour
{
    float xUpper = 5.8f;
    float xLower = -5.8f;
    float zUpper = 5.8f;
    float zLower = -5.8f;
    private void Update()
    {
        if (transform.position.x >= xUpper)
        {
            transform.position = new Vector3(xUpper, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= xLower)
        {
            transform.position = new Vector3(xLower, transform.position.y, transform.position.z);
        }
        if (transform.position.z >= zUpper)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zUpper);
        }
        if (transform.position.z <= zLower)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zLower);
        }
    }
}