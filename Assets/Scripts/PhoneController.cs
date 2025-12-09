using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject uiHPFrame; // Masukkan objek "HP_Frame" (Panel UI HP) ke sini
    public bool isSignalLost = false; // Status apakah sinyal hilang (dikontrol GameManager)

    private bool isPhoneOpen = false; // Status apakah HP sedang dibuka

    void Start()
    {
        // Pastikan HP tertutup saat awal game
        if (uiHPFrame != null)
        {
            uiHPFrame.SetActive(false);
            isPhoneOpen = false;
        }
        else
        {
            Debug.LogError("UI HP Frame belum dimasukkan ke script PhoneController!");
        }
    }

    void Update()
    {
        // Input Tombol 'M' untuk buka/tutup HP
        if (Input.GetKeyDown(KeyCode.M))
        {
            TogglePhone();
        }
    }

    void TogglePhone()
    {
        // Jika sinyal hilang (fase klimaks), HP tidak bisa dibuka
        if (isSignalLost)
        {
            // Opsional: Mainkan suara "kresek-kresek" atau tampilkan pesan "No Signal"
            Debug.Log("Sinyal Hilang! HP tidak berfungsi.");
            return;
        }

        if (uiHPFrame != null)
        {
            isPhoneOpen = !isPhoneOpen; // Balik status (Buka jadi Tutup, Tutup jadi Buka)
            uiHPFrame.SetActive(isPhoneOpen);
        }
    }

    // Fungsi ini dipanggil oleh GameManager saat fase klimaks
    public void ForceClosePhoneAndDisable()
    {
        isSignalLost = true;
        isPhoneOpen = false;
        if (uiHPFrame != null)
        {
            uiHPFrame.SetActive(false);
        }
    }
}