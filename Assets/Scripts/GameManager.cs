using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text textKoin; 
    public int koinTerkumpul = 0;
    public int targetKoin = 100;
    public EnemyAI enemyScript; 

    void Awake() { if (instance == null) instance = this; }
    void Start() 
    { 
        UpdateUI();
        // Pasang Minimap otomatis
        if (GetComponent<MinimapCamera>() == null)
            gameObject.AddComponent<MinimapCamera>();
    }

    public void TambahKoin()
    {
        koinTerkumpul++;
        UpdateUI();

        // SAAT KOIN 99 (KLIMAKS)
        if (koinTerkumpul == 2)
        {
            Debug.Log("PINTU MUNCUL! ANGEL MARAH!");
            
            // 1. Angel Marah
            if (enemyScript != null) enemyScript.ActivateWeepingMode();

            // 2. Munculkan Pintu (Cari di anak-anak MazeGenerator)
            MazeGeneratorPerfectLoop generator = FindAnyObjectByType<MazeGeneratorPerfectLoop>();
            if (generator != null)
            {
                foreach (Transform child in generator.transform)
                {
                    if (child.CompareTag("Pintu"))
                    {
                        child.gameObject.SetActive(true); // MUNCULKAN PINTU!
                        Debug.Log("PINTU DITEMUKAN DAN DIAKTIFKAN!");
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("MazeGenerator tidak ditemukan!");
            }
        }
    }

    void UpdateUI() { if (textKoin != null) textKoin.text = "Koin: " + koinTerkumpul + " / " + targetKoin; }
}