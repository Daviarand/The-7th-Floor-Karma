using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public float mapSize = 100f; // Ukuran area yang dicover kamera
    private Camera mapCam;

    void Start()
    {
        // 1. Cari MazeGenerator untuk tahu ukuran labirin
        MazeGeneratorPerfectLoop generator = FindAnyObjectByType<MazeGeneratorPerfectLoop>();
        if (generator != null)
        {
            // Rumus size dari MazeGenerator: 10f * scaleLantai
            float realSize = 10f * generator.scaleLantai;
            mapSize = realSize; 
        }

        // 2. Buat GameObject baru untuk Kamera Map
        GameObject camObj = new GameObject("MinimapCamera");
        mapCam = camObj.AddComponent<Camera>();
        
        // Hapus AudioListener bawaan agar tidak konflik dengan Main Camera
        // (Biasanya Unity otomatis nambah AudioListener di kamera baru)
        AudioListener al = camObj.GetComponent<AudioListener>();
        if (al) Destroy(al);

        // 3. Setting Posisi (Di atas tengah labirin)
        // Asumsi labirin di (0,0,0), kita taruh kamera tinggi di Y
        camObj.transform.position = new Vector3(0, 100, 0);
        camObj.transform.rotation = Quaternion.Euler(90, 0, 0);

        // 4. Setting Kamera
        mapCam.orthographic = true;
        mapCam.orthographicSize = (mapSize / 2) + 5f; // Tambah padding 5 unit
        mapCam.clearFlags = CameraClearFlags.SolidColor;
        mapCam.backgroundColor = Color.black; // Background hitam
        
        // 5. Setting Viewport (Tampil di Pojok Kanan Atas)
        // x, y, w, h (0-1)
        mapCam.rect = new Rect(0.7f, 0.7f, 0.25f, 0.25f);
        
        // 6. Pastikan render di atas UI/Main Camera
        mapCam.depth = 10; 
    }

    void Update()
    {
        // Fitur Tambahan: Tekan 'M' untuk memperbesar map
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapCam.rect.width < 0.5f)
                mapCam.rect = new Rect(0.1f, 0.1f, 0.8f, 0.8f); // Layar Penuh
            else
                mapCam.rect = new Rect(0.7f, 0.7f, 0.25f, 0.25f); // Pojok Kecil
        }
    }
}
