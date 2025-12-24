using UnityEngine;
using System.Collections;

public class ElevatorFlicker : MonoBehaviour
{
    [Header("Daftar Lampu")]
    public Light[] lightsToFlicker;

    [Header("Audio Listrik")]
    public AudioSource electricAudioSource; // Drag Audio Source ke sini
    public AudioClip electricSound;         // Drag file suara ke sini
    [Range(0f, 1f)] public float soundVolume = 0.5f; // Pengatur keras suara

    [Header("Pengaturan Terang Gelap")]
    public float minIntensity = 0f;
    public float maxIntensity = 5f;

    [Header("Pengaturan Kecepatan Kedip")]
    public float minSpeed = 0.05f;    
    public float maxSpeed = 0.2f;     

    void Start()
    {
        // Mencari AudioSource otomatis jika lupa di-drag
        if (electricAudioSource == null) 
            electricAudioSource = GetComponent<AudioSource>();

        StartCoroutine(Flickering());
    }

    IEnumerator Flickering()
    {
        while (true)
        {
            // SAFETY
            if (minSpeed < 0.05f) minSpeed = 0.05f;
            if (maxSpeed < 0.05f) maxSpeed = 0.05f;

            // 1. Acak intensitas cahaya
            float targetIntensity = Random.Range(minIntensity, maxIntensity);

            // 2. Update Lampu
            foreach (Light l in lightsToFlicker)
            {
                if (l != null) l.intensity = targetIntensity;
            }

            // 3. LOGIKA SUARA (Baru!)
            // Jika lampu menyala cukup terang (di atas 0.1), mainkan suara
            if (targetIntensity > 0.1f)
            {
                if (electricAudioSource != null && electricSound != null)
                {
                    // Ubah pitch sedikit biar suaranya tidak monoton (efek rusak)
                    electricAudioSource.pitch = Random.Range(0.8f, 1.2f);
                    
                    // Mainkan suara sekali (One Shot)
                    electricAudioSource.PlayOneShot(electricSound, soundVolume);
                }
            }

            // 4. Tunggu
            float waitTime = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(waitTime);
        }
    }
}