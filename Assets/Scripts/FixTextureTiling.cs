using UnityEngine;

public class FixTextureTiling : MonoBehaviour
{
    [Header("Besar Kecilnya Pola Batu")]
    public float tileScale = 0.5f; // Semakin besar angka, semakin kecil batunya

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) return;

        // 1. Ambil ukuran tembok saat ini (setelah di-scale oleh generator)
        // Kita cari sisi mana yang paling panjang (X atau Z)
        float panjang = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        float tinggi = transform.lossyScale.y;

        // 2. Ubah Tiling Material secara instan
        // Rumus: Panjang Tembok * Skala Tile
        rend.material.mainTextureScale = new Vector2(panjang * tileScale, tinggi * tileScale);
    }
}