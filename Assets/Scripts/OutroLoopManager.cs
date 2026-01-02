using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OutroLoopManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI textDisplay;
    public CanvasGroup fadeOverlay;
    public GameObject dialogPanel;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typingSfx; 

    [Header("Scene Settings")]
    // UBAH DISINI: Defaultnya langsung ke MainScene
    public string sceneTujuan = "MainScene"; 
    public float typingSpeed = 0.05f; 
    public float bacaDelay = 3f;      

    // --- DATA DIALOG ---
    private string[] outroLines = {
        "Kamu pikir kamu sudah menemukan jalan keluar?",
        "Tidak semudah itu...",
        "Ingatlah kembali rasa sakit yang kau berikan pada orang tuamu.",
        "Hukumanmu bukanlah kematian, melainkan pengulangan.",
        "Terjebaklah di sini... selamanya."
    };

    void Awake() 
    {
        if (fadeOverlay != null) 
        {
            fadeOverlay.alpha = 1f; 
            fadeOverlay.blocksRaycasts = true;
        }
        
        if (dialogPanel != null) dialogPanel.SetActive(false);
        if (textDisplay != null) textDisplay.text = "";
    }

    void Start()
    {
        StartCoroutine(PlayOutroSequence());
    }

    IEnumerator PlayOutroSequence()
    {
        // 1. FADE IN
        yield return StartCoroutine(FadeRoutine(1, 0, 2f));

        if (dialogPanel != null) dialogPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // 2. LOOP KALIMAT
        foreach (string kalimat in outroLines)
        {
            textDisplay.text = ""; 

            if (audioSource != null && typingSfx != null)
            {
                audioSource.clip = typingSfx;
                audioSource.loop = true;
                audioSource.Play();
            }

            foreach (char letter in kalimat.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.loop = false;
            }

            yield return new WaitForSeconds(bacaDelay);
        }

        // 3. FADE OUT & PINDAH KE GAMEPLAY (MAINSCENE)
        if (dialogPanel != null) dialogPanel.SetActive(false); 
        textDisplay.text = ""; 
        
        yield return StartCoroutine(FadeRoutine(0, 1, 2.5f));

        // Kita tetap simpan data loop (siapa tau nanti mau bikin musuh makin susah)
        int currentLoop = PlayerPrefs.GetInt("LoopCount", 0);
        PlayerPrefs.SetInt("LoopCount", currentLoop + 1);
        PlayerPrefs.Save(); 
        
        // LANGSUNG KE MAIN SCENE
        SceneManager.LoadScene(sceneTujuan);
    }

    IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (fadeOverlay != null)
                fadeOverlay.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }
        if (fadeOverlay != null) fadeOverlay.alpha = endAlpha;
    }
}