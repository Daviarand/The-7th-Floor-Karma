// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
//     public TMP_Text textUI; 
    
//     [Header("Data Game")]
//     public int koinTerkumpul = 0;
//     public int targetTrigger = 99; 
//     public int totalKoinFinal = 100; 

//     [Header("Referenced Objects")]
//     public GameObject prefabKunci;   
//     public PhoneController phoneScript; 
//     public EnemyAI enemyScript; 

//     // --- PERUBAHAN DISINI ---
//     [Header("Pintu Keluar Manual")]
//     public GameObject pintuExitObject; // DRAG PINTU YANG ANDA PASANG KESINI
    
//     [Header("UI Game Over")]
//     public GameObject panelGameOver; 

//     public bool isGameOver = false;

//     void Awake() { if (instance == null) instance = this; }

//     void Start() 
//     { 
//         if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
//         if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

//         if (prefabKunci != null) prefabKunci.SetActive(false);
//         if (panelGameOver != null) panelGameOver.SetActive(false);
        
//         // Sembunyikan Pintu Exit di Awal Game secara otomatis
//         if (pintuExitObject != null) 
//         {
//             pintuExitObject.SetActive(false);
//         }
//         else
//         {
//             Debug.LogError("PERINGATAN: Anda belum memasukkan objek Pintu ke GameManager!");
//         }
        
//         UpdateUI("Cari jalan keluar... (Tekan M untuk Peta)");
//         Time.timeScale = 1; 
//     }

//     public void TambahKoin()
//     {
//         if (isGameOver) return;

//         koinTerkumpul++;
        
//         if (koinTerkumpul == targetTrigger)
//         {
//             TriggerClimaxMode();
//         }
//         else if (koinTerkumpul >= totalKoinFinal)
//         {
//             UpdateUI("KUNCI DITEMUKAN! CARI PINTU KELUAR!");
//             // Logika tambahan jika pintu perlu dibuka kuncinya bisa disini
//         }
//         else
//         {
//             UpdateUI("Koin: " + koinTerkumpul);
//         }
//     }

//     void TriggerClimaxMode()
//     {
//         if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        
//         if (prefabKunci != null) 
//         {
//             prefabKunci.SetActive(true);
//             prefabKunci.transform.position = new Vector3(0, 1, 0); 
//         }
        
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
        
//         // --- NYALAKAN PINTU MANUAL ---
//         if (pintuExitObject != null)
//         {
//             pintuExitObject.SetActive(true);
//             Debug.Log("Pintu Keluar Muncul!");
//         }

//         UpdateUI("SINYAL HILANG... LARI!!!");
//     }

//     public void TriggerGameOver()
//     {
//         if (isGameOver) return;
//         isGameOver = true;
//         Debug.Log("ANDA TERTANGKAP!");
//         if (panelGameOver != null) panelGameOver.SetActive(true);
//         Cursor.lockState = CursorLockMode.None; 
//         Cursor.visible = true;
//         Time.timeScale = 0; 
//     }

//     public void RestartGame()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//         Time.timeScale = 1;
//     }

//     void UpdateUI(string pesan) 
//     { 
//         if (textUI != null) textUI.text = pesan; 
//     }
// }







using UnityEngine;
using TMPro;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("UI Story")]
    public TMP_Text textUI;       
    public GameObject panelCerita; 

    [Header("Data Game")]
    public int koinTerkumpul = 0;
    public int targetTrigger = 99; // Pastikan ini 99
    public int koinTengahJalan = 50; 
    
    [Tooltip("Koordinat dimana Kunci akan muncul")]
    public Vector3 posisiMunculKunci = new Vector3(-6, 1, 0); 

    [Header("Referenced Objects")]
    public GameObject prefabKunci; // Drag PREFAB KOIN dari folder Project kesini
    public PhoneController phoneScript; 
    public EnemyAI enemyScript; 
    public GameObject pintuExitObject; 
    
    // Kecepatan Mengetik 
    private float typingSpeed = 0.03f; 
    private float bacaDelay = 3f; 

    private bool isClimax = false;
    private Coroutine currentStoryRoutine;

    void Awake() { if (instance == null) instance = this; }

    void Start() 
    { 
        if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
        if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

        // Kita tidak perlu menyembunyikan prefabKunci karena dia masih berupa file prefab
        if (pintuExitObject != null) pintuExitObject.SetActive(false);
        if (panelCerita != null) panelCerita.SetActive(false); 
        
        Time.timeScale = 1; 

        // --- INTRO ---
        string[] introTeks = {
            "Kepalaku sakit sekali... Di mana aku sekarang?",
            "Lantai 7... Konon tempat ini terkutuk. Aku harus segera mencari jalan keluar.",
            "Sinyal di sini buruk. Untung peta di HP masih berfungsi (Tekan M)."
        };
        PlaySequence(introTeks);
    }

    public void TambahKoin()
    {
        koinTerkumpul++;
        
        // --- Cek Koin Pertama ---
        if (koinTerkumpul == 1)
        {
            string[] koinPertamaTeks = {
                "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
                "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
            };
            PlaySequence(koinPertamaTeks);
        }
        
        // --- Cek Pertengahan ---
        else if (koinTerkumpul == koinTengahJalan)
        {
            PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
        }

        // --- Cek KLIMAKS (99 Koin) ---
        else if (koinTerkumpul == targetTrigger && !isClimax)
        {
            TriggerClimaxMode();
        }
        
        // --- Cek MENANG (100 Koin / Sudah ambil kunci) ---
        else if (koinTerkumpul >= 100) 
        {
            PlaySequence(new string[] { "KUNCI SUDAH KETEMU! Pintu Keluar Bisa Terbuka! AKU HARUS LARI!" });
        }
    }

    void TriggerClimaxMode()
    {
        isClimax = true;

        if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        
        // --- PERBAIKAN DI SINI: INSTANTIATE ---
        if (prefabKunci != null) 
        {
            // Kita "Lahirkan" kuncinya di koordinat yang sudah ditentukan
            Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
            Debug.Log("Kunci Muncul di: " + posisiMunculKunci);
        }
        
        if (enemyScript != null) enemyScript.ActivateWeepingMode();
        if (pintuExitObject != null) pintuExitObject.SetActive(true);

        // Teks Klimaks
        string[] climaxTeks = {
            "Sial! HP-ku mati! Sinyalnya hilang total!",
            "Suara apa itu? Sesuatu yang buruk sedang mendekat!",
            "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
        };
        PlaySequence(climaxTeks);
    }

    public void PlaySequence(string[] daftarKalimat)
    {
        if (currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
        currentStoryRoutine = StartCoroutine(SequenceProcess(daftarKalimat));
    }

    IEnumerator SequenceProcess(string[] lines)
    {
        if (panelCerita != null) panelCerita.SetActive(true);
        foreach (string kalimat in lines)
        {
            if (textUI != null) textUI.text = ""; 
            foreach (char huruf in kalimat.ToCharArray())
            {
                if (textUI != null) textUI.text += huruf;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(bacaDelay);
        }
        if (textUI != null) textUI.text = "";
        if (panelCerita != null) panelCerita.SetActive(false);
        currentStoryRoutine = null;
    }
}