using UnityEngine;
// Hapus using SceneManager, kita pakai lewat Fader

public class MainMenuManager : MonoBehaviour
{
    public string namaSceneCutscene = "IntroCutscene"; 
    public SceneFader sceneFader; // Masukkan script Fader di sini

    void Start()
    {
        // Cari otomatis jika lupa drag
        if (sceneFader == null) 
            sceneFader = FindAnyObjectByType<SceneFader>();
    }

    public void TekanPlay()
    {
        // GANTI INI: Dari SceneManager.LoadScene jadi...
        if (sceneFader != null)
            sceneFader.PindahScene(namaSceneCutscene);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(namaSceneCutscene); // Cadangan
    }

    public void TekanQuit()
    {
        Application.Quit();
    }
}