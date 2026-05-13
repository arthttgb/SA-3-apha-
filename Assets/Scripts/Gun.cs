using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Configuração da Arma")]
    public string gunName = "Pistola";
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float bulletSpeed = 30f;
    public float damage = 10f;
    
    [Header("Munição")]
    public int maxAmmo = 30;
    private int currentAmmo;
    
    [Header("Efeitos")]
    public float recoilForce = 0.05f;
    public AudioClip shootSound;
    private AudioSource audioSource;

    private float nextFireTime = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();

        if (firePoint == null)
        {
            Debug.LogError("FirePoint não configurado!");
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab não configurado! Arraste a prefab de bala aqui.");
        }
    }

    void Update()
    {
        // Clique esquerdo para atirar
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        // R para recarregar
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // F para mostrar munição no console
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"[{gunName}] Munição: {currentAmmo}/{maxAmmo}");
        }
    }

    public void Shoot()
    {
        // Verifica se pode atirar
        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log($"[{gunName}] Sem munição! Pressione R para recarregar.");
            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab não atribuído!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint não atribuído!");
            return;
        }

        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        // Posição onde a bala vai aparecer
        Vector3 spawnPos = firePoint.position;
        
        // Direção do tiro (para onde a câmera está olhando)
        Vector3 shootDirection = mainCamera.transform.forward;

        // Cria a bala
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        if (bullet == null)
        {
            Debug.LogError("Erro ao criar bala!");
            return;
        }

        // Rotaciona a bala na direção do tiro
        bullet.transform.forward = shootDirection;

        // Configura velocidade da bala
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = shootDirection * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bala sem Rigidbody!");
        }

        // Efeitos de tiro
        PlayShootEffects();

        Debug.Log($"[{gunName}] ✓ Tiro! Munição: {currentAmmo}/{maxAmmo}");
    }

    void PlayShootEffects()
    {
        // Som
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Recuo da câmera
        StartCoroutine(RecoilAnimation());
    }

    IEnumerator RecoilAnimation()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        
        // Recua para trás
        mainCamera.transform.localPosition -= mainCamera.transform.forward * recoilForce;
        
        // Volta suavemente
        yield return new WaitForSeconds(0.05f);
        mainCamera.transform.localPosition = originalPos;
    }

    public void Reload()
    {
        if (currentAmmo == maxAmmo)
        {
            Debug.Log($"[{gunName}] Já está carregado!");
            return;
        }

        currentAmmo = maxAmmo;
        Debug.Log($"[{gunName}] ✓ Recarregado! Munição: {currentAmmo}/{maxAmmo}");
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public string GetGunName()
    {
        return gunName;
    }

    public float GetAmmoPercentage()
    {
        return (currentAmmo / (float)maxAmmo) * 100f;
    }
}
