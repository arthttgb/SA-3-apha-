using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float maxDistance = 100f;
    public float damageAmount = 10f;

    private Vector3 startPos;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Bala sem Rigidbody!");
    }

    void Start()
    {
        startPos = transform.position;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = transform.forward * speed;
        }

        // Ignora colisões com o jogador (todos os colliders do player)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var bulletCols = GetComponentsInChildren<Collider>();
            var playerCols = player.GetComponentsInChildren<Collider>();
            foreach (var bc in bulletCols)
                foreach (var pc in playerCols)
                    Physics.IgnoreCollision(bc, pc, true);
        }
    }

    void Update()
    {
        if (rb == null) return;

        if (rb.linearVelocity.magnitude > 0.1f)
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

    [Header("Visual")]
public GameObject damagePopupPrefab; // Arraste seu novo Prefab para cá no Inspector da Bala

void OnCollisionEnter(Collision collision)
{
    Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
    float finalDamage = 0;
    
    if (hitbox != null)
    {
        finalDamage = damageAmount * hitbox.multiplicadorDano;
        hitbox.AplicarDanoDiferenciado(damageAmount);
    }
    else if (collision.gameObject.TryGetComponent<Target>(out Target target))
    {
        finalDamage = damageAmount;
        target.TakeDamage(damageAmount);
    }

    // SE DEU DANO, MOSTRA O NÚMERO
    if (finalDamage > 0)
    {
        ShowDamage(finalDamage, collision.contacts[0].point);
    }

    Destroy(gameObject);
}

void ShowDamage(float amount, Vector3 position)
{
    if (damagePopupPrefab == null) return;

    // Cria o número na posição do impacto
    GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
    
    // Se for dano alto (headshot), coloca cor amarela, senão branca
    Color textColor = amount > damageAmount ? Color.yellow : Color.white;
    
    popup.GetComponent<DamagePopup>().Setup(amount, textColor);
}
}
