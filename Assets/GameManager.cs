using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text scoretext;

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
    
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; 
    }

    private void UpdateScore()
    {

        int scoreperSecond = 10;

        timer += Time.deltaTime;
        score = (int)(timer * scoreperSecond);
        scoretext.text = string.Format("{0:00000}", score);
    
    }
}
