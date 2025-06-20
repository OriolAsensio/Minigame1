using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text scoreText;

    [Header("Scene Loading")]
    [Tooltip("Nombre de la escena a cargar desde el editor")]
    [SerializeField] private string sceneToLoad;

    private int score;
    private float timer;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        UpdateScore();
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Carga la escena asignada en el editor.
    /// </summary>
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("GameManager: 'sceneToLoad' está vacío. Asigna un nombre en el Inspector.");
            return;
        }
        Time.timeScale = 1f; // Asegura tiempo normal
        SceneManager.LoadScene(sceneToLoad);
    }

    private void UpdateScore()
    {
        int scoreperSecond = 10;

        timer += Time.deltaTime;
        score = (int)(timer * scoreperSecond);
        scoreText.text = string.Format("{0:00000}", score);
    }
    /// <summary>
    /// Sale del juego o detiene la reproducción en el editor.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Si estamos en el editor de Unity, detenemos la reproducción
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si es una build, cerramos la aplicación
        Application.Quit();
#endif
    }
}

