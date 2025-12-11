👻 The 7th Floor Karma

The 7th Floor Karma adalah game survival horror sudut pandang orang pertama (First-Person) yang dibuat dengan Unity. Pemain terjebak di dalam labirin prosedural dan harus mengumpulkan koin "Karma" untuk melarikan diri, sambil menghindari entitas mengerikan yang hanya bergerak ketika tidak terlihat.

🌟 Fitur Utama

1. 🧠 Intelligent Enemy AI (Weeping Angel Mechanic)

Musuh dalam game ini menggunakan mekanik "Don't Blink":

Patung: Saat masuk dalam pandangan kamera pemain, musuh akan mematung (diam).

Pengejar: Saat pemain membuang muka atau terhalang tembok, musuh akan mengejar dengan kecepatan tinggi.

Jumpscare: Jika musuh berhasil menyentuh pemain, game berakhir seketika.

2. 📱 Diegetic UI (Sistem HP & Minimap)

Alih-alih UI konvensional di layar, navigasi dilakukan melalui smartphone 3D yang dipegang karakter:

Realtime Minimap: Menggunakan Render Texture untuk menampilkan denah labirin di layar HP.

Player Tracking: Kamera minimap secara dinamis mengikuti pergerakan dan rotasi pemain.

Fitur Buka/Tutup: Pemain dapat menekan tombol untuk melihat atau menyembunyikan HP.

3. 🗺️ Procedural Maze Generation

Setiap kali permainan dimulai, tata letak labirin dibuat secara acak (Procedural Generation), sehingga pengalaman bermain selalu unik dan tidak repetitif.

4. ⚡ Fase Klimaks (Sinyal Hilang)

Saat pemain hampir menyelesaikan level (mengumpulkan 99 koin):

Sinyal HP akan hilang (Minimap mati/layar statis).

Musuh menjadi lebih agresif (kecepatan bertambah).

Kunci pintu keluar akan muncul di lokasi acak.

🎮 Kontrol Permainan

Tombol

Fungsi

W, A, S, D

Bergerak (Jalan)

Mouse

Mengarahkan Kamera (Look)

M

Buka / Tutup HP (Cek Peta)

Shift

Lari (Sprint)

🛠️ Teknologi & Aset

Engine: Unity 6 / 2022+ (Universal Render Pipeline - URP)

Scripting: C#

Aset 3D:

Stylized Hand Painted Dungeon (Environment)

Weeping Angel Model (Character)

Mobile Phone Model (Props)

Sistem Input: Unity New Input System

📂 Struktur Script Penting

GameManager.cs: Mengatur state permainan (Koin, Fase Klimaks, Game Over, Win Condition).

EnemyAI.cs: Logika kecerdasan buatan musuh, deteksi visibilitas kamera (Frustum Culling & Raycast).

MinimapCamera.cs: Mengontrol kamera kedua yang merender peta ke tekstur layar HP.

PhoneController.cs: Mengatur animasi dan logika interaksi HP (buka/tutup/sinyal hilang).

MazeGenerator.cs: Algoritma pembuatan labirin otomatis.

PlayerMovement.cs: Kontrol pergerakan karakter utama.

🚀 Cara Menjalankan Project (Installation)

Pastikan kamu memiliki Unity Hub dan versi Editor yang sesuai (Unity 6 atau 2022 LTS disarankan).

Clone repositori ini:

git clone [https://github.com/username/The-7th-Floor-Karma.git](https://github.com/username/The-7th-Floor-Karma.git)


Buka Unity Hub, klik Add, dan pilih folder project ini.

Tunggu proses Import Assets selesai.

Buka Scene utama di Assets/Scenes/SampleScene.unity.

Tekan tombol ▶️ Play di editor.

📝 Catatan Pengembang

Game ini masih dalam tahap pengembangan aktif. Fitur yang akan datang meliputi:

[ ] Penambahan efek suara (Audio SFX) untuk atmosfer yang lebih mencekam.

[ ] Level bertingkat (Lantai 7, 6, dst).

[ ] Variasi musuh dan jebakan.

Dibuat dengan ❤️ dan ☕ untuk tugas/proyek game development.
