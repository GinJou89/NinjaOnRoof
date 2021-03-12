using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    public float Speed;
    public ParticleSystem DestroyEffect;
    void Update()
    {
        transform.position += Vector3.forward * Speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime * 10);
        if (transform.position.z > 30)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrier"))
        {
            Debug.Log("Suriken destroy");
            DestroyEffect.Play();
            Destroy(gameObject);
        }
    }
}
