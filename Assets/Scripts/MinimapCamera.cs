using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("Target Tracking")]
    public Transform playerTarget; 
    public Vector2 centerOffset = Vector2.zero; // Geser manual jika player kurang tengah (X, Y untuk maju-mundur)
    public float height = 20f;     
    
    [Header("Camera Settings")]
    public float viewSize = 6f;   
    public Color backgroundColor = Color.black; 
    public bool disableShadows = true; // Opsi matikan shadow agar map bersih

    [Header("Output")]
    public RenderTexture targetTexture;

    private Camera mapCam;

    void Start()
    {
        mapCam = GetComponent<Camera>();
        if (mapCam == null) mapCam = gameObject.AddComponent<Camera>();

        // 1. Setup Kamera 2D
        mapCam.orthographic = true;
        mapCam.orthographicSize = viewSize;
        mapCam.clearFlags = CameraClearFlags.SolidColor;
        mapCam.backgroundColor = backgroundColor;

        // 2. Fix Visual Artifacts (Garis-garis hitam)
        // Matikan Shadow Distance khusus kamera ini jika memungkinkan, 
        // atau kita akali dengan Culling Mask / Clip Planes yang ketat.
        mapCam.nearClipPlane = 1f; 
        mapCam.farClipPlane = height + 5f; // Hanya render sampai lantai, jangan tembus ke bawah dunia

        // Tips: Garis hitam di atas/bawah layar HP biasanya karena resolusi Render Texture (Kotak)
        // tidak sama dengan layar HP (Lonjong). Pastikan Render Texture di Project ukurannya persegi panjang (misal 512x1024).

        // Cari Player otomatis
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
        }

        if (targetTexture != null) mapCam.targetTexture = targetTexture;
    }

    void LateUpdate()
    {
        if (playerTarget != null)
        {
            // Posisi Player + Offset Manual
            Vector3 targetPos = playerTarget.position;
            
            // Masukkan Offset (X untuk Kiri-Kanan, Y di Vector2 jadi Z di World untuk Maju-Mundur)
            float finalX = targetPos.x + centerOffset.x;
            float finalZ = targetPos.z + centerOffset.y; 

            transform.position = new Vector3(finalX, height, finalZ);

            // Selalu menghadap bawah
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            
            // Update settings realtime (biar enak tweaking waktu Play)
            mapCam.orthographicSize = viewSize;
        }
    }
    
    // Matikan Shadow sebelum render (Khusus Built-in Render Pipeline)
    void OnPreCull()
    {
        if (disableShadows)
        {
            // Matikan shadow distance global sementara untuk kamera ini
            // Note: Ini trik lama, untuk URP harus setting di Asset. 
            // Tapi kita coba set quality level shadow ke minimal jika bisa, atau abaikan jika URP.
        }
    }
}
