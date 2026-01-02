// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;

//     [Header("UI Story")]
//     public TMP_Text textUI;
//     public GameObject panelCerita;

//     // --- TAMBAHAN BARU: UI KARMA ---
//     [Header("UI Gameplay")]
//     public TMP_Text textKarma; 

//     [Header("UI Jumpscare")]
//     public GameObject jumpscarePanel;

//     [Header("Data Game")]
//     public int koinTerkumpul = 0;
//     public int targetTrigger = 99;
//     public int koinTengahJalan = 50;
//     [Tooltip("Koordinat dimana Kunci akan muncul")]
//     public Vector3 posisiMunculKunci = new Vector3(-6, 1, 0);

//     [Header("Referenced Objects")]
//     public GameObject prefabKunci;
//     public PhoneController phoneScript;
//     public EnemyAI enemyScript;
//     public GameObject pintuExitObject;

//     [Header("Audio System")]
//     public AudioSource sfxSource;
//     public AudioSource bgmSource;

//     public AudioClip sfxKoin;
//     public AudioClip bgmNormal;
//     public AudioClip sfxStory;
//     public AudioClip jumpscareSound;

//     [Header("Pengaturan Volume")]
//     [Range(0f, 5f)]
//     public float volumeJumpscare = 1.0f;

//     // Settingan Teks
//     private float typingSpeed = 0.05f;
//     private float bacaDelay = 3f;

//     private bool isClimax = false;
//     private bool isGameOver = false;
//     private Coroutine currentStoryRoutine;

//     // ==========================================
//     // BAGIAN BARU: VARIASI TEKS INTRO MAINSCENE
//     // ==========================================
//     [Header("Variasi Teks Intro")]
//     [TextArea(2,5)]
//     public string[] introLoop0 = {
//         "Kepalaku sakit sekali... Di mana aku sekarang?",
//         "Lantai 7... Konon tempat ini terkutuk. Aku harus segera mencari jalan keluar.",
//         "Sinyal di sini buruk. Untung peta di HP masih berfungsi."
//     };

//     [TextArea(2,5)]
//     public string[] introLoop1 = {
//         "Hah?! K-Kenapa aku kembali ke pintu depan?!",
//         "Pintunya terkunci lagi... Tidak mungkin!",
//         "Aku baru saja keluar kan? Kenapa aku ditarik kembali ke sini?!"
//     };

//     [TextArea(2,5)]
//     public string[] introLoop2 = {
//         "TIDAK! JANGAN DI SINI LAGI! KUMOHON!",
//         "Aku minta maaf Bu... Aku minta maaf Yah...",
//         "Tolong biarkan aku pergi... Aku tidak mau terjebak selamanya..."
//     };
//     // ==========================================

//     void Awake() { if (instance == null) instance = this; }

//     void Start()
//     {
//         if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
//         if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

//         if (bgmSource != null && bgmNormal != null)
//         {
//             bgmSource.clip = bgmNormal;
//             bgmSource.loop = true;
//             bgmSource.Play();
//         }

//         if (pintuExitObject != null) pintuExitObject.SetActive(false);
//         if (panelCerita != null) panelCerita.SetActive(false);
//         if (jumpscarePanel != null) jumpscarePanel.SetActive(false);

//         UpdateKarmaUI();
//         Time.timeScale = 1;

//         StartCoroutine(StartIntroWithDelay());
//     }

//     IEnumerator StartIntroWithDelay()
//     {
//         yield return new WaitForSeconds(3f);

//         // --- LOGIKA BARU: PILIH TEKS BERDASARKAN LOOP ---
//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         string[] selectedIntro;

//         if (loopCount == 0)
//         {
//             selectedIntro = introLoop0; // Teks Normal
//         }
//         else if (loopCount == 1)
//         {
//             selectedIntro = introLoop1; // Teks Bingung (Loop 1)
//         }
//         else
//         {
//             selectedIntro = introLoop2; // Teks Putus Asa (Loop 2++)
//         }

//         PlaySequence(selectedIntro);
//         // ------------------------------------------------
//     }

//     public void TambahKoin()
//     {
//         koinTerkumpul++;
//         UpdateKarmaUI();

//         if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

//         if (koinTerkumpul == 1)
//         {
//             // Tips: Kamu juga bisa bikin logika LoopCount disini kalau mau teks koinnya berubah juga
//             string[] koinPertamaTeks = {
//                 "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
//                 "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
//             };
//             PlaySequence(koinPertamaTeks);
//         }
//         else if (koinTerkumpul == koinTengahJalan)
//         {
//             PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
//         }
//         else if (koinTerkumpul == targetTrigger && !isClimax)
//         {
//             TriggerClimaxMode();
//         }
//         else if (koinTerkumpul >= 100)
//         {
//             PlaySequence(new string[] { "KUNCI DITEMUKAN! Pintu Keluar Terbuka! AKU HARUS LARI!" });
//         }
//     }

//     void UpdateKarmaUI()
//     {
//         if (textKarma != null)
//         {
//             textKarma.text = "KOIN: " + koinTerkumpul;
//         }
//     }

//     void TriggerClimaxMode()
//     {
//         isClimax = true;
//         if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();

//         if (prefabKunci != null) Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
//         if (pintuExitObject != null) pintuExitObject.SetActive(true);

//         string[] climaxTeks = {
//             "Sial! HP-ku mati! Sinyalnya hilang total!",
//             "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
//         };
//         PlaySequence(climaxTeks);
//     }

//     public void TriggerJumpscare()
//     {
//         if (isGameOver) return;
//         isGameOver = true;

//         if (jumpscarePanel != null) jumpscarePanel.SetActive(true);

//         if (sfxSource != null && jumpscareSound != null)
//             sfxSource.PlayOneShot(jumpscareSound, volumeJumpscare);

//         if (bgmSource != null) bgmSource.Stop();

//         Time.timeScale = 0f;
//     }

//     public void RestartGame()
//     {
//         Time.timeScale = 1f;
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

//             if (sfxSource != null && sfxStory != null)
//             {
//                 sfxSource.clip = sfxStory;
//                 sfxSource.loop = true;
//                 sfxSource.Play();
//             }

//             foreach (char huruf in kalimat.ToCharArray())
//             {
//                 if (textUI != null) textUI.text += huruf;
//                 yield return new WaitForSeconds(typingSpeed);
//             }

//             if (sfxSource != null)
//             {
//                 sfxSource.Stop();
//                 sfxSource.loop = false;
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
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Story")]
    public TMP_Text textUI;
    public GameObject panelCerita;

    [Header("UI Gameplay")]
    public TMP_Text textKarma; 

    [Header("UI Jumpscare")]
    public GameObject jumpscarePanel;

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
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    public AudioClip sfxKoin;
    public AudioClip bgmNormal;
    public AudioClip sfxStory;
    public AudioClip jumpscareSound;

    [Header("Pengaturan Volume")]
    [Range(0f, 5f)]
    public float volumeJumpscare = 1.0f;

    // Settingan Teks
    private float typingSpeed = 0.05f;
    private float bacaDelay = 3f;

    private bool isClimax = false;
    private bool isGameOver = false;
    private Coroutine currentStoryRoutine;

    // ==========================================
    // VARIASI TEKS INTRO MAINSCENE
    // ==========================================
    [Header("Variasi Teks Intro")]
    [TextArea(2,5)]
    public string[] introLoop0 = {
        "Kepalaku sakit sekali... Di mana aku sekarang?",
        "Lantai 7... Konon tempat ini terkutuk. Aku harus segera mencari jalan keluar.",
        "Sinyal di sini buruk. Untung peta di HP masih berfungsi."
    };

    [TextArea(2,5)]
    public string[] introLoop1 = {
        "Hah?! K-Kenapa aku kembali ke pintu depan?!",
        "Pintunya terkunci lagi... Tidak mungkin!",
        "Aku baru saja keluar kan? Kenapa aku ditarik kembali ke sini?!"
    };

    [TextArea(2,5)]
    public string[] introLoop2 = {
        "TIDAK! JANGAN DI SINI LAGI! KUMOHON!",
        "Aku minta maaf Bu... Aku minta maaf Yah...",
        "Tolong biarkan aku pergi... Aku tidak mau terjebak selamanya..."
    };

    void Awake() { if (instance == null) instance = this; }

    void Start()
    {
        if (enemyScript == null) enemyScript = FindAnyObjectByType<EnemyAI>();
        if (phoneScript == null) phoneScript = FindAnyObjectByType<PhoneController>();

        if (bgmSource != null && bgmNormal != null)
        {
            bgmSource.clip = bgmNormal;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        if (pintuExitObject != null) pintuExitObject.SetActive(false);
        if (panelCerita != null) panelCerita.SetActive(false);
        if (jumpscarePanel != null) jumpscarePanel.SetActive(false);

        UpdateKarmaUI();
        Time.timeScale = 1;

        StartCoroutine(StartIntroWithDelay());
    }

    IEnumerator StartIntroWithDelay()
    {
        yield return new WaitForSeconds(3f);

        // --- LOGIKA PILIH TEKS BERDASARKAN LOOP ---
        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
        string[] selectedIntro;

        if (loopCount == 0) selectedIntro = introLoop0; 
        else if (loopCount == 1) selectedIntro = introLoop1; 
        else selectedIntro = introLoop2; 

        PlaySequence(selectedIntro);
    }

    // ==========================================================
    // BAGIAN YANG DIUBAH: Pengecekan Loop sebelum Muncul Teks
    // ==========================================================
    public void TambahKoin()
    {
        koinTerkumpul++;
        UpdateKarmaUI();

        if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

        // Kita cek dulu ini loop keberapa
        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);

        if (koinTerkumpul == 1)
        {
            // HANYA MUNCUL JIKA PEMAIN BARU (Loop 0)
            if (loopCount == 0)
            {
                string[] koinPertamaTeks = {
                    "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
                    "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
                };
                PlaySequence(koinPertamaTeks);
            }
        }
        else if (koinTerkumpul == koinTengahJalan)
        {
            // HANYA MUNCUL JIKA PEMAIN BARU (Loop 0)
            if (loopCount == 0)
            {
                PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
            }
        }
        else if (koinTerkumpul == targetTrigger && !isClimax)
        {
            TriggerClimaxMode();
        }
        else if (koinTerkumpul >= 100)
        {
            // Teks ending tetap dimunculkan (atau mau dihilangkan juga boleh)
            // Disini saya biarkan muncul sebagai tanda sudah selesai
            PlaySequence(new string[] { "KUNCI DITEMUKAN! Pintu Keluar Terbuka! AKU HARUS LARI!" });
        }
    }

    void TriggerClimaxMode()
    {
        isClimax = true;
        
        // Logika Gameplay (Tetap Jalan meski Loop 2)
        if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
        if (prefabKunci != null) Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
        if (enemyScript != null) enemyScript.ActivateWeepingMode();
        if (pintuExitObject != null) pintuExitObject.SetActive(true);

        // Logika Teks (Hanya muncul di Loop 0)
        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
        
        if (loopCount == 0)
        {
            string[] climaxTeks = {
                "Sial! HP-ku mati! Sinyalnya hilang total!",
                "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
            };
            PlaySequence(climaxTeks);
        }
        // Jika Loop 2 dst, tidak ada teks, langsung dikejar hantu (lebih tegang)
    }
    // ==========================================================

    void UpdateKarmaUI()
    {
        if (textKarma != null)
        {
            textKarma.text = "KOIN: " + koinTerkumpul;
        }
    }

    public void TriggerJumpscare()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (jumpscarePanel != null) jumpscarePanel.SetActive(true);

        if (sfxSource != null && jumpscareSound != null)
            sfxSource.PlayOneShot(jumpscareSound, volumeJumpscare);

        if (bgmSource != null) bgmSource.Stop();

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            if (sfxSource != null && sfxStory != null)
            {
                sfxSource.clip = sfxStory;
                sfxSource.loop = true;
                sfxSource.Play();
            }

            foreach (char huruf in kalimat.ToCharArray())
            {
                if (textUI != null) textUI.text += huruf;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (sfxSource != null)
            {
                sfxSource.Stop();
                sfxSource.loop = false;
            }

            yield return new WaitForSeconds(bacaDelay);
        }

        if (textUI != null) textUI.text = "";
        if (panelCerita != null) panelCerita.SetActive(false);
        currentStoryRoutine = null;
    }
}