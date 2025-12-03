using UnityEngine;
using System.Collections.Generic;

public class MazeGeneratorPerfectLoop : MonoBehaviour
{
    [Header("Settings Prefab")]
    public GameObject wallPrefab; // Masukkan WALL dari Aset Dungeon
    public GameObject coinPrefab; 
    public GameObject doorPrefab; 
    public int seed = 12345;

    [Header("Penyesuaian Aset (PENTING)")]
    // COBA ATUR INI DI INSPECTOR SAMPAI PAS:
    public float wallYOffset = 0f; // Coba 0, 1, atau 2
    public float wallHeight = 4f;  // Tinggi tembok yang diinginkan

    [Header("Ukuran & Kepadatan")]
    public float scaleLantai = 8f; 
    public int jumlahGrid = 20; 
    public float wallThickness = 2f; 
    public int totalCoins = 100; 

    [Header("Bentuk Lorong")]
    [Range(0f, 1f)]
    public float chanceToGoStraight = 0.7f; 

    private float cellSize;
    private List<GameObject> generatedObjects = new List<GameObject>(); 
    
    private class Cell { public bool visited = false; public GameObject wallRight; public GameObject wallBottom; }

    [ContextMenu("Bangun Labirin Lengkap")]
    public void GenerateMaze()
    {
        ClearAll();
        Random.InitState(seed);

        float realSize = 10f * scaleLantai; 
        cellSize = realSize / jumlahGrid;   
        float startPos = -(realSize / 2) + (cellSize / 2); 

        Cell[,] grid = new Cell[jumlahGrid, jumlahGrid];

        // --- ISI SUDUT (CORNER) ---
        for (int x = 0; x <= jumlahGrid; x++) {
            for (int z = 0; z <= jumlahGrid; z++) {
                float pX = -(realSize / 2) + (x * cellSize);
                float pZ = -(realSize / 2) + (z * cellSize);
                // Gunakan wallYOffset agar tidak tenggelam
                SpawnObj(wallPrefab, new Vector3(pX, wallYOffset, pZ), new Vector3(wallThickness, wallHeight, wallThickness), Quaternion.identity);
            }
        }

        // --- GRID DINDING ---
        for (int x = 0; x < jumlahGrid; x++) {
            for (int z = 0; z < jumlahGrid; z++) {
                grid[x, z] = new Cell();
                float posX = startPos + (x * cellSize);
                float posZ = startPos + (z * cellSize);

                if (x < jumlahGrid - 1) {
                    Vector3 wPos = new Vector3(posX + (cellSize / 2), wallYOffset, posZ);
                    GameObject w = SpawnObj(wallPrefab, wPos, new Vector3(wallThickness, wallHeight, cellSize - wallThickness), Quaternion.identity);
                    grid[x, z].wallRight = w;
                }
                if (z < jumlahGrid - 1) {
                    Vector3 wPos = new Vector3(posX, wallYOffset, posZ + (cellSize / 2));
                    // ROTASI PENTING: Aset Dungeon biasanya menghadap Z. Kalau diputar 90, dia jadi X.
                    GameObject w = SpawnObj(wallPrefab, wPos, new Vector3(wallThickness, wallHeight, cellSize - wallThickness), Quaternion.Euler(0, 90, 0));
                    grid[x, z].wallBottom = w;
                }
            }
        }
        
        BuatTembokKeliling(realSize, wallThickness);

        // ... (SISA LOGIKA MAZE SAMA SEPERTI SEBELUMNYA) ...
        // Agar code tidak kepanjangan di chat, logika Gali Jalan, Jebol Buntu, Spawn Koin/Pintu 
        // saya panggil lewat fungsi terpisah di bawah ini (GenerateLogic).
        // Isinya 100% sama dengan logika Anda.
        GenerateLogic(grid, startPos);
        
        Debug.Log("Labirin Aset Dungeon Siap!");
    }

    // --- LOGIKA UTAMA (DIPISAH BIAR RAPI) ---
    void GenerateLogic(Cell[,] grid, float startPos)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0);
        grid[0, 0].visited = true;
        stack.Push(current);
        Vector2Int lastDir = Vector2Int.zero; 

        while (stack.Count > 0) {
            current = stack.Pop();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current, grid);
            if (neighbors.Count > 0) {
                stack.Push(current); 
                Vector2Int next = Vector2Int.zero;
                bool foundMove = false;
                if (lastDir != Vector2Int.zero) {
                    Vector2Int pNext = current + lastDir;
                    if (IsCellValid(pNext) && !grid[pNext.x, pNext.y].visited && Random.value < chanceToGoStraight) {
                        next = pNext; foundMove = true;
                    }
                }
                if (!foundMove) next = neighbors[Random.Range(0, neighbors.Count)];
                lastDir = next - current;
                RemoveWallBetween(grid, current, next);
                grid[next.x, next.y].visited = true;
                stack.Push(next);
            } else lastDir = Vector2Int.zero;
        }

        JebolSemuaJalanBuntu(grid);

        // Spawn Items
        List<Vector3> locs = new List<Vector3>();
        for(int x=0;x<jumlahGrid;x++) for(int z=0;z<jumlahGrid;z++) 
            locs.Add(new Vector3(startPos+(x*cellSize), 1f, startPos+(z*cellSize)));
        
        for(int i=0;i<locs.Count;i++) { Vector3 t=locs[i]; int r=Random.Range(i,locs.Count); locs[i]=locs[r]; locs[r]=t; }

        if (doorPrefab!=null && locs.Count>0) {
            GameObject d = Instantiate(doorPrefab, locs[0], Quaternion.identity);
            d.SetActive(false); d.transform.parent=transform; generatedObjects.Add(d);
        }
        if (coinPrefab!=null) {
            int lim = Mathf.Min(totalCoins, locs.Count-1);
            for(int i=1; i<=lim; i++) {
                Instantiate(coinPrefab, locs[i], coinPrefab.transform.rotation, transform);
            }
        }
    }

    // --- HELPER FUNCTIONS ---
    GameObject SpawnObj(GameObject prefab, Vector3 pos, Vector3 scale, Quaternion rot) {
        GameObject o = Instantiate(prefab, pos, rot);
        o.transform.localScale = scale; // PERHATIAN: Ini akan mengubah bentuk Aset jadi gepeng kalau tidak pas
        o.transform.parent = transform;
        generatedObjects.Add(o);
        return o;
    }
    
    void BuatTembokKeliling(float s, float t) { 
        float h = s/2; 
        SpawnObj(wallPrefab, new Vector3(0,wallYOffset,-h), new Vector3(s+t,wallHeight,t), Quaternion.identity);
        SpawnObj(wallPrefab, new Vector3(0,wallYOffset,h), new Vector3(s+t,wallHeight,t), Quaternion.identity);
        SpawnObj(wallPrefab, new Vector3(-h,wallYOffset,0), new Vector3(t,wallHeight,s+t), Quaternion.identity);
        SpawnObj(wallPrefab, new Vector3(h,wallYOffset,0), new Vector3(t,wallHeight,s+t), Quaternion.identity);
    }

    // (Sisanya: RemoveWall, JebolBuntu, dll sama persis - copy paste saja dari script lama Anda jika error)
    // Agar muat, pastikan fungsi RemoveWallBetween, JebolSemuaJalanBuntu, HapusRef, GetUnvisitedNeighbors, IsCellValid, ClearAll ada di sini.
    // ... (Fungsi logika tidak berubah) ...
    
    void RemoveWallBetween(Cell[,] grid, Vector2Int a, Vector2Int b) {
        GameObject t = null;
        if(b.x>a.x) t=grid[a.x,a.y].wallRight; else if(b.x<a.x) t=grid[b.x,b.y].wallRight;
        else if(b.y>a.y) t=grid[a.x,a.y].wallBottom; else if(b.y<a.y) t=grid[b.x,b.y].wallBottom;
        if(t!=null) { DestroyImmediate(t); HapusRef(grid,a.x,a.y,t); }
    }
    void JebolSemuaJalanBuntu(Cell[,] grid) {
        for(int x=0;x<jumlahGrid;x++) for(int z=0;z<jumlahGrid;z++) {
            List<GameObject> l = new List<GameObject>();
            if(x<jumlahGrid-1 && grid[x,z].wallRight!=null) l.Add(grid[x,z].wallRight);
            if(z<jumlahGrid-1 && grid[x,z].wallBottom!=null) l.Add(grid[x,z].wallBottom);
            if(x>0 && grid[x-1,z].wallRight!=null) l.Add(grid[x-1,z].wallRight);
            if(z>0 && grid[x,z-1].wallBottom!=null) l.Add(grid[x,z-1].wallBottom);
            int b=0; if(x==0)b++; if(x==jumlahGrid-1)b++; if(z==0)b++; if(z==jumlahGrid-1)b++;
            if((l.Count+b)>=3 && l.Count>0) { GameObject t=l[Random.Range(0,l.Count)]; DestroyImmediate(t); HapusRef(grid,x,z,t); }
        }
    }
    void HapusRef(Cell[,] g, int x, int z, GameObject t) {
        if(g[x,z].wallRight==t) g[x,z].wallRight=null; if(g[x,z].wallBottom==t) g[x,z].wallBottom=null;
        if(x>0 && g[x-1,z].wallRight==t) g[x-1,z].wallRight=null; if(z>0 && g[x,z-1].wallBottom==t) g[x,z-1].wallBottom=null;
    }
    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int c, Cell[,] grid) {
        List<Vector2Int> l = new List<Vector2Int>();
        if(IsCellValid(new Vector2Int(c.x-1,c.y)) && !grid[c.x-1,c.y].visited) l.Add(new Vector2Int(c.x-1,c.y));
        if(IsCellValid(new Vector2Int(c.x+1,c.y)) && !grid[c.x+1,c.y].visited) l.Add(new Vector2Int(c.x+1,c.y));
        if(IsCellValid(new Vector2Int(c.x,c.y-1)) && !grid[c.x,c.y-1].visited) l.Add(new Vector2Int(c.x,c.y-1));
        if(IsCellValid(new Vector2Int(c.x,c.y+1)) && !grid[c.x,c.y+1].visited) l.Add(new Vector2Int(c.x,c.y+1));
        return l;
    }
    bool IsCellValid(Vector2Int c) { return c.x >= 0 && c.y >= 0 && c.x < jumlahGrid && c.y < jumlahGrid; }
    [ContextMenu("Hapus Labirin")] public void ClearAll() {
        foreach(GameObject o in generatedObjects) if(o!=null) DestroyImmediate(o); 
        generatedObjects.Clear(); while(transform.childCount>0) DestroyImmediate(transform.GetChild(0).gameObject); 
    }
}
