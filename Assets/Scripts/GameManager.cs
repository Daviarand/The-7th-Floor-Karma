// using UnityEngine;
// using TMPro;
// using System.Collections; 

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
    
//     [Header("UI Story")]
//     public TMP_Text textUI;       
//     public GameObject panelCerita; 

//     [Header("Data Game")]
//     public int koinTerkumpul = 0;
//     public int targetTrigger = 99; // Pastikan ini 99
//     public int koinTengahJalan = 50; 
    
//     [Tooltip("Koordinat dimana Kunci akan muncul")]
//     public Vector3 posisiMunculKunci = new Vector3(-6, 1, 0); 

//     [Header("Referenced Objects")]
//     public GameObject prefabKunci; // Drag PREFAB KOIN dari folder Project kesini
//     public PhoneController phoneScript; 
//     public EnemyAI enemyScript; 
//     public GameObject pintuExitObject; 
    
//     // Kecepatan Mengetik 
//     private float typingSpeed = 0.03f; 
//     private float bacaDelay = 3f; 

//     private bool isClimax = false;
//     private Coroutine currentStoryRoutine;

//     void Awake() { if (instance == null) instance = this; }

//     void Start() 
//     { 
//         if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
//         if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

//         // Kita tidak perlu menyembunyikan prefabKunci karena dia masih berupa file prefab
//         if (pintuExitObject != null) pintuExitObject.SetActive(false);
//         if (panelCerita != null) panelCerita.SetActive(false); 
        
//         Time.timeScale = 1; 

//         // --- INTRO ---
//         string[] introTeks = {
//             "Kepalaku sakit sekali... Di mana aku sekarang?",
//             "Lantai 7... Konon tempat ini terkutuk. Aku harus segera mencari jalan keluar.",
//             "Sinyal di sini buruk. Untung peta di HP masih berfungsi (Tekan M)."
//         };
//         PlaySequence(introTeks);
//     }

//     public void TambahKoin()
//     {
//         koinTerkumpul++;
        
//         // --- Cek Koin Pertama ---
//         if (koinTerkumpul == 1)
//         {
//             string[] koinPertamaTeks = {
//                 "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
//                 "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
//             };
//             PlaySequence(koinPertamaTeks);
//         }
        
//         // --- Cek Pertengahan ---
//         else if (koinTerkumpul == koinTengahJalan)
//         {
//             PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
//         }

//         // --- Cek KLIMAKS (99 Koin) ---
//         else if (koinTerkumpul == targetTrigger && !isClimax)
//         {
//             TriggerClimaxMode();
//         }
        
//         // --- Cek MENANG (100 Koin / Sudah ambil kunci) ---
//         else if (koinTerkumpul >= 100) 
//         {
//             PlaySequence(new string[] { "KUNCI SUDAH KETEMU! Pintu Keluar Bisa Terbuka! AKU HARUS LARI!" });
//         }
//     }

//     void TriggerClimaxMode()
//     {
//         isClimax = true;

//         if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        
//         // --- PERBAIKAN DI SINI: INSTANTIATE ---
//         if (prefabKunci != null) 
//         {
//             // Kita "Lahirkan" kuncinya di koordinat yang sudah ditentukan
//             Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
//             Debug.Log("Kunci Muncul di: " + posisiMunculKunci);
//         }
        
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
//         if (pintuExitObject != null) pintuExitObject.SetActive(true);

//         // Teks Klimaks
//         string[] climaxTeks = {
//             "Sial! HP-ku mati! Sinyalnya hilang total!",
//             "Suara apa itu? Sesuatu yang buruk sedang mendekat!",
//             "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
//         };
//         PlaySequence(climaxTeks);
//     }

//     public void PlaySequence(string[] daftarKalimat)
//     {
//         if (currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
//         currentStoryRoutine = StartCoroutine(SequenceProcess(daftarKalimat));
//     }

//     IEnumerator SequenceProcess(string[] lines)
//     {
//         if (panelCerita != null) panelCerita.SetActive(true);
//         foreach (string kalimat in lines)
//         {
//             if (textUI != null) textUI.text = ""; 
//             foreach (char huruf in kalimat.ToCharArray())
//             {
//                 if (textUI != null) textUI.text += huruf;
//                 yield return new WaitForSeconds(typingSpeed);
//             }
//             yield return new WaitForSeconds(bacaDelay);
//         }
//         if (textUI != null) textUI.text = "";
//         if (panelCerita != null) panelCerita.SetActive(false);
//         currentStoryRoutine = null;
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
    public int targetTrigger = 99; 
    public int koinTengahJalan = 50; 
    [Tooltip("Koordinat dimana Kunci akan muncul")]
    public Vector3 posisiMunculKunci = new Vector3(-6, 1, 0); 

    [Header("Referenced Objects")]
    public GameObject prefabKunci; 
    public PhoneController phoneScript; 
    public EnemyAI enemyScript; 
    public GameObject pintuExitObject; 
    
    [Header("Audio System")]
    public AudioSource sfxSource;    // Audio Source untuk SFX
    public AudioSource bgmSource;    // Audio Source untuk Lagu
    
    public AudioClip sfxKoin;        // Suara Koin
    public AudioClip bgmNormal;      // Lagu Utama
    public AudioClip sfxStory;       // Suara Keyboard/Kertas

    // Settingan Teks
    private float typingSpeed = 0.05f; // Sedikit diperlambat biar pas sama suara keyboard
    private float bacaDelay = 3f; 

    private bool isClimax = false;
    private Coroutine currentStoryRoutine;

    void Awake() { if (instance == null) instance = this; }

    void Start() 
    { 
        if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
        if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();
        
        // Mainkan BGM
        if (bgmSource != null && bgmNormal != null)
        {
            bgmSource.clip = bgmNormal;
            bgmSource.loop = true; 
            bgmSource.Play();
        }

        if (pintuExitObject != null) pintuExitObject.SetActive(false);
        if (panelCerita != null) panelCerita.SetActive(false); 
        
        Time.timeScale = 1; 

        // --- GANTI LOGIKA INTRO ---
        // Jangan langsung PlaySequence, tapi panggil Coroutine Jeda dulu
        StartCoroutine(StartIntroWithDelay());
    }

    // Fungsi Baru: Memberi jeda sebelum teks pertama muncul
    IEnumerator StartIntroWithDelay()
    {
        // Tunggu 3 detik (biar player merasakan atmosfer gelap dulu)
        yield return new WaitForSeconds(3f);

        string[] introTeks = {
            "Kepalaku sakit sekali... Di mana aku sekarang?",
            "Lantai 7... Konon tempat ini terkutuk. Aku harus segera mencari jalan keluar.",
            "Sinyal di sini buruk. Untung peta di HP masih berfungsi."
        };
        PlaySequence(introTeks);
    }

    public void TambahKoin()
    {
        koinTerkumpul++;
        
        if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

        if (koinTerkumpul == 1)
        {
            string[] koinPertamaTeks = {
                "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
                "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
            };
            PlaySequence(koinPertamaTeks);
        }
        else if (koinTerkumpul == koinTengahJalan)
        {
            PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
        }
        else if (koinTerkumpul == targetTrigger && !isClimax)
        {
            TriggerClimaxMode();
        }
        else if (koinTerkumpul >= 100) 
        {
            PlaySequence(new string[] { "KUNCI DITEMUKAN! Pintu Keluar Terbuka! AKU HARUS LARI!" });
        }
    }

    void TriggerClimaxMode()
    {
        isClimax = true;
        if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        
        if (prefabKunci != null) Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
        if (enemyScript != null) enemyScript.ActivateWeepingMode();
        if (pintuExitObject != null) pintuExitObject.SetActive(true);

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

            // --- 1. NYALAKAN SUARA (LOOPING) ---
            if (sfxSource != null && sfxStory != null)
            {
                sfxSource.clip = sfxStory;
                sfxSource.loop = true; // Agar bunyi "tik-tik-tik" terus nyala
                sfxSource.Play();
            }

            // --- 2. KETIK HURUF ---
            foreach (char huruf in kalimat.ToCharArray())
            {
                if (textUI != null) textUI.text += huruf;
                yield return new WaitForSeconds(typingSpeed);
            }

            // --- 3. MATIKAN SUARA (STOP) ---
            // Teks sudah selesai diketik, matikan suara keyboardnya
            if (sfxSource != null)
            {
                sfxSource.Stop();
                sfxSource.loop = false; // Matikan loop agar aman untuk sfx lain
            }

            // --- 4. JEDA BACA ---
            yield return new WaitForSeconds(bacaDelay);
        }

        if (textUI != null) textUI.text = "";
        if (panelCerita != null) panelCerita.SetActive(false);
        currentStoryRoutine = null;
    }
}