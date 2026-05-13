using UnityEngine;
using TMPro; // Necessário para o texto moderno do Unity

public class DamagePopup : MonoBehaviour
{
    public float speed = 2f;        // Velocidade de subida
    public float duration = 1f;     // Quanto tempo dura na tela
    private TextMeshPro textMesh;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, Color color)
    {
        textMesh.text = damageAmount.ToString();
        textMesh.color = color;
        timer = duration;
    }

    void Update()
    {
        // Sobe o texto
        transform.position += Vector3.up * speed * Time.deltaTime;
        
        // Faz o texto olhar sempre para a câmera do jogador
        transform.LookAt(transform.position + Camera.main.transform.forward);

        // Timer para sumir
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // Efeito de sumir aos poucos (Fade out)
            float alpha = timer / duration;
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            
            if (timer <= -0.5f) Destroy(gameObject);
        }
    }
}