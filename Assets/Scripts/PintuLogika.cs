// using UnityEngine;

// public class PintuLogika : MonoBehaviour
// {
//     void OnTriggerEnter(Collider other)
//     {
//         // Cek apakah yang nabrak adalah Player
//         if (other.CompareTag("Player"))
//         {
//             // Cek apakah koin sudah 100?
//             if (GameManager.instance.koinTerkumpul >= 100)
//             {
//                 Debug.Log("MENANG! ANDA BEBAS!");
//                 // Nanti kita tambahkan layar 'You Win' di sini
//                 Time.timeScale = 0; // Stop game
//             }
//             else
//             {
//                 Debug.Log("Pintu terkunci! Cari koin terakhir!");
//             }
//         }
//     }
// }



using UnityEngine;
using UnityEngine.SceneManagement; // WAJIB ADA untuk pindah scene

public class PintuLogika : MonoBehaviour
{
    [Header("Nama Scene Tujuan")]
    public string namaSceneOutro = "OutroCutscene"; // Pastikan nama ini SAMA PERSIS dengan nama file scene kamu

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang nabrak adalah Player
        if (other.CompareTag("Player"))
        {
            // if (GameManager.instance.koinTerkumpul >= 100)
            if (GameManager.instance.koinTerkumpul >= 2)
            {
                Debug.Log("Pindah ke Outro...");
                
                // Langsung pindah scene
                SceneManager.LoadScene(namaSceneOutro);
            }
            else
            {
                Debug.Log("Pintu terkunci! Koin saat ini: " + GameManager.instance.koinTerkumpul);
            }
        }
    }
}