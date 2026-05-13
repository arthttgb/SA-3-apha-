using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public float fireCooldown = 0.1f;

    float lastShotTime = -10f;

    void Start()
    {
        // se já existir GunSystem na hierarquia, desabilita este script para evitar disparos duplicados
        if (GetComponentInParent<GunSystem>() != null || GetComponentInChildren<GunSystem>() != null)
        {
            Debug.Log("ProjectileShooter desativado: GunSystem presente na hierarquia.");
            enabled = false;
            return;
        }

        if (firePoint == null && Camera.main != null)
            firePoint = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastShotTime < fireCooldown) return;
            lastShotTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("ProjectilePrefab ou firePoint não atribuídos.");
            return;
        }

        Vector3 spawnPos = firePoint.position + firePoint.forward * 0.4f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, firePoint.rotation);

        // fallback visual para testes
        if (proj.GetComponentInChildren<Renderer>() == null)
        {
            GameObject dbg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dbg.transform.SetParent(proj.transform, false);
            dbg.transform.localPosition = Vector3.zero;
            dbg.transform.localScale = Vector3.one * 0.12f;
            Destroy(dbg.GetComponent<Collider>());
        }

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.constraints = RigidbodyConstraints.None;
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
        else
        {
            // fallback: cria e configura um Rigidbody em runtime
            var addedRb = proj.AddComponent<Rigidbody>();
            addedRb.useGravity = false;
            addedRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            addedRb.linearVelocity = firePoint.forward * projectileSpeed;
            Debug.LogWarning("Prefab não tinha Rigidbody — adicionado em runtime.");
        }

        Debug.DrawRay(spawnPos, firePoint.forward * 2f, Color.red, 1f);
    }
}