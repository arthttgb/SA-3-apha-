using UnityEngine;
using TMPro;

public class ZombieKillCounter : MonoBehaviour
{
    public static ZombieKillCounter instance;

    public int kills = 0;

    [Header("UI")]
    public TextMeshProUGUI killText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddKill()
    {
        kills++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (killText != null)
            killText.text = "Zumbis mortos: " + kills;
    }
}