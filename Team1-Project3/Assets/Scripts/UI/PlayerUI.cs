using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject powerBar;
    [SerializeField] private Image powerBarImage;
    [SerializeField] private Color lowColor;
    [SerializeField] private Color highColor;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource clickNoiseSource;
    [SerializeField] private TextMeshProUGUI gradeScore;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private GateHandler gateHandler;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject resumeButton;
    private float normalizedPower;
    private SceneLoader sceneLoader;

    private bool gamePaused;
    private bool levelEnded = false;

    public void SetPower(float power)
    {
        if (power > 0 && power < 11)
        {
            normalizedPower = Mathf.InverseLerp(0,10,power);
        }
    }

    private void Start()
    {
        sceneLoader = SceneLoader.instance;
    }

    // Update is called once per frame
    void Update()
    {
        ScalePowerBar(normalizedPower);
        ColorPowerBar(normalizedPower);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                if(!levelEnded)
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ScalePowerBar(float normalizedValue)
    {
        Vector3 newScale = new(normalizedValue, 1, 1);
        powerBar.transform.localScale = newScale;
    }

    private void ColorPowerBar(float normalizedValue)
    {
        powerBarImage.color = Color.Lerp(lowColor, highColor, normalizedValue);
    }

    public void PauseGame()
    {
        Debug.Log("paused game");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        DisplayGradeAndTime();
        nextLevelButton.SetActive(false);
        gamePaused = true;
    }

    public void FallOffMapRestartGame()
    {
        levelEnded = true;
        Debug.Log("fell off map");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        nextLevelButton.SetActive(false);
        gamePaused = true;
    }

    public void ResumeGame()
    {
        ButtonClickNoise();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    public void EndLevel()
    {
        levelEnded = true;
        Debug.Log("ended game");
        Time.timeScale = 0;
        DisplayGradeAndTime();
        pauseMenu.SetActive(true);
        nextLevelButton.SetActive(true);
        resumeButton.SetActive(false);
        gamePaused = true;
    }

    public void OnClickNextLevelButton()
    {
        Time.timeScale = 1;
        sceneLoader.NextLevel();
    }

    public void OnClickRestartButton()
    {
        Time.timeScale = 1;
        sceneLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickMainMenuButton()
    {
        Time.timeScale = 1;
        sceneLoader.LoadLevel(0);
    }

    public void ButtonClickNoise()
    {
        clickNoiseSource.Play();
    }


    void DisplayGradeAndTime()
    {
        float currentTime = player.GetComponent<GateTimer>().courseTime;
        TimeRank rank = player.GetComponent<GateHandler>().DetermineScore();
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        string formattedTime = timeSpan.ToString("m\\:ss");
        time.text = formattedTime;
        gradeScore.text = rank.ToString();
    }
}
