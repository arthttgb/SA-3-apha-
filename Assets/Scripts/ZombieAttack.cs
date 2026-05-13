using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    Transform player;
    PlayerHealth playerHealth;

    float lastAttackTime;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange && Time.time > lastAttackTime)
        {
            lastAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    void Attack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Zumbi atacou o jogador!");
        }
    }
}