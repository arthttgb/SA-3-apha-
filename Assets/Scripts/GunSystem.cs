using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    public Camera fpsCam;
    public float damage = 10f;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 50f;

    public float fireCooldown = 0.1f;
    float lastShotTime = -10f;

    public float spawnOffset = 0.4f;

    // 🔊 AUDIO
    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    static bool prefabWarningLogged = false;

    void Awake()
    {
        if (fpsCam == null)
            fpsCam = Camera.main;

        var all = FindObjectsOfType<GunSystem>();
        if (all.Length > 1)
        {
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != all[0])
                {
                    Debug.LogWarning($"GunSystem duplicado em '{all[i].name}', desativando.");
                    all[i].enabled = false;
                }
            }
        }
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Time.time - lastShotTime >= fireCooldown)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    void Shoot()
    {
        if (fpsCam == null) return;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        // 🔊 SOM DO TIRO
        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(shootSound);
        }

        Transform spawnT = bulletSpawnPoint != null ? bulletSpawnPoint : fpsCam.transform;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = ray.GetPoint(100f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;

        Vector3 direction = (targetPoint - spawnT.position).normalized;
        Vector3 spawnPos = spawnT.position + direction * spawnOffset;

        if (bulletPrefab == null)
        {
            if (!prefabWarningLogged)
            {
                Debug.LogWarning("bulletPrefab não atribuído!");
                prefabWarningLogged = true;
            }
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(direction));

        var shooterCols = GetComponentsInChildren<Collider>();
        var bulletCols = bullet.GetComponentsInChildren<Collider>();

        if (bulletCols.Length == 0)
        {
            var c = bullet.GetComponent<Collider>();
            if (c != null)
                bulletCols = new Collider[] { c };
        }

        foreach (var bc in bulletCols)
            foreach (var sc in shooterCols)
                Physics.IgnoreCollision(bc, sc, true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        else
        {
            var addedRb = bullet.AddComponent<Rigidbody>();
            addedRb.useGravity = false;
            addedRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            addedRb.linearVelocity = direction * bulletSpeed;
        }

        Debug.DrawRay(spawnPos, direction * 2f, Color.red, 1f);
    }
}