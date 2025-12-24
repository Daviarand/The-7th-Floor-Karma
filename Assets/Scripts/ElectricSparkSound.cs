using UnityEngine;
using System.Collections;

public class ElectricSparkSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sparkClip; // Masukkan file suara di sini

    [Header("Pengaturan Variasi")]
    [Range(0f, 1f)]
    public float chanceForDouble = 0.5f; // 50% kemungkinan bunyi double
    public float doubleBeepSpeed = 0.15f; // Jeda cepat antar 'beep-beep' (detik)
    
    [Header("Jeda Antar Loop")]
    public float minDelay = 1.0f; // Jeda minimal (misal 1 detik)
    public float maxDelay = 3.0f; // Jeda maksimal (misal 3 detik)

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Matikan Loop bawaan AudioSource supaya script yang kontrol
        audioSource.loop = false;

        StartCoroutine(PlayElectricLoop());
    }

    IEnumerator PlayElectricLoop()
    {
        while (true) // Loop selamanya
        {
            // --- BUNYI PERTAMA ---
            PlaySound();

            // --- CEK APAKAH BUNYI DOUBLE? ---
            // Random.value menghasilkan angka 0.0 sampai 1.0
            if (Random.value < chanceForDouble) 
            {
                // Tunggu sebentar banget (efek stutter)
                yield return new WaitForSeconds(doubleBeepSpeed);
                
                // --- BUNYI KEDUA ---
                PlaySound();
            }

            // --- JEDA MENUNJU BUNYI BERIKUTNYA ---
            // Kita acak jedanya biar tidak monoton
            float randomWait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomWait);
        }
    }

    void PlaySound()
    {
        // Gunakan PlayOneShot supaya suara bisa menumpuk (tidak saling memotong kaku)
        if (sparkClip != null)
        {
            audioSource.PlayOneShot(sparkClip); 
        }
        else 
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}