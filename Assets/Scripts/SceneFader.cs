using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    // Sesuaikan nama variabel ini dengan yang ada di script lamamu
    // Berdasarkan gambar inspector, sepertinya namanya seperti ini:
    public CanvasGroup fadeCanvasGroup; 
    public Image fadeImage;
    public float kecepatanFade = 1f;
    public float durasiTahanHitam = 1.5f;

    private void Start()
    {
        // --- BAGIAN INI YANG PENTING ---
        // Memastikan saat Scene mulai, layar dipaksa hitam (Alpha 1)
        fadeCanvasGroup.alpha = 1; 
        
        // Langsung jalankan animasi menipis (Fade In)
        StartCoroutine(FadeIn());
        // -------------------------------
    }

    // Fungsi FadeIn (Layar Hitam -> Bening)
    public IEnumerator FadeIn()
    {
        // Opsional: Tunggu sebentar dalam keadaan hitam (biar loading scene terasa smooth)
        yield return new WaitForSeconds(durasiTahanHitam);

        while (fadeCanvasGroup.alpha > 0)
        {
            // Kurangi alpha pelan-pelan
            fadeCanvasGroup.alpha -= Time.deltaTime * kecepatanFade;
            yield return null;
        }
        
        // Pastikan benar-benar 0 di akhir
        fadeCanvasGroup.alpha = 0;
    }

    // Fungsi untuk pindah scene (Bening -> Hitam)
    public void PindahScene(string namaScene)
    {
        StartCoroutine(FadeOut(namaScene));
    }

    public IEnumerator FadeOut(string namaScene)
    {
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += Time.deltaTime * kecepatanFade;
            yield return null;
        }

        SceneManager.LoadScene(namaScene);
    }
}