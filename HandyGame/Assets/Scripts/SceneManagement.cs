using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public CanvasScript canvas;

    public Texture2D normalModeImage;

    public Texture2D hardModeImage;

    public RawImage difficultyImage;

    private DatatClass dataClass;

    private bool isGamePaused = false;

    private void Start()
    {
        dataClass = new DatatClass();
        SaveJson();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private class DatatClass
    {
        public bool difficulty = false;
    }
    public void LoadCombatScene(int sceneIndex)
    {
        SceneManager.LoadScene("Combat"+sceneIndex.ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(dataClass);
        File.WriteAllText(Application.persistentDataPath + "/Settings.json", json);
        difficultyImage.texture = dataClass.difficulty ? hardModeImage : normalModeImage;
    }

    public void ChangeDifficulty()
    {
        dataClass.difficulty = !dataClass.difficulty;
        SaveJson();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        canvas.pauseImage.gameObject.SetActive(isGamePaused);
    }
}