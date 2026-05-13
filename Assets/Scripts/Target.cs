using UnityEngine;
using UnityEngine.AI; // Necessário para o NavMesh funcionar

public class Target : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public float health = 100f;
    public string playerTag = "Player";

    [Header("Configurações de Movimento")]
    public float speed = 3f;
    public float stopDistance = 1.5f;

    private Transform player;
    private NavMeshAgent agent; // Nosso "motor" de movimento
    private bool hasTouched = false;

    void Start()
    {
        // 1. Pegamos o componente NavMeshAgent que você adicionou no objeto
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stopDistance;

        // 2. Localizamos o Player
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        // Se o player sumir ou o zumbi morrer, para tudo
        if (player == null || health <= 0f) return;

        // 3. MANDAR O ZUMBI ANDAR:
        // O NavMeshAgent cuida de subir morros e rotacionar o corpo sozinho
        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        // 4. Lógica de Dano (Sua lógica original melhorada)
        if (dist <= stopDistance + 0.3f) // Se chegou perto
        {
            if (!hasTouched)
            {
                Atacar();
            }
        }
        else
        {
            // Se o player se afastar, o zumbi pode atacar de novo depois
            hasTouched = false;
        }
    }

    void Atacar()
    {
        hasTouched = true;
        player.SendMessage("TakeDamage", 10f, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Zumbi mordeu o player!");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    void Die()
    {
        if (ZombieKillCounter.instance != null)
            ZombieKillCounter.instance.AddKill();

        Destroy(gameObject);
    }
}