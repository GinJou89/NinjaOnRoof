using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public GameObject StartObject;
    public GameObject[] ObstaclesPlatforms;
    public Transform PlatformContainer;
    private Transform _endPossition = null;

    void Start()
    {
        StartPlatfomsBuild();
    }
    private void StartPlatfomsBuild() 
    {
        CreateStartPlatform();
        for (int i = 0; i < 5; i++)
        {
            CreatePlatform();
        }
    }
    private void CreateStartPlatform()
    {
        GameObject platform = Instantiate(StartObject, transform.position, Quaternion.identity, PlatformContainer);
        _endPossition = platform.GetComponent<PlatformController>().EndPoint;
    }
    public void CreatePlatform() 
    {
        GameObject obj = ObstaclesPlatforms[Random.Range(0, ObstaclesPlatforms.Length)];
        GameObject platform = Instantiate(obj, _endPossition.position, Quaternion.identity, PlatformContainer);
        _endPossition = platform.GetComponent<PlatformController>().EndPoint; 
    }
}
