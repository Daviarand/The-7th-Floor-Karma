// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;
// using UnityEngine.Video;

// public class IntroStoryManager : MonoBehaviour
// {
//     [Header("References")]
//     public VideoPlayer videoPlayer; 
//     public TextMeshProUGUI textDisplay;
//     public CanvasGroup fadeOverlay; 
//     public GameObject dialogPanel;  

//     [Header("Audio Settings")]
//     public AudioSource audioSource;
//     public AudioClip typingSfx; 

//     [Header("Settings")]
//     public string sceneTujuan = "MainScene"; 
//     public float typingSpeed = 0.05f; 
//     public float jedaPerBaris = 2f; // Jeda sebelum teks berganti ke baris baru

//     [TextArea(5, 10)]
//     public string kalimatIntro; // Gunakan tanda '/' untuk memisahkan antar baris

//     void Start()
//     {
//         if (dialogPanel != null) dialogPanel.SetActive(false);
//         if (textDisplay != null) textDisplay.text = "";
//         if (fadeOverlay != null) fadeOverlay.alpha = 0f;

//         if (videoPlayer != null)
//             videoPlayer.loopPointReached += OnVideoFinished;
//     }

//     void OnVideoFinished(VideoPlayer vp)
//     {
//         if (dialogPanel != null) dialogPanel.SetActive(true);
//         StartCoroutine(PlayStorySequence());
//     }

//     IEnumerator PlayStorySequence()
//     {
//         // Memecah teks berdasarkan simbol '/'
//         string[] barisTeks = kalimatIntro.Split('/');

//         foreach (string kalimat in barisTeks)
//         {
//             textDisplay.text = ""; // Kosongkan layar untuk baris baru
            
//             if (audioSource != null && typingSfx != null)
//             {
//                 audioSource.clip = typingSfx;
//                 audioSource.loop = true;
//                 audioSource.Play();
//             }

//             foreach (char letter in kalimat.Trim().ToCharArray())
//             {
//                 textDisplay.text += letter;
//                 yield return new WaitForSeconds(typingSpeed);
//             }

//             if (audioSource != null) audioSource.Stop();

//             // Tunggu sebentar agar pemain bisa baca baris ini
//             yield return new WaitForSeconds(jedaPerBaris);
//         }

//         // FADE OUT ke MainScene setelah semua baris selesai
//         float timer = 0;
//         while (timer < 1.5f)
//         {
//             timer += Time.deltaTime;
//             if (fadeOverlay != null) fadeOverlay.alpha = timer / 1.5f;
//             yield return null;
//         }

//         SceneManager.LoadScene(sceneTujuan);
//     }
// }








using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroStoryManager : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer; 
    public TextMeshProUGUI textDisplay;
    public CanvasGroup fadeOverlay; 
    public GameObject dialogPanel;  

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typingSfx; 

    [Header("Settings")]
    public string sceneTujuan = "MainScene"; 
    public float typingSpeed = 0.05f; 
    public float jedaPerBaris = 2f; 

    [TextArea(5, 10)]
    public string kalimatIntro; 

    // --- TAMBAHAN BARU ---
    private bool isSkipping = false; 

    void Start()
    {
        if (dialogPanel != null) dialogPanel.SetActive(false);
        if (textDisplay != null) textDisplay.text = "";
        if (fadeOverlay != null) fadeOverlay.alpha = 0f;

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    // --- TAMBAHAN BARU: CEK INPUT SETIAP FRAME ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSkipping)
        {
            isSkipping = true;
            StopAllCoroutines(); // Hentikan pengetikan yang sedang berjalan
            SceneManager.LoadScene(sceneTujuan);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (dialogPanel != null) dialogPanel.SetActive(true);
        StartCoroutine(PlayStorySequence());
    }

    IEnumerator PlayStorySequence()
    {
        string[] barisTeks = kalimatIntro.Split('/');

        foreach (string kalimat in barisTeks)
        {
            textDisplay.text = ""; 
            
            if (audioSource != null && typingSfx != null)
            {
                audioSource.clip = typingSfx;
                audioSource.loop = true;
                audioSource.Play();
            }

            foreach (char letter in kalimat.Trim().ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (audioSource != null) audioSource.Stop();

            yield return new WaitForSeconds(jedaPerBaris);
        }

        float timer = 0;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (fadeOverlay != null) fadeOverlay.alpha = timer / 1.5f;
            yield return null;
        }

        // --- TAMBAHAN: CEK APAKAH SUDAH SKIP SEBELUM LOAD ---
        if (!isSkipping) SceneManager.LoadScene(sceneTujuan);
    }
}