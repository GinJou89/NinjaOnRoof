using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float Speed;
    public float DestroyPoint;
    public bool is_Destroy;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, Speed * Time.deltaTime);
        if (transform.position.z < DestroyPoint)
        {
            if (is_Destroy)
            {
                Destroy(gameObject);
            }

            else
            {
                transform.position = new Vector3(0, 0, 90);     
            }
        }
    }
}
