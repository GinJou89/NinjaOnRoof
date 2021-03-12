using UnityEngine;

public class BonusController : MonoBehaviour
{
    public ParticleSystem HitEffect;
    void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime * 3);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIcontroller.instanse.AddBonus(1);
            Destroy(gameObject);
        }
    }

}
