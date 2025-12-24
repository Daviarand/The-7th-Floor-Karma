using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string namaSceneGameplay = "SampleScene";
    public SceneFader sceneFader; // Masukkan script Fader di sini

    bool sedangPindah = false; // Mencegah spam tombol spasi

    void Start()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (sceneFader == null) sceneFader = FindAnyObjectByType<SceneFader>();

        videoPlayer.loopPointReached += VideoSelesai;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)) && !sedangPindah)
        {
            MulaiGameplay();
        }
    }

    void VideoSelesai(VideoPlayer vp)
    {
        if (!sedangPindah) MulaiGameplay();
    }

    void MulaiGameplay()
    {
        sedangPindah = true;
        // Panggil transisi halus
        if (sceneFader != null)
            sceneFader.PindahScene(namaSceneGameplay);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(namaSceneGameplay);
    }
}