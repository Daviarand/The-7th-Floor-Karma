using UnityEngine;

public class Koin : MonoBehaviour
{
    // Fungsi bawaan Unity saat sesuatu menembus benda Trigger
    void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang nabrak itu Player?
        if (other.CompareTag("Player"))
        {
            // 1. Lapor ke bos (GameManager)
            GameManager.instance.TambahKoin();

            // 2. Hancurkan diri sendiri
            Destroy(gameObject);
        }
    }
}