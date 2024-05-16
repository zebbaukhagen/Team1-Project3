using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    // Not implemented yet
    // private AudioManager audioManager;

    // Used for level select
    private List<bool> listOfLevelsCompleted = new List<bool>();
    private bool levelOneCompleted = false;
    private bool levelTwoCompleted = false;
    private bool levelThreeCompleted = false;

    public List<bool> ListOfLevelsCompleted
    {
        get => listOfLevelsCompleted;
        private set => listOfLevelsCompleted = value;
    }
    private void Awake()
    {
        Debug.Log("SceneLoader awakened.");
        // Ensure only one instance of SceneLoader exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("SceneLoader destroyed.");
        }
    }

    private void Start()
    {
        //audioManager = AudioManager.instance;
        //Debug.Log("audioManager instance is equal to " + audioManager);
        //InitializeLevelCompletionList();


    }

    private void Update()
    {
        // CheatCodes();
    }

    private void InitializeLevelCompletionList()
    {
        // Not used yet
        listOfLevelsCompleted.Add(levelOneCompleted);
        listOfLevelsCompleted.Add(levelTwoCompleted);
        listOfLevelsCompleted.Add(levelThreeCompleted);
    }

    public void NextLevel()
    {
        // goes to the next scene regardless of current scene, in a circle
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextSceneIndex);
        }
        else
        {
            LoadLevel(0);
        }
    }

    public void LoadLevel(int buildIndexOfSceneToLoad)
    {
        // loads the appropriate level and music
        // audioManager.PlayLevelMusic(buildIndexOfSceneToLoad);
        SceneManager.LoadScene(buildIndexOfSceneToLoad);
    }

    public void MarkLevelStarted(int levelToMark)
    {
        // marks the corresponding level as started for level select
        listOfLevelsCompleted[levelToMark] = true;
    }
}
