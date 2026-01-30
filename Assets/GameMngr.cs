using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float survivalTime = 30f;
    public float detachTime = 2f;
    
    [Header("UI Elements")]
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI timerText;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource; 
    public AudioClip crashSFX; 
    public AudioClip winSFX; 

    [Header("References")]
    public Transform cameraTransform;
    public MonoBehaviour asteroidSpawner;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool hasDetached = false;
    private bool spawnerDisabled = false;

    void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        
        if (musicSource != null) musicSource.Stop();
        
        UpdateTimerUI();
    }

    void Update()
    {
        if (!gameStarted || gameEnded) return;

        survivalTime -= Time.deltaTime;
        UpdateTimerUI();

        if (survivalTime <= 6f && !spawnerDisabled)
        {
            if (asteroidSpawner != null) asteroidSpawner.enabled = false;
            spawnerDisabled = true;
            Debug.Log("Spawner disabled - clear skies ahead!");
        }

        if (survivalTime <= detachTime && !hasDetached)
        {
            DetachCamera();
        }

        if (survivalTime <= 0)
        {
            WinGame();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float displayTime = Mathf.Max(0, survivalTime);
            timerText.text = displayTime.ToString("F2") + "s";
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        if (musicSource != null) musicSource.Play();
    }

    public void ExplodeShip()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (sfxSource != null && crashSFX != null) sfxSource.PlayOneShot(crashSFX);

        if (musicSource != null) musicSource.Stop();
        
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void WinGame()
    {
        gameEnded = true;

        if (sfxSource != null && winSFX != null) sfxSource.PlayOneShot(winSFX);

        if (musicSource != null) musicSource.Stop();
        
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void DetachCamera()
    {
        hasDetached = true;
        cameraTransform.SetParent(null); 
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}