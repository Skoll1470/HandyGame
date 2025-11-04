using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public CanvasScript canvas;

    public Texture2D normalModeImage;

    public Texture2D hardModeImage;

    public RawImage autoScrollingImage;

    public int autoScrollingSpeed = 5;

    public TMP_Text autoScrollingSpeedText;

    public Image settingsImage;

    private DataClass dataClass;

    private bool isGamePaused = false;
    private class DataClass
    {
        public bool autoScrolling = false;
        public int autoScrollingSpeed = 5;
    }

    private void Start()
    {
        dataClass = new DataClass();
        ReadJson();
        SaveJson();
        autoScrollingSpeedText.text = autoScrollingSpeed.ToString() + " s";
        settingsImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateAutoScrolling();
        }
    }

    public void LoadCombatScene(int sceneIndex)
    {
        SceneManager.LoadScene("Combat"+sceneIndex.ToString());
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(dataClass);
        File.WriteAllText(Application.persistentDataPath + "/Settings.json", json);
        autoScrollingImage.texture = dataClass.autoScrolling ? hardModeImage : normalModeImage;
        autoScrollingSpeedText.text = dataClass.autoScrollingSpeed.ToString() + " s";
    }

    public void UpdateAutoScrolling()
    {
        dataClass.autoScrolling = !dataClass.autoScrolling;
        SaveJson();
    }

    public void UpdateAutoScrollingSpeed(bool isIncrementing)
    {
        if (isIncrementing)
        {
            autoScrollingSpeed++;
        }
        else
        {
            if (autoScrollingSpeed > 1)
            {
                autoScrollingSpeed--;
            }
        }
        dataClass.autoScrollingSpeed = autoScrollingSpeed;
        SaveJson();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        canvas.pauseImage.gameObject.SetActive(isGamePaused);
        SaveJson();
    }

    public void ShowHideSettings(bool isShowing)
    {
        settingsImage.gameObject.SetActive(isShowing);
    }

    private void ReadJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/Settings.json");
        dataClass = JsonUtility.FromJson<DataClass>(json);
        autoScrollingSpeed = dataClass.autoScrollingSpeed;
    }
}