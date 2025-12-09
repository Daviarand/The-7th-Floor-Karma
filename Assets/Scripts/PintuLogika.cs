using UnityEngine;

public class PintuLogika : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang nabrak adalah Player
        if (other.CompareTag("Player"))
        {
            // Cek apakah koin sudah 100? (sementara 3 untuk testing)
            if (GameManager.instance.koinTerkumpul >= 3)
            {
                Debug.Log("MENANG! ANDA BEBAS!");
                // Nanti kita tambahkan layar 'You Win' di sini
                Time.timeScale = 0; // Stop game
            }
            else
            {
                Debug.Log("Pintu terkunci! Cari koin terakhir!");
            }
        }
    }
}