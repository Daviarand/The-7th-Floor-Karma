// using UnityEngine;
// using UnityEngine.SceneManagement;

// #if UNITY_EDITOR
// using UnityEditor; // Tambahan library khusus Editor
// #endif

// public class MainMenuManager : MonoBehaviour
// {
//     public void MainkanGame()
//     {
//         SceneManager.LoadScene("IntroCutscene");
//     }

//     public void KeluarGame()
//     {
//         Debug.Log("Game Ditutup!");

//         // Logika: Jika di Editor, stop playing. Jika di Game asli, tutup aplikasi.
//         #if UNITY_EDITOR
//             EditorApplication.isPlaying = false;
//         #else
//             Application.Quit();
//         #endif
//     }

//     public void BukaSettings()
//     {
//         Debug.Log("Fitur Settings belum aktif.");
//     }
// }




using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Wajib ada untuk fitur 'Tunggu Sebentar'

public class MainMenuManager : MonoBehaviour
{
    [Header("Pengaturan Suara")]
    public AudioSource sumberSuara; // Tempat memutar suara
    public AudioClip suaraKlik;     // File suara tombolnya

    // Fungsi tombol PLAY (Sekarang memanggil Coroutine)
    public void MainkanGame()
    {
        StartCoroutine(JedaPindahScene());
    }

    // Ini fungsi khusus untuk memutar suara -> tunggu -> pindah
    IEnumerator JedaPindahScene()
    {
        // 1. Cek apakah ada file suaranya?
        if (sumberSuara != null && suaraKlik != null)
        {
            // Bunyikan suara
            sumberSuara.PlayOneShot(suaraKlik);

            // TUNGGU SESUAI PANJANG AUDIO (Otomatis)
            // Jadi kalau audionya 3 detik, dia akan tunggu 3 detik.
            yield return new WaitForSeconds(suaraKlik.length);
        }
        else
        {
            // Kalau lupa pasang suara, pakai jeda default biar gak error
            yield return new WaitForSeconds(0.5f);
        }

        // 2. Setelah suara selesai 100%, baru pindah scene
        SceneManager.LoadScene("IntroCutscene");
    }

    public void KeluarGame()
    {
        // Kita juga bisa kasih suara buat tombol keluar kalau mau
        if (sumberSuara != null && suaraKlik != null) sumberSuara.PlayOneShot(suaraKlik);
        
        Debug.Log("Game Ditutup!");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BukaSettings()
    {
        if (sumberSuara != null && suaraKlik != null) sumberSuara.PlayOneShot(suaraKlik);
        Debug.Log("Fitur Settings belum aktif.");
    }
}