using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public float Speed;
    public WorldBuilder WorldBuilder;
    public float MinZ = -10f;
    public float TimeToStart = 3.3f;

    public delegate void TryToDelAndAddPlatform();
    public event TryToDelAndAddPlatform OnPlatformMovement;

    public static WorldController instanse;
    private void Awake()
    {
        if (WorldController.instanse != null)
        {
            Destroy(gameObject);
            return;
        }
        WorldController.instanse = this;
    }
    private void Start()
    {
        StartCoroutine(OnPlaformMovementCorutine());
    }
    private void Update()
    {
        if (TimeToStart <= 0)
        {
            Move();
        }
        TimeToStart -= Time.deltaTime;

        if (UIcontroller.instanse.GetPoint() != 0 && UIcontroller.instanse.GetPoint() % 10 == 0)
        {
            Speed += 0.005f;
        }
    }
    private void OnDestroy()
    {
        WorldController.instanse = null;
    }
    private void Move()
    {
        transform.position -= Vector3.forward * Speed * Time.deltaTime;
    }
    IEnumerator OnPlaformMovementCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (OnPlatformMovement != null)
            {
                OnPlatformMovement();
            }
        }
    }
}
