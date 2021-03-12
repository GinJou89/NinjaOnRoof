using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform EndPoint;
    void Start()
    {
        WorldController.instanse.OnPlatformMovement += TryDelAndAddPlatform;
    }
    private void TryDelAndAddPlatform()
    {
        if (transform.position.z <= WorldController.instanse.MinZ)
        {
            WorldController.instanse.WorldBuilder.CreatePlatform();
            Destroy(gameObject);
        }
        
    }
    private void OnDestroy()
    {
        if (WorldController.instanse != null)
        {
            WorldController.instanse.OnPlatformMovement -= TryDelAndAddPlatform;
        }
    }
}
