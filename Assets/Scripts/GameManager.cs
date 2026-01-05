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

//     [Header("UI Gameplay")]
//     public TMP_Text textKarma; 
//     public TMP_Text textHealth; 

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

//     // --- TAMBAHAN AUDIO TRANSISI ---
//     public AudioClip sfxElevatorDown; 
//     public AudioClip sfxElevatorBell; 

//     [Header("Pengaturan Volume")]
//     [Range(0f, 5f)]
//     public float volumeJumpscare = 1.0f;

//     [Header("Health Settings")]
//     public int playerHealth = 3; 

//     private float typingSpeed = 0.05f;
//     private float bacaDelay = 3f;

//     private bool isClimax = false;
//     private bool isGameOver = false;
//     private Coroutine currentStoryRoutine;

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
//         UpdateHealthUI(); 
//         Time.timeScale = 1;

//         // --- MERUBAH PEMANGGILAN AWAL KE SEQUENCE ELEVATOR ---
//         StartCoroutine(SequenceElevatorMasuk());
//     }

//     // --- FUNGSI BARU UNTUK TRANSISI SUARA ELEVATOR ---
//     IEnumerator SequenceElevatorMasuk()
//     {
//         // 1. Suara elevator turun
//         if (sfxSource != null && sfxElevatorDown != null)
//         {
//             sfxSource.PlayOneShot(sfxElevatorDown);
//             yield return new WaitForSeconds(4f); // Tunggu elevator turun (sesuaikan durasi audio)
//         }

//         // 2. Suara Bell Ting!
//         if (sfxSource != null && sfxElevatorBell != null)
//         {
//             sfxSource.PlayOneShot(sfxElevatorBell);
//             yield return new WaitForSeconds(1.5f); // Jeda singkat setelah bel
//         }

//         // 3. Lanjut ke Intro Cerita (Fungsi Asli Anda)
//         StartCoroutine(StartIntroWithDelay());
//     }

//     IEnumerator StartIntroWithDelay()
//     {
//         // Delay 3 detik bawaan kodingan asli Anda
//         yield return new WaitForSeconds(3f);
//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         string[] selectedIntro;

//         if (loopCount == 0) selectedIntro = introLoop0; 
//         else if (loopCount == 1) selectedIntro = introLoop1; 
//         else selectedIntro = introLoop2; 

//         PlaySequence(selectedIntro);
//     }

//     public void KurangiNyawa()
//     {
//         if (isGameOver) return;
//         playerHealth--;
//         UpdateHealthUI(); 

//         if (playerHealth <= 0)
//         {
//             TriggerJumpscare();
//         }
//     }

//     void UpdateHealthUI()
//     {
//         if (textHealth != null)
//         {
//             textHealth.text = "NYAWA: " + playerHealth;
//         }
//     }

//     public void TambahKoin()
//     {
//         koinTerkumpul++;
//         UpdateKarmaUI();
//         if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         if (koinTerkumpul == 1)
//         {
//             if (loopCount == 0)
//             {
//                 string[] koinPertamaTeks = {
//                     "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
//                     "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
//                 };
//                 PlaySequence(koinPertamaTeks);
//             }
//         }
//         else if (koinTerkumpul == koinTengahJalan)
//         {
//             if (loopCount == 0) PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
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

//     void TriggerClimaxMode()
//     {
//         isClimax = true;
//         if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
//         if (prefabKunci != null) Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
//         if (pintuExitObject != null) pintuExitObject.SetActive(true);

//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         if (loopCount == 0)
//         {
//             string[] climaxTeks = {
//                 "Sial! HP-ku mati! Sinyalnya hilang total!",
//                 "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
//             };
//             PlaySequence(climaxTeks);
//         }
//     }

//     void UpdateKarmaUI()
//     {
//         if (textKarma != null) textKarma.text = "KOIN: " + koinTerkumpul;
//     }

//     public void TriggerJumpscare()
//     {
//         if (isGameOver) return;
//         isGameOver = true;
//         if (jumpscarePanel != null) jumpscarePanel.SetActive(true);
//         if (sfxSource != null && jumpscareSound != null)
//             sfxSource.PlayOneShot(jumpscareSound, volumeJumpscare);
//         if (bgmSource != null) bgmSource.Stop();
        
//         StartCoroutine(AutoResetGame(5f));
//     }

//     IEnumerator AutoResetGame(float delay)
//     {
//         yield return new WaitForSecondsRealtime(delay);
//         RestartGame();
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
//             if (sfxSource != null) { sfxSource.Stop(); sfxSource.loop = false; }
//             yield return new WaitForSeconds(bacaDelay);
//         }
//         if (textUI != null) textUI.text = "";
//         if (panelCerita != null) panelCerita.SetActive(false);
//         currentStoryRoutine = null;
//     }
// }











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

//     [Header("UI Gameplay")]
//     public TMP_Text textKarma; 
//     public TMP_Text textHealth; 

//     [Header("UI Jumpscare")]
//     public GameObject jumpscarePanel;

//     // --- TAMBAHAN UNTUK KONTROL LAYAR HITAM ---
//     [Header("Transition Settings")]
//     public CanvasGroup canvasTransition; // Tarik Panel Hitam (CanvasGroup) ke sini

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

//     public AudioClip sfxElevatorDown; 
//     public AudioClip sfxElevatorBell; 

//     [Header("Pengaturan Volume")]
//     [Range(0f, 5f)]
//     public float volumeJumpscare = 1.0f;

//     [Header("Health Settings")]
//     public int playerHealth = 3; 

//     private float typingSpeed = 0.05f;
//     private float bacaDelay = 3f;

//     private bool isClimax = false;
//     private bool isGameOver = false;
//     private Coroutine currentStoryRoutine;

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

//         // --- TAMBAHAN: PAKSA LAYAR TETAP HITAM PEKAT DI DETIK PERTAMA ---
//         if (canvasTransition != null) 
//         {
//             canvasTransition.alpha = 1f;
//             canvasTransition.gameObject.SetActive(true);
//         }

//         UpdateKarmaUI();
//         UpdateHealthUI(); 
//         Time.timeScale = 1;

//         StartCoroutine(SequenceElevatorMasuk());
//     }

//     IEnumerator SequenceElevatorMasuk()
// {
//     // Memastikan layar tetap HITAM PEKAT saat mulai
//     if (canvasTransition != null) canvasTransition.alpha = 1f;

//     // 1. Suara elevator turun (Layar Masih Hitam)
//     if (sfxSource != null && sfxElevatorDown != null)
//     {
//         sfxSource.PlayOneShot(sfxElevatorDown);
//         yield return new WaitForSeconds(4f); // Sesuaikan dengan durasi suara turun
//     }

//     // 2. Suara Bell Ting! (Layar MASIH HITAM di sini)
//     if (sfxSource != null && sfxElevatorBell != null)
//     {
//         sfxSource.PlayOneShot(sfxElevatorBell);
        
//         // --- KUNCINYA DI SINI ---
//         // Tunggu sampai suara bel selesai sebelum lanjut ke kode memudar.
//         // Jika suara bel Anda durasinya 2 detik, masukkan 2f.
//         yield return new WaitForSeconds(2.5f); 
//     }

//     // 3. SEKARANG BARU BUKA LAYAR HITAM (Setelah bel selesai)
//     if (canvasTransition != null)
//     {
//         float duration = 2f; // Durasi memudar
//         float currentTime = 0f;
//         while (currentTime < duration)
//         {
//             currentTime += Time.deltaTime;
//             canvasTransition.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
//             yield return null;
//         }
//         canvasTransition.alpha = 0f;
//         canvasTransition.blocksRaycasts = false;
//     }

//     // 4. Lanjut ke Intro Cerita
//     StartCoroutine(StartIntroWithDelay());
// }

//     IEnumerator StartIntroWithDelay()
//     {
//         yield return new WaitForSeconds(1f); // Jeda singkat setelah layar terbuka
//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         string[] selectedIntro;

//         if (loopCount == 0) selectedIntro = introLoop0; 
//         else if (loopCount == 1) selectedIntro = introLoop1; 
//         else selectedIntro = introLoop2; 

//         PlaySequence(selectedIntro);
//     }

//     public void KurangiNyawa()
//     {
//         if (isGameOver) return;
//         playerHealth--;
//         UpdateHealthUI(); 

//         if (playerHealth <= 0)
//         {
//             TriggerJumpscare();
//         }
//     }

//     void UpdateHealthUI()
//     {
//         if (textHealth != null)
//         {
//             textHealth.text = "NYAWA: " + playerHealth;
//         }
//     }

//     public void TambahKoin()
//     {
//         koinTerkumpul++;
//         UpdateKarmaUI();
//         if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         if (koinTerkumpul == 1)
//         {
//             if (loopCount == 0)
//             {
//                 string[] koinPertamaTeks = {
//                     "Koin emas? Kenapa ada banyak koin berserakan di tempat seram ini?",
//                     "Mungkin jika aku mengumpulkannya, sesuatu akan terjadi..."
//                 };
//                 PlaySequence(koinPertamaTeks);
//             }
//         }
//         else if (koinTerkumpul == koinTengahJalan)
//         {
//             if (loopCount == 0) PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
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

//     void TriggerClimaxMode()
//     {
//         isClimax = true;
//         if (phoneScript != null) phoneScript.ForceClosePhoneAndDisable();
//         if (prefabKunci != null) Instantiate(prefabKunci, posisiMunculKunci, Quaternion.identity);
//         if (enemyScript != null) enemyScript.ActivateWeepingMode();
//         if (pintuExitObject != null) pintuExitObject.SetActive(true);

//         int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
//         if (loopCount == 0)
//         {
//             string[] climaxTeks = {
//                 "Sial! HP-ku mati! Sinyalnya hilang total!",
//                 "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
//             };
//             PlaySequence(climaxTeks);
//         }
//     }

//     void UpdateKarmaUI()
//     {
//         if (textKarma != null) textKarma.text = "KOIN: " + koinTerkumpul;
//     }

//     public void TriggerJumpscare()
//     {
//         if (isGameOver) return;
//         isGameOver = true;
//         if (jumpscarePanel != null) jumpscarePanel.SetActive(true);
//         if (sfxSource != null && jumpscareSound != null)
//             sfxSource.PlayOneShot(jumpscareSound, volumeJumpscare);
//         if (bgmSource != null) bgmSource.Stop();
        
//         StartCoroutine(AutoResetGame(5f));
//     }

//     IEnumerator AutoResetGame(float delay)
//     {
//         yield return new WaitForSecondsRealtime(delay);
//         RestartGame();
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
//             if (sfxSource != null) { sfxSource.Stop(); sfxSource.loop = false; }
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
    public TMP_Text textHealth; 

    // --- TAMBAHAN UNTUK HUD KONTROL ---
    public GameObject controlsHUD; // Tarik objek Teks Instruksi Anda ke sini

    [Header("UI Jumpscare")]
    public GameObject jumpscarePanel;

    [Header("Transition Settings")]
    public CanvasGroup canvasTransition; 

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

    public AudioClip sfxElevatorDown; 
    public AudioClip sfxElevatorBell; 

    [Header("Pengaturan Volume")]
    [Range(0f, 5f)]
    public float volumeJumpscare = 1.0f;

    [Header("Health Settings")]
    public int playerHealth = 3; 

    private float typingSpeed = 0.05f;
    private float bacaDelay = 3f;

    private bool isClimax = false;
    private bool isGameOver = false;
    private Coroutine currentStoryRoutine;

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

        // Pastikan HUD Kontrol menyala di awal
        if (controlsHUD != null) controlsHUD.SetActive(true);

        if (canvasTransition != null) canvasTransition.alpha = 1f;

        UpdateKarmaUI();
        UpdateHealthUI(); 
        Time.timeScale = 1;

        StartCoroutine(SequenceElevatorMasuk());
    }

    IEnumerator SequenceElevatorMasuk()
    {
        if (canvasTransition != null) canvasTransition.alpha = 1f;

        if (sfxSource != null && sfxElevatorDown != null)
        {
            sfxSource.PlayOneShot(sfxElevatorDown);
            yield return new WaitForSeconds(4f); 
        }

        if (sfxSource != null && sfxElevatorBell != null)
        {
            sfxSource.PlayOneShot(sfxElevatorBell);
            yield return new WaitForSeconds(1.5f); 
        }

        if (canvasTransition != null)
        {
            float duration = 2f; 
            float currentTime = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                canvasTransition.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
                yield return null;
            }
            canvasTransition.alpha = 0f;
            canvasTransition.blocksRaycasts = false;
        }

        StartCoroutine(HideControlsAfterDelay(7f));

        StartCoroutine(StartIntroWithDelay());
    }

    // Fungsi Coroutine baru untuk menyembunyikan HUD
    IEnumerator HideControlsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (controlsHUD != null) controlsHUD.SetActive(false);
    }

    IEnumerator StartIntroWithDelay()
    {
        yield return new WaitForSeconds(1f);
        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
        string[] selectedIntro;

        if (loopCount == 0) selectedIntro = introLoop0; 
        else if (loopCount == 1) selectedIntro = introLoop1; 
        else selectedIntro = introLoop2; 

        PlaySequence(selectedIntro);
    }

    public void KurangiNyawa()
    {
        if (isGameOver) return;
        playerHealth--;
        UpdateHealthUI(); 

        if (playerHealth <= 0)
        {
            TriggerJumpscare();
        }
    }

    void UpdateHealthUI()
    {
        if (textHealth != null)
        {
            textHealth.text = "NYAWA: " + playerHealth;
        }
    }

    public void TambahKoin()
    {
        koinTerkumpul++;
        UpdateKarmaUI();
        if (sfxSource != null && sfxKoin != null) sfxSource.PlayOneShot(sfxKoin);

        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
        if (koinTerkumpul == 1)
        {
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
            if (loopCount == 0) PlaySequence(new string[] { "Jangan lengah. Aku harus terus bergerak." });
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

        int loopCount = PlayerPrefs.GetInt("LoopCount", 0);
        if (loopCount == 0)
        {
            string[] climaxTeks = {
                "Sial! HP-ku mati! Sinyalnya hilang total!",
                "Aku harus mencari kunci dan lari ke pintu keluar SEKARANG!"
            };
            PlaySequence(climaxTeks);
        }
    }

    void UpdateKarmaUI()
    {
        if (textKarma != null) textKarma.text = "KOIN: " + koinTerkumpul;
    }

    public void TriggerJumpscare()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (jumpscarePanel != null) jumpscarePanel.SetActive(true);
        if (sfxSource != null && jumpscareSound != null)
            sfxSource.PlayOneShot(jumpscareSound, volumeJumpscare);
        if (bgmSource != null) bgmSource.Stop();
        
        StartCoroutine(AutoResetGame(5f));
    }

    IEnumerator AutoResetGame(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        RestartGame();
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
            if (sfxSource != null) { sfxSource.Stop(); sfxSource.loop = false; }
            yield return new WaitForSeconds(bacaDelay);
        }
        if (textUI != null) textUI.text = "";
        if (panelCerita != null) panelCerita.SetActive(false);
        currentStoryRoutine = null;
    }
}