using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;

    void Start()
    {
        // ⏱️ Destroi depois de um tempo (range)
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 💥 Ao bater em qualquer coisa
        Destroy(gameObject);
    }
}