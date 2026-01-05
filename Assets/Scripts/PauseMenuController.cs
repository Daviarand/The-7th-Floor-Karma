// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class PauseMenuController : MonoBehaviour
// {
//     [Header("UI Panels")]
//     public GameObject pauseMenuUI;   
//     public GameObject optionsPanel;  

//     [Header("Player Reference")]
//     // Referensi ke script pergerakan player agar bisa dimatikan saat pause
//     // PENTING: Jika nama script kamu beda, ganti 'PlayerMovement' dengan nama script aslimu!
//     public PlayerMovement playerScript; 

//     [Header("Scene Names")]
//     public string mainMenuScene = "IntroCutscene"; 

//     public static bool GameIsPaused = false;

//     void Start()
//     {
//         if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
//         if (optionsPanel != null) optionsPanel.SetActive(false);
        
//         GameIsPaused = false;
//         Time.timeScale = 1f; 
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             if (GameIsPaused)
//             {
//                 ResumeGame();
//             }
//             else
//             {
//                 PauseGame();
//             }
//         }
//     }

//     public void ResumeGame()
//     {
//         if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
//         if (optionsPanel != null) optionsPanel.SetActive(false);

//         Time.timeScale = 1f; 
//         GameIsPaused = false;

//         // Kunci kursor lagi
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;

//         // NYALAKAN lagi script player agar bisa gerak/nengok
//         if (playerScript != null) playerScript.enabled = true;
//     }

//     void PauseGame()
//     {
//         if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        
//         Time.timeScale = 0f; 
//         GameIsPaused = true;

//         // Bebaskan kursor
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;

//         // MATIKAN script player agar kamera diam
//         if (playerScript != null) playerScript.enabled = false;
//     }

//     public void RestartLevel()
//     {
//         Time.timeScale = 1f; 
//         GameIsPaused = false;
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }

//     public void LoadMainMenu()
//     {
//         Time.timeScale = 1f;
//         GameIsPaused = false;
//         SceneManager.LoadScene(mainMenuScene);
//     }

//     public void QuitGame()
//     {
//         Debug.Log("Keluar dari Game...");
//         Application.Quit();
//     }
    
//     public void OpenOptions()
//     {
//         pauseMenuUI.SetActive(false); 
//         optionsPanel.SetActive(true);
//     }

//     public void CloseOptions()
//     {
//         optionsPanel.SetActive(false);
//         pauseMenuUI.SetActive(true);
//     }
// }









using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuUI;   
    public GameObject optionsPanel;  

    [Header("Player Reference")]
    public PlayerMovement playerScript; 

    [Header("Scene Names")]
    public string mainMenuScene = "IntroCutscene"; 

    public static bool GameIsPaused = false;

    void Start()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        
        GameIsPaused = false;
        Time.timeScale = 1f; 
        
        // Pastikan suara nyala saat game mulai (untuk jaga-jaga)
        AudioListener.pause = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        Time.timeScale = 1f; 
        GameIsPaused = false;

        // Kunci kursor lagi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Nyalakan player
        if (playerScript != null) playerScript.enabled = true;

        // --- UPDATE: NYALAKAN SUARA LAGI ---
        AudioListener.pause = false; 
    }

    void PauseGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f; 
        GameIsPaused = true;

        // Bebaskan kursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Matikan player
        if (playerScript != null) playerScript.enabled = false;

        // --- UPDATE: MATIKAN SEMUA SUARA ---
        AudioListener.pause = true;
    }

    public void RestartLevel()
    {
        // Reset semua kondisi sebelum reload
        Time.timeScale = 1f; 
        GameIsPaused = false;
        AudioListener.pause = false; // PENTING: Nyalakan suara lagi sebelum restart

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false; // PENTING: Nyalakan suara lagi sebelum pindah

        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();
    }
    
    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false); 
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}