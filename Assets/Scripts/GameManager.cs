// using UnityEngine;
// using TMPro;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
//     public TMP_Text textUI; // Gabungan teks Koin & Info
    
//     [Header("Data Game")]
//     public int koinTerkumpul = 0;
//     public int targetTrigger = 2; // Ganti jadi 99 nanti
//     public int totalKoinFinal = 100; // Total setelah kunci diambil

//     [Header("Referenced Objects")]
//     public GameObject prefabKunci;   // Drag Prefab KUNCI/Koin ke-100 kesini
//     public PhoneController phoneScript; // GANTI: Drag script PhoneController kesini
//     public EnemyAI enemyScript; 

//     void Awake() { if (instance == null) instance = this; }

//     void Start() 
//     { 
//         // Cari script musuh otomatis
//         if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
        
//         // Cari script PhoneController otomatis jika belum diisi
//         if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

//         // Pastikan Kunci & Pintu sembunyi dulu di awal
//         if (prefabKunci != null) prefabKunci.SetActive(false);
        
//         UpdateUI("Cari jalan keluar... (Tekan M untuk Peta)");
//     }

//     public void TambahKoin()
//     {
//         koinTerkumpul++;
        
//         // FASE 1: TRIGGER CLIMAX (Saat Koin 99 / atau 2 saat testing)
//         if (koinTerkumpul == targetTrigger)
//         {
//             TriggerClimaxMode();
//         }
//         // FASE 2: MENANG (Saat Kunci diambil/Koin 100)
//         else if (koinTerkumpul >= totalKoinFinal)
//         {
//             UpdateUI("KUNCI DITEMUKAN! CARI PINTU KELUAR!");
//             // Disini logika buka pintu aktif
//         }
//         else
//         {
//             UpdateUI("Koin: " + koinTerkumpul);
//         }
//     }

//     void TriggerClimaxMode()
//     {
//         Debug.Log("SISTEM ERROR! SINYAL HILANG! ANGEL MARAH!");

//         // 1. Matikan Map/HP lewat PhoneController
//         if (phoneScript != null) 
//         {
//             phoneScript.ForceClosePhoneAndDisable();
//             // Tambahkan efek suara statis/rusak disini nanti
//         }

//         // 2. Munculkan Kunci (Koin Terakhir)
//         if (prefabKunci != null)
//         {
//             prefabKunci.SetActive(true);
//             // Tips: Sebaiknya spawn kunci di tengah map atau tempat terjauh
//             prefabKunci.transform.position = new Vector3(0, 1, 0); 
//         }

//         // 3. Angel Marah
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
        
//         // 4. Munculkan Pintu Exit
//         MunculkanPintu();

//         UpdateUI("SINYAL HILANG... LARI!!!");
//     }

//     void MunculkanPintu()
//     {
//         MazeGeneratorPerfectLoop generator = FindAnyObjectByType<MazeGeneratorPerfectLoop>();
//         if (generator != null)
//         {
//             foreach (Transform child in generator.transform)
//             {
//                 if (child.CompareTag("Pintu"))
//                 {
//                     child.gameObject.SetActive(true);
//                     break;
//                 }
//             }
//         }
//     }

//     void UpdateUI(string pesan) 
//     { 
//         if (textUI != null) textUI.text = pesan; 
//     }
// }





using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Wajib untuk Restart Game

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text textUI; 
    
    [Header("Data Game")]
    public int koinTerkumpul = 0;
    public int targetTrigger = 99; 
    public int totalKoinFinal = 100; 

    [Header("Referenced Objects")]
    public GameObject prefabKunci;   
    public PhoneController phoneScript; 
    public EnemyAI enemyScript; 
    
    [Header("UI Game Over")]
    public GameObject panelGameOver; // Drag Panel Hitam tadi kesini

    // Status Game
    public bool isGameOver = false;

    void Awake() { if (instance == null) instance = this; }

    void Start() 
    { 
        // Cari script otomatis
        if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
        if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

        if (prefabKunci != null) prefabKunci.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false); // Pastikan mati di awal
        
        UpdateUI("Cari jalan keluar... (Tekan M untuk Peta)");
        Time.timeScale = 1; // Pastikan waktu jalan
    }

    public void TambahKoin()
    {
        if (isGameOver) return; // Kalau sudah kalah, gak bisa ambil koin

        koinTerkumpul++;
        
        if (koinTerkumpul == targetTrigger)
        {
            TriggerClimaxMode();
        }
        else if (koinTerkumpul >= totalKoinFinal)
        {
            UpdateUI("KUNCI DITEMUKAN! CARI PINTU KELUAR!");
        }
        else
        {
            UpdateUI("Koin: " + koinTerkumpul);
        }
    }

    void TriggerClimaxMode()
    {
        if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        if (prefabKunci != null) 
        {
            prefabKunci.SetActive(true);
            prefabKunci.transform.position = new Vector3(0, 1, 0); 
        }
        if (enemyScript != null) enemyScript.ActivateWeepingMode();
        MunculkanPintu();
        UpdateUI("SINYAL HILANG... LARI!!!");
    }

    // --- FUNGSI BARU: GAME OVER (DIPANGGIL OLEH ANGEL) ---
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("ANDA TERTANGKAP!");

        // 1. Munculkan Layar Hitam
        if (panelGameOver != null) panelGameOver.SetActive(true);

        // 2. Matikan Kontrol Player (Biar gak bisa jalan lagi)
        Cursor.lockState = CursorLockMode.None; // Munculkan kursor mouse
        Cursor.visible = true;
        
        // 3. Matikan Waktu (Game Pause)
        // Kita beri delay sedikit nanti biar ada efek kaget, tapi untuk sekarang pause dulu
        Time.timeScale = 0; 
    }

    public void RestartGame()
    {
        // Reload Scene saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    void MunculkanPintu()
    {
        MazeGeneratorPerfectLoop generator = FindAnyObjectByType<MazeGeneratorPerfectLoop>();
        if (generator != null)
        {
            foreach (Transform child in generator.transform)
            {
                if (child.CompareTag("Pintu"))
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void UpdateUI(string pesan) 
    { 
        if (textUI != null) textUI.text = pesan; 
    }
}