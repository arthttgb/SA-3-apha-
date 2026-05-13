using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Conexões")]
    public Target scriptPrincipal; // Arraste o objeto "Pai" do zumbi que tem o script Target

    [Header("Configurações")]
    public float multiplicadorDano = 1f; // 1 para corpo, 10 para cabeça (Hit Kill)

    public void AplicarDanoDiferenciado(float danoBase)
    {
        if (scriptPrincipal != null)
        {
            scriptPrincipal.TakeDamage(danoBase * multiplicadorDano);
        }
    }
}